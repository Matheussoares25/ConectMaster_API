using ConectMaster.Bancodedados;
using ConectMaster.DTOS;
using ConectMaster.Helpers;
using ConectMaster.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConectMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioResponse>>> Get()
        {
            if (!User.TemPermissao("Visualizar usuarios"))
                return Forbid();

            try
            {
                var usuariosComPerfis = await _context.Usuarios
                    .Include(u => u.Perfil)
                    .Select(u => new UsuarioResponse
                    {
                        Id = u.Id ?? 0,
                        Nome = u.Nome,
                        Email = u.Email,
                        Ramal = u.Ramal,
                        Telefone = u.Telefone,
                        Setor = u.Setor,
                        PerfilId = u.PerfilId
                    })
                    .ToListAsync();

            

                return Ok(usuariosComPerfis);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioResponse>> Get(int id)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .Where(u => u.Id == id)
                    .Select(u => new UsuarioResponse
                    {
                        Id = u.Id ?? 0,
                        Nome = u.Nome,
                        Email = u.Email,
                        Ramal = u.Ramal,
                        Telefone = u.Telefone,
                        Setor = u.Setor,
                        PerfilId = u.PerfilId
                    })
                    .FirstOrDefaultAsync();
                if (usuario == null) return NotFound();
                return usuario;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioResponse>> Post(UsuarioDTO request)
        {
            if (!User.TemPermissao("Criar Usuario"))
                return Forbid();

            try
            {
                var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == request.Email);
                if (usuarioExistente != null) 
                    return BadRequest(new { error = "Email já cadastrado" });

                var usuario = new Usuarios
                {
                    Nome = request.Nome,
                    Email = request.Email,
                    Ramal = request.Ramal,
                    Telefone = request.Telefone,
                    Setor = request.Setor,
                    PerfilId = request.PerfilId
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                try
                {
                    _context.LogsAuditoria.Add(new Models.LogAuditoria
                    {
                        UsuarioId = User.GetId(),
                        UsuarioName = User.GetName(),
                        Acao = "Criar",
                        Entidade = "Usuario",
                        EntidadeId = usuario.Id,
                        Detalhes = System.Text.Json.JsonSerializer.Serialize(request)
                    });
                    await _context.SaveChangesAsync();
                }
                catch { }

                return CreatedAtAction(nameof(Get), new { id = usuario.Id }, new UsuarioResponse
                {
                    Id = usuario.Id ?? 0,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Ramal = usuario.Ramal,
                    Telefone = usuario.Telefone,
                    Setor = usuario.Setor,
                    PerfilId = usuario.PerfilId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UsuarioDTO request)
        {
            if (!User.TemPermissao("Editar usuarios"))
                return Forbid();

            try
            {
                var usuarioBanco = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);

                if (usuarioBanco == null) return NotFound();

                var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == request.Email && x.Id != id);

                if (usuarioExistente != null) return BadRequest(new { error = "Email já cadastrado" });

                usuarioBanco.Nome = request.Nome;
                usuarioBanco.Email = request.Email;
                usuarioBanco.Ramal = request.Ramal;
                usuarioBanco.Telefone = request.Telefone;
                usuarioBanco.Setor = request.Setor;
                usuarioBanco.PerfilId = request.PerfilId;

                await _context.SaveChangesAsync();

                try
                {
                    _context.LogsAuditoria.Add(new Models.LogAuditoria
                    {
                        UsuarioId = User.GetId(),
                        UsuarioName = User.GetName(),
                        Acao = "Editar",
                        Entidade = "Usuario",
                        EntidadeId = id,
                        Detalhes = System.Text.Json.JsonSerializer.Serialize(request)
                    });
                    await _context.SaveChangesAsync();
                }
                catch { }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!User.TemPermissao("Apagar usuarios"))
                return Forbid();
            try
            {
                var item = await _context.Usuarios.FindAsync(id);
                if (item == null) return NotFound();

                var detalhes = System.Text.Json.JsonSerializer.Serialize(item);

                _context.Usuarios.Remove(item);
                await _context.SaveChangesAsync();

                try
                {
                    _context.LogsAuditoria.Add(new Models.LogAuditoria
                    {
                        UsuarioId = User.GetId(),
                        Acao = "Excluir",
                        Entidade = "Usuario",
                        EntidadeId = id,
                        Detalhes = detalhes
                    });
                    await _context.SaveChangesAsync();
                }
                catch { }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
