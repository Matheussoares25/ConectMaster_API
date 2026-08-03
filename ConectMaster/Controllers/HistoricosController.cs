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
    public class HistoricosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HistoricosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Historico>>> Get()
        {
            if (!User.TemPermissao("Visualizar historicos"))
                return Forbid();

            try
            {
                return await _context.Historicos.Include(h => h.Usuario).ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Historico>> Get(int id)
        {

            try
            {
                var item = await _context.Historicos.Include(h => h.Usuario).FirstOrDefaultAsync(h => h.Id == id);
                if (item == null) return NotFound();
                return item;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Historico>> Post(Historico model)
        {

            try
            {
                _context.Historicos.Add(model);
                await _context.SaveChangesAsync();

                try
                {
                    _context.LogsAuditoria.Add(new Models.LogAuditoria
                    {
                        UsuarioId = User.GetId(),
                        Acao = "Criar",
                        Entidade = "Historico",
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
        public async Task<IActionResult> Put(int id, Historico model)
        {
            if (!User.TemPermissao("Editar historicos"))
                return Forbid();
            try
            {
                if (id != model.Id) return BadRequest();
                if (!await _context.Historicos.AnyAsync(x => x.Id == id)) return NotFound();

                _context.Entry(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                try
                {
                    _context.LogsAuditoria.Add(new Models.LogAuditoria
                    {
                        UsuarioId = User.GetId(),
                        Acao = "Editar",
                        Entidade = "Historico",
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
            if (!User.TemPermissao("Apagar historicos"))
                return Forbid();
            try
            {
                var item = await _context.Historicos.FindAsync(id);
                if (item == null) return NotFound();

                var detalhes = System.Text.Json.JsonSerializer.Serialize(item);

                _context.Historicos.Remove(item);
                await _context.SaveChangesAsync();

                try
                {
                    _context.LogsAuditoria.Add(new Models.LogAuditoria
                    {
                        UsuarioId = User.GetId(),
                        Acao = "Excluir",
                        Entidade = "Historico",
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
