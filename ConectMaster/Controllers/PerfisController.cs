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
    public class PerfisController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PerfisController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Perfil>>> Get()
        {
            if (!User.TemPermissao("Visualizar perfis"))
                return Forbid();
            try
            {
                return await _context.Perfis.ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("geral")]
        public async Task<IActionResult> GetGeral()
        {
            if (!User.TemPermissao("Visualizar perfis"))
                return Forbid();
            try
            {
                // Carrega perfis e associações de permissão
                var perfis = await _context.Perfis.ToListAsync();
                var perfilPerms = await _context.PerfilPermissoes
                    .Include(pp => pp.Permissao)
                    .ToListAsync();

                // Conta usuários vinculados a cada perfil
                var usersPerProfile = await _context.Usuarios
                    .Where(u => u.PerfilId != null)
                    .GroupBy(u => u.PerfilId)
                    .Select(g => new { PerfilId = g.Key, Count = g.Count() })
                    .ToListAsync();

                var counts = usersPerProfile.ToDictionary(x => x.PerfilId!.Value, x => x.Count);

                var perfilMap = perfilPerms
                    .GroupBy(pp => pp.PerfilId)
                    .ToDictionary(g => g.Key, g => g.Select(pp => new { pp.PermissaoId, pp.Permissao.Name }).ToList());

                var result = perfis.Select(p => new
                {
                    p.Id,
                    p.Name,
                    UsuariosVinculados = counts.ContainsKey(p.Id) ? counts[p.Id] : 0,
                    Permissoes = perfilMap.ContainsKey(p.Id)
                        ? perfilMap[p.Id].Select(pp => new
                        {
                            Id = pp.PermissaoId,
                            Name = pp.Name,
                        }).Cast<object>().ToList()
                        : new List<object>()
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Perfil>> Get(int id)
        {
            if (!User.TemPermissao("Visualizar perfis"))
                return Forbid();
            try
            {
                var item = await _context.Perfis.FindAsync(id);
                if (item == null) return NotFound();
                return item;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Perfil>> Post(Perfil model)
        {
            if (!User.TemPermissao("Criar perfis"))
                return Forbid();

            try
            {
                _context.Perfis.Add(model);
                await _context.SaveChangesAsync();

                // Log auditoria criar
                try
                {
                    _context.LogsAuditoria.Add(new Models.LogAuditoria
                    {
                        UsuarioId = User.GetId(),
                        Acao = "Criar",
                        Entidade = "Perfil",
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
        public async Task<IActionResult> Put(int id, Perfil model)
        {
            if (!User.TemPermissao("Editar perfis"))
                return Forbid();

            var perfil = await _context.Perfis.FindAsync(id);

            if (perfil == null)
                return NotFound();

            var nomeAntigo = perfil.Name;
            perfil.Name = model.Name;

            await _context.SaveChangesAsync();

            // Log auditoria editar
            try
            {
                _context.LogsAuditoria.Add(new Models.LogAuditoria
                {
                    UsuarioId = User.GetId(),
                    Acao = "Editar",
                    Entidade = "Perfil",
                    EntidadeId = perfil.Id,
                    Detalhes = $"Nome alterado de '{nomeAntigo}' para '{model.Name}'"
                });
                await _context.SaveChangesAsync();
            }
            catch { }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!User.TemPermissao("Apagar perfis"))
                return Forbid();

            try
            {
                var item = await _context.Perfis.FindAsync(id);
                if (item == null) return NotFound();
                var detalhes = System.Text.Json.JsonSerializer.Serialize(item);

                _context.Perfis.Remove(item);
                await _context.SaveChangesAsync();

                try
                {
                    _context.LogsAuditoria.Add(new Models.LogAuditoria
                    {
                        UsuarioId = User.GetId(),
                        Acao = "Excluir",
                        Entidade = "Perfil",
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
