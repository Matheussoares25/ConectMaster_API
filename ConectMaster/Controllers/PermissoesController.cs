using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ConectMaster.Bancodedados;
using ConectMaster.Models;
using ConectMaster.Helpers;

namespace ConectMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PermissoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PermissoesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Permissao>>> Get()
        {
            if (!User.TemPermissao("Visualizar permissoes"))
                return Forbid();
            try
            {
                return await _context.Permissoes
      .OrderBy(p => p.Name)
      .ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Permissao>> Get(int id)
        {
            if (!User.TemPermissao("Visualizar permissoes"))
                return Forbid();

            try
            {
                var item = await _context.Permissoes.FindAsync(id);
                if (item == null) return NotFound();
                return item;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Permissao>> Post(Permissao model)
        {
            if (!User.TemPermissao("Criar permissoes"))
                return Forbid();

            try
            {
                _context.Permissoes.Add(model);
                await _context.SaveChangesAsync();

                // Log de auditoria - Criar
                try
                {
                    _context.LogsAuditoria.Add(new Models.LogAuditoria
                    {
                        UsuarioId = User.GetId(),
                        Acao = "Criar",
                        Entidade = "Permissao",
                        EntidadeId = model.Id,
                        Detalhes = System.Text.Json.JsonSerializer.Serialize(model)
                    });
                    await _context.SaveChangesAsync();
                }
                catch { }

                return CreatedAtAction(nameof(Get), new { id = model.Id }, model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Permissao model)
        {
            if (!User.TemPermissao("Editar permissoes"))
                return Forbid();

            try
            {
                if (id != model.Id) return BadRequest();
                if (!await _context.Permissoes.AnyAsync(x => x.Id == id)) return NotFound();

                _context.Entry(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                // Log de auditoria - Editar
                try
                {
                    _context.LogsAuditoria.Add(new Models.LogAuditoria
                    {
                        UsuarioId = User.GetId(),
                        Acao = "Editar",
                        Entidade = "Permissao",
                        EntidadeId = model.Id,
                        Detalhes = System.Text.Json.JsonSerializer.Serialize(model)
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
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!User.TemPermissao("Apagar permissoes"))
                return Forbid();

            try
            {
                var item = await _context.Permissoes.FindAsync(id);
                if (item == null) return NotFound();
                var detalhes = System.Text.Json.JsonSerializer.Serialize(item);

                _context.Permissoes.Remove(item);
                await _context.SaveChangesAsync();

                try
                {
                    _context.LogsAuditoria.Add(new Models.LogAuditoria
                    {
                        UsuarioId = User.GetId(),
                        Acao = "Excluir",
                        Entidade = "Permissao",
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
