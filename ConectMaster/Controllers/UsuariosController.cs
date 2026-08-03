using ConectMaster.Bancodedados;
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
        public async Task<ActionResult<IEnumerable<Usuarios>>> Get()
        {
            if (!User.TemPermissao("Visualizar usuarios"))
                return Forbid();

            try
            {
                // Busca usuários com seus perfis
                var usuariosComPerfis = await _context.Usuarios
                    .Include(u => u.Perfil)
                    .ToListAsync();


                // Pega os IDs dos usuários
                var userIds = usuariosComPerfis
                    .Where(u => u.Id != null)
                    .Select(u => u.Id!.Value)
                    .ToList();


                // Busca os vínculos PerfilView junto com a View
                var perfilViews = await _context.UsuarioView
                    .Where(pv => userIds.Contains(pv.UsuarioId))
                    .Include(pv => pv.View)
                    .ToListAsync();


                // Agrupa as views por usuário
                var viewsMap = perfilViews
                    .GroupBy(pv => pv.UsuarioId)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(pv => (object)new
                        {
                            Id = pv.Id, // ID DO VÍNCULO PerfilView
                            ViewId = pv.ViewId,
                            Name = pv.View != null ? pv.View.Name : ""
                        })
                        .ToList()
                    );


                // Monta a resposta final
                var resposta = usuariosComPerfis.Select(u => new
                {

                    u.Id,
                    u.Nome,
                    u.Email,
                    u.Ramal,
                    u.Telefone,
                    u.Setor,

                    Perfil = u.Perfil != null
                        ? new
                        {
                            u.Perfil.Id,
                            u.Perfil.Name
                        }
                        : null,


                    Views = viewsMap.ContainsKey(u.Id ?? 0)
                        ? viewsMap[u.Id ?? 0]
                        : new List<object>()
                });


                return Ok(new
                {
                    Usuarios = resposta
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuarios>> Get(int id)
        {
            try
            {
                var item = await _context.Usuarios.FindAsync(id);
                if (item == null) return NotFound();
                return item;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Usuarios>> Post(Usuarios usuario)
        {
            if (!User.TemPermissao("Criar Usuario"))
                return Forbid();

            try
            {
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                try
                {
                    _context.LogsAuditoria.Add(new Models.LogAuditoria
                    {
                        UsuarioId = User.GetId(),
                        Acao = "Criar",
                        Entidade = "Usuario",
                        EntidadeId = usuario.Id,
                        Detalhes = System.Text.Json.JsonSerializer.Serialize(usuario)
                    });
                    await _context.SaveChangesAsync();
                }
                catch { }

                return CreatedAtAction(nameof(Get), new { id = usuario.Id }, usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Usuarios usuario)
        {
            if (!User.TemPermissao("Editar usuarios"))
                return Forbid();

            try
            {
                if (!await _context.Usuarios.AnyAsync(x => x.Id == id))
                    return NotFound();

                usuario.Id = id;

                _context.Entry(usuario).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                try
                {
                    _context.LogsAuditoria.Add(new Models.LogAuditoria
                    {
                        UsuarioId = User.GetId(),
                        Acao = "Editar",
                        Entidade = "Usuario",
                        EntidadeId = id,
                        Detalhes = System.Text.Json.JsonSerializer.Serialize(usuario)
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
