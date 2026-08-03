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
    public class PerfilPermissoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PerfilPermissoesController(AppDbContext context)
        {
            _context = context;
        }

        // Lista permissões associadas a um perfil
        [HttpGet("perfil/{perfilId}")]
        public async Task<ActionResult<IEnumerable<Permissao>>> GetPermissoesDoPerfil(int perfilId)
        {
            try
            {
                var exists = await _context.Perfis.AnyAsync(p => p.Id == perfilId);
                if (!exists) return NotFound(new { error = "Perfil não encontrado" });

                var permissoes = await _context.PerfilPermissoes
                    .Where(pp => pp.PerfilId == perfilId)
                    .Include(pp => pp.Permissao)
                    .Select(pp => pp.Permissao)
                    .ToListAsync();

                return permissoes;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // Associa uma permissão a um perfil
        [HttpPost]
        public async Task<ActionResult<PerfilPermissao>> Post(PerfilPermissao model)
        {
          
            if (!User.TemPermissao("Criar permissoes"))
                return Forbid();

            try
            {
                // valida perfis e permissões existentes
                if (!await _context.Perfis.AnyAsync(p => p.Id == model.PerfilId))
                    return NotFound(new { error = "Perfil não encontrado" });

                if (!await _context.Permissoes.AnyAsync(p => p.Id == model.PermissaoId))
                    return NotFound(new { error = "Permissão não encontrada" });

                // evita duplicatas
                var exists = await _context.PerfilPermissoes.AnyAsync(pp => pp.PerfilId == model.PerfilId && pp.PermissaoId == model.PermissaoId);
                if (exists) return Conflict(new { error = "Associação já existe" }); 

                _context.PerfilPermissoes.Add(model);
                await _context.SaveChangesAsync();

                try
                {
                    _context.LogsAuditoria.Add(new Models.LogAuditoria
                    {
                        UsuarioId = User.GetId(),      
                        Acao = "Criar",
                        Entidade = "PerfilPermissao",
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

        // Recupera associação por id
        [HttpGet("{id}")]
        public async Task<ActionResult<PerfilPermissao>> GetById(int id)
        {
            try
            {
                var item = await _context.PerfilPermissoes
                    .Include(pp => pp.Perfil)
                    .Include(pp => pp.Permissao)
                    .FirstOrDefaultAsync(pp => pp.Id == id);

                if (item == null) return NotFound();
                return item;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // Remove associação por perfilId e permissaoId
        [HttpDelete("{permissaoId}")]
        public async Task<IActionResult> Delete( int permissaoId,[FromBody] int perfilId)
        {
            var item = await _context.PerfilPermissoes
                .FirstOrDefaultAsync(x =>
                    x.PerfilId == perfilId &&
                    x.PermissaoId == permissaoId);

            if (item == null)
                return NotFound();

            var detalhes = System.Text.Json.JsonSerializer.Serialize(item);

            _context.PerfilPermissoes.Remove(item);
            await _context.SaveChangesAsync();

            try
            {
                _context.LogsAuditoria.Add(new Models.LogAuditoria
                {
                    UsuarioId = User.GetId(),
                    Acao = "Excluir",
                    Entidade = "PerfilPermissao",
                    EntidadeId = item.Id,
                    Detalhes = detalhes
                });
                await _context.SaveChangesAsync();
            }
            catch { }

            return NoContent();
        }

    }
}
