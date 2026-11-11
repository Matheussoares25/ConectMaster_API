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
    public class PerfilPermissoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PerfilPermissoesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("perfil/{perfilId}")]
        public async Task<ActionResult<IEnumerable<PerfilPermissaoResponse>>> GetPermissoesDoPerfil(int perfilId)
        {
            try
            {
                var exists = await _context.Perfis.AnyAsync(p => p.Id == perfilId);
                if (!exists) return NotFound(new { error = "Perfil não encontrado" });

                var permissoes = await _context.PerfilPermissoes
                    .Where(pp => pp.PerfilId == perfilId)
                    .Include(pp => pp.Permissao)
                    .Include(pp => pp.Perfil)
                    .Select(pp => new PerfilPermissaoResponse
                    {
                        Id = pp.Id,
                        PerfilId = pp.PerfilId,
                        PermissaoId = pp.PermissaoId,
                        PerfilNome = pp.Perfil.Name,
                        PermissaoNome = pp.Permissao.Name
                    })
                    .ToListAsync();

                return permissoes;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<PerfilPermissaoResponse>> Post(PerfilPermissaoDto dto)
        {
            if (!User.TemPermissao("Criar permissoes"))
                return Forbid();

            try
            {
                if (!await _context.Perfis.AnyAsync(p => p.Id == dto.PerfilId))
                    return NotFound(new { error = "Perfil não encontrado" });

                if (!await _context.Permissoes.AnyAsync(p => p.Id == dto.PermissaoId))
                    return NotFound(new { error = "Permissão não encontrada" });

                var exists = await _context.PerfilPermissoes.AnyAsync(pp => pp.PerfilId == dto.PerfilId && pp.PermissaoId == dto.PermissaoId);
                if (exists) return Conflict(new { error = "Associação já existe" });

                var model = new PerfilPermissao
                {
                    PerfilId = dto.PerfilId,
                    PermissaoId = dto.PermissaoId
                };

                _context.PerfilPermissoes.Add(model);
                await _context.SaveChangesAsync();

                try
                {
                    _context.LogsAuditoria.Add(new LogAuditoria
                    {
                        UsuarioId = User.GetId(),
                        Acao = "Criar",
                        Entidade = "PerfilPermissao",
                        EntidadeId = model.Id,
                        Detalhes = System.Text.Json.JsonSerializer.Serialize(dto)
                    });
                    await _context.SaveChangesAsync();
                }
                catch { }

                var perfil = await _context.Perfis.FindAsync(model.PerfilId);
                var permissao = await _context.Permissoes.FindAsync(model.PermissaoId);

                return CreatedAtAction(nameof(GetById), new { id = model.Id }, new PerfilPermissaoResponse
                {
                    Id = model.Id,
                    PerfilId = model.PerfilId,
                    PermissaoId = model.PermissaoId,
                    PerfilNome = perfil?.Name,
                    PermissaoNome = permissao?.Name
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PerfilPermissaoResponse>> GetById(int id)
        {
            try
            {
                var item = await _context.PerfilPermissoes
                    .Include(pp => pp.Perfil)
                    .Include(pp => pp.Permissao)
                    .FirstOrDefaultAsync(pp => pp.Id == id);

                if (item == null) return NotFound();

                return new PerfilPermissaoResponse
                {
                    Id = item.Id,
                    PerfilId = item.PerfilId,
                    PermissaoId = item.PermissaoId,
                    PerfilNome = item.Perfil?.Name,
                    PermissaoNome = item.Permissao?.Name
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] PerfilPermissaoDto dto)
        {
            if (!User.TemPermissao("Apagar permissoes"))
                return Forbid();

            try
            {
                var item = await _context.PerfilPermissoes
                    .FirstOrDefaultAsync(x => x.PerfilId == dto.PerfilId && x.PermissaoId == dto.PermissaoId);

                if (item == null)
                    return NotFound();

                var detalhes = System.Text.Json.JsonSerializer.Serialize(dto);

                _context.PerfilPermissoes.Remove(item);
                await _context.SaveChangesAsync();

                try
                {
                    _context.LogsAuditoria.Add(new LogAuditoria
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
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
