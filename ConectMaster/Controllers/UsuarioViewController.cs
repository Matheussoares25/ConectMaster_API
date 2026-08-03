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
    [Authorize]
    public class UsuarioViewController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuarioViewController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioView>>> Get()
        {
            if (!User.TemPermissao("Visualizar perfilviews"))
                return Forbid();
            try
            {
                return await _context.UsuarioView
                    .Include(pv => pv.View)
                    .Include(pv => pv.Usuario)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("me")]
        public async Task<ActionResult<IEnumerable<string>>> Me()
        {
            try
            {
                var userId = User.GetId();

                var views = await _context.UsuarioView
                    .Where(pv => pv.UsuarioId == userId)
                    .Include(pv => pv.View)
                    .Select(pv => pv.View.Name)
                    .ToListAsync();

                return views;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioView>> Post(UsuarioView model)
        {
            if (!User.TemPermissao("Criar usuarioviews"))
                return Forbid();

            try
            {
                // valida usuario e view existentes
                if (!await _context.Usuarios.AnyAsync(u => u.Id == model.UsuarioId))
                    return NotFound(new { error = "Usuário não encontrado" });

                if (!await _context.Views.AnyAsync(v => v.Id == model.ViewId))
                    return NotFound(new { error = "View não encontrada" });

                // evita duplicatas
                var exists = await _context.UsuarioView.AnyAsync(pv => pv.UsuarioId == model.UsuarioId && pv.ViewId == model.ViewId);
                if (exists) return Conflict(new { error = "Associação já existe" });

                _context.UsuarioView.Add(model);
                await _context.SaveChangesAsync();

                try
                {
                    _context.LogsAuditoria.Add(new LogAuditoria
                    {
                        UsuarioId = User.GetId(),
                        Acao = "Criar",
                        Entidade = "PerfilView",
                        EntidadeId = model.Id,
                        Detalhes = System.Text.Json.JsonSerializer.Serialize(model)
                    });
                    await _context.SaveChangesAsync();
                }
                catch { }

                // retorna a associação criada
                return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioView>> GetById(int id)
        {
            try
            {
                var item = await _context.UsuarioView
                    .Include(pv => pv.View)
                    .Include(pv => pv.Usuario)
                    .FirstOrDefaultAsync(pv => pv.Id == id);

                if (item == null) return NotFound();
                return item;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

            // Remove associação por viewId + usuarioId (evita precisar do id internamente)
            [HttpDelete("{viewId}")]
            public async Task<IActionResult> DeleteByKeys( int viewId, [FromBody] int usuarioId)
            {
                if (!User.TemPermissao("Apagar usuarioviws"))
                    return Forbid();

                try
                {
                    var item = await _context.UsuarioView
                        .FirstOrDefaultAsync(x => x.ViewId == viewId && x.UsuarioId == usuarioId);

                    if (item == null) return NotFound();

                    var detalhes = System.Text.Json.JsonSerializer.Serialize(item);

                    _context.UsuarioView.Remove(item);
                    await _context.SaveChangesAsync();

                    try
                    {
                        _context.LogsAuditoria.Add(new LogAuditoria
                        {
                            UsuarioId = User.GetId(),
                            Acao = "Excluir",
                            Entidade = "UsuarioView",
                            EntidadeId = item.Id,
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
