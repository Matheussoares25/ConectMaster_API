using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConectMaster.Bancodedados;
using ConectMaster.Models;
using ConectMaster.DTOS;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace ConectMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginCadController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly IPasswordHasher<Usuarios> _hasher;

        public LoginCadController(AppDbContext context, IConfiguration config, IPasswordHasher<Usuarios> hasher)
        {
            _context = context;
            _config = config;
            _hasher = hasher;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto req)
        {
            try
            {
                var user = await _context.Usuarios
    .Include(u => u.Perfil)
    .Include(u => u.Departamento)
    .FirstOrDefaultAsync(u => u.Email == req.Email);
                if (user == null || string.IsNullOrEmpty(req.Senha))
                    return Unauthorized(new { error = "E-mail ou senha inválidos." });

                if (!IsIdentityPasswordHash(user.Senha))
                {
                    if (!string.Equals(user.Senha, req.Senha, StringComparison.Ordinal))
                        return Unauthorized(new { error = "E-mail ou senha inválidos." });

                    user.Senha = _hasher.HashPassword(user, req.Senha);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var resultadoSenha = _hasher.VerifyHashedPassword(user, user.Senha, req.Senha);
                    if (resultadoSenha == PasswordVerificationResult.Failed)
                        return Unauthorized(new { error = "E-mail ou senha inválidos." });

                    if (resultadoSenha == PasswordVerificationResult.SuccessRehashNeeded)
                    {
                        user.Senha = _hasher.HashPassword(user, req.Senha);
                        await _context.SaveChangesAsync();
                    }
                }

                // carregar permissões do perfil
                var permissoes = await _context.PerfilPermissoes
                    .Where(pp => pp.PerfilId == user.PerfilId)
                    .Include(pp => pp.Permissao)
                    .Select(pp => pp.Permissao.Name)
                    .ToListAsync();

                var views = await _context.UsuarioView
                    .Where(pv => pv.UsuarioId == user.Id)
                    .Include(pv => pv.View)
                    .Select(pv => pv.View.Name)
                    .ToListAsync();

                var token = GenerateToken(user, user.Perfil?.Name ?? string.Empty, permissoes);

                var resultUser = new UsuarioRespostaDto
                {
                    Id = user.Id,
                    Nome = user.Nome,
                    Email = user.Email,
                    Ramal = user.Ramal,
                    Telefone = user.Telefone,
                    DepartamentoId = user.DepartamentoId,
                    DepartamentoNome = user.Departamento?.Nome,
                    Perfil = user.Perfil?.Name,
                    Views = views
                };

                return Ok(new LoginResponseDto
                {
                    Token = token,
                    User = resultUser,
                    Message = "Login realizado com sucesso"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, message = "Erro ao autenticar" });
            }
        }

        private static bool IsIdentityPasswordHash(string storedPassword)
        {
            try
            {
                var hashBytes = Convert.FromBase64String(storedPassword);
                return hashBytes.Length >= 49 && hashBytes[0] == 0
                    || hashBytes.Length >= 61 && hashBytes[0] == 1;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private string GenerateToken(Usuarios user, string perfilName, List<string> permissoes)
        {
            var key = _config["Jwt:Key"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Nome),
                new Claim(ClaimTypes.Email, user.Email)
            };

            if (!string.IsNullOrEmpty(perfilName))
            {
                claims.Add(new Claim(ClaimTypes.Role, perfilName));
            }

            // adicionar permissões como claims separadas
            foreach (var p in permissoes)
            {
                claims.Add(new Claim("permission", p));
            }

            var keyBytes = Encoding.UTF8.GetBytes(key);
            var securityKey = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = creds,
                Issuer = issuer,
                Audience = audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
