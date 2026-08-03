using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConectMaster.Bancodedados;
using ConectMaster.Models;
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

        public class LoginRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Senha { get; set; } = string.Empty;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> Login([FromBody] LoginRequest req)
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

                // não retornar a senha
                //não retorna id
                var resultUser = new
                {
                    user.Id,
                    user.Nome,
                    user.Email,
                    user.Ramal,
                    user.Telefone,
                    user.Setor,
                    Perfil = user.Perfil?.Name,
                    Views = views
                };

                return Ok(new { token, user = resultUser, message = "Login realizado com sucesso" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, message = "Erro ao autenticar" });
            }
        }

        private string GenerateToken(Usuarios user, string perfilName, List<string> permissoes)
        {
            var key = _config["Jwt:Key"] ?? "CHANGE_THIS_SECRET_KEY_1234567890";
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
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = creds,
                Issuer = issuer,
                Audience = audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
