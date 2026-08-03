using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ConectMaster.Bancodedados;
using ConectMaster.Models;

namespace ConectMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LogsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LogsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/logs
        // Opcional: ?entidade=Perfil&usuarioId=1
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? entidade, [FromQuery] int? usuarioId)
        {
            try
            {
                var query = _context.LogsAuditoria.AsQueryable();

                if (!string.IsNullOrEmpty(entidade))
                    query = query.Where(l => l.Entidade == entidade);

                if (usuarioId.HasValue && usuarioId.Value > 0)
                    query = query.Where(l => l.UsuarioId == usuarioId.Value);

                var list = await query
                    .OrderByDescending(l => l.DataHora)
                    .ToListAsync();

                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
