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

namespace ConectMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginCadController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public LoginCadController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto req)
        {
            try
            {
                var user = await _context.Usuarios.Include(u => u.Perfil).FirstOrDefaultAsync(u => u.Email == req.Email);
                if (user == null) return NotFound(new { error = "Usuário com este email não encontrado", message = "Usuário não encontrado" });

                // Senha em texto por enquanto (substituir por hash em produção)
                if (user.Senha != req.Senha) return Unauthorized(new { error = "Credenciais inválidas", message = "Senha incorreta" });

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
                    Id = user.Id ?? 0,
                    Nome = user.Nome,
                    Email = user.Email,
                    Ramal = user.Ramal,
                    Telefone = user.Telefone,
                    Setor = user.Setor,
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
