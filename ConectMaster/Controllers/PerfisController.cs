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
    public class PerfisController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PerfisController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PerfilResponse>>> Get()
        {
            if (!User.TemPermissao("Visualizar perfis"))
                return Forbid();
            try
            {
                var perfis = await _context.Perfis
                    .Select(p => new PerfilResponse
                    {
                        Id = p.Id,
                        Name = p.Name
                    })
                    .ToListAsync();
                return Ok(perfis);
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
                var perfis = await _context.Perfis.ToListAsync();
                var perfilPerms = await _context.PerfilPermissoes
                    .Include(pp => pp.Permissao)
                    .ToListAsync();

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
        public async Task<ActionResult<PerfilResponse>> Get(int id)
        {
            if (!User.TemPermissao("Visualizar perfis"))
                return Forbid();
            try
            {
                var item = await _context.Perfis
                    .Where(p => p.Id == id)
                    .Select(p => new PerfilResponse
                    {
                        Id = p.Id,
                        Name = p.Name
                    })
                    .FirstOrDefaultAsync();
                if (item == null) return NotFound();
                return item;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<PerfilResponse>> Post(PerfilRequest request)
        {
            if (!User.TemPermissao("Criar perfis"))
                return Forbid();

            try
            {
                var perfil = new Perfil
                {
                    Name = request.Name
                };

                _context.Perfis.Add(perfil);
                await _context.SaveChangesAsync();

                try
                {
                    _context.LogsAuditoria.Add(new Models.LogAuditoria
                    {
                        UsuarioId = User.GetId(),
                        UsuarioName = User.GetName(),
                        Acao = "Criar",
                        Entidade = "Perfil",
                        EntidadeId = perfil.Id,
                        Detalhes = System.Text.Json.JsonSerializer.Serialize(request)
                    });
                    await _context.SaveChangesAsync();
                }
                catch { }

                return CreatedAtAction(nameof(Get), new { id = perfil.Id }, new PerfilResponse
                {
                    Id = perfil.Id,
                    Name = perfil.Name
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, PerfilRequest request)
        {
            if (!User.TemPermissao("Editar perfis"))
                return Forbid();

            var perfil = await _context.Perfis.FindAsync(id);

            if (perfil == null)
                return NotFound();

            var nomeAntigo = perfil.Name;
            perfil.Name = request.Name;

            await _context.SaveChangesAsync();

            try
            {
                _context.LogsAuditoria.Add(new Models.LogAuditoria
                {
                    UsuarioId = User.GetId(),
                    UsuarioName = User.GetName(),
                    Acao = "Editar",
                    Entidade = "Perfil",
                    EntidadeId = perfil.Id,
                    Detalhes = $"Nome alterado de '{nomeAntigo}' para '{request.Name}'"
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
                        UsuarioName = User.GetName(),
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
