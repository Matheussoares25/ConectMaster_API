using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ConectMaster.Bancodedados;
using ConectMaster.Models;
using ConectMaster.Helpers;
using ConectMaster.DTOS;

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
        public async Task<ActionResult<IEnumerable<PermissaoResponse>>> Get()
        {
            if (!User.TemPermissao("Visualizar permissoes"))
                return Forbid();
            try
            {
                return await _context.Permissoes
                    .OrderBy(p => p.Name)
                    .Select(p => new PermissaoResponse { Id = p.Id, Name = p.Name })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PermissaoResponse>> Get(int id)
        {
            if (!User.TemPermissao("Visualizar permissoes"))
                return Forbid();

            try
            {
                var item = await _context.Permissoes.FindAsync(id);
                if (item == null) return NotFound();

                return new PermissaoResponse { Id = item.Id, Name = item.Name };
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<PermissaoResponse>> Post(PermissaoDto request)
        {
            if (!User.TemPermissao("Criar permissoes"))
                return Forbid();

            try
            {
                var model = new Permissao { Name = request.Name };
                _context.Permissoes.Add(model);
                await _context.SaveChangesAsync();

                try
                {
                    _context.LogsAuditoria.Add(new LogAuditoria
                    {
                        UsuarioId = User.GetId(),
                        UsuarioName = User.GetName(),
                        Acao = "Criar",
                        Entidade = "Permissao",
                        EntidadeId = model.Id,
                        Detalhes = System.Text.Json.JsonSerializer.Serialize(request)
                    });
                    await _context.SaveChangesAsync();
                }
                catch { }

                return CreatedAtAction(nameof(Get), new { id = model.Id }, new PermissaoResponse { Id = model.Id, Name = model.Name });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, PermissaoDto request)
        {
            if (!User.TemPermissao("Editar permissoes"))
                return Forbid();

            try
            {
                var item = await _context.Permissoes.FindAsync(id);
                if (item == null) return NotFound();

                item.Name = request.Name;
                _context.Permissoes.Update(item);
                await _context.SaveChangesAsync();

                try
                {
                    _context.LogsAuditoria.Add(new LogAuditoria
                    {
                        UsuarioId = User.GetId(),
                        UsuarioName = User.GetName(),
                        Acao = "Editar",
                        Entidade = "Permissao",
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
                    _context.LogsAuditoria.Add(new LogAuditoria
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
