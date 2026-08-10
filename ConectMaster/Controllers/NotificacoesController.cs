using ConectMaster.Bancodedados;
using ConectMaster.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConectMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificacoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NotificacoesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("me")]
        public async Task<ActionResult> Me()
        {
            try
            {
                var userId = User.GetId();

                var notifs = await _context.Notificacoes
                    .Where(n => n.UsuarioId == userId && !n.Lida)
                    .Include(n => n.Tipo)
                    .OrderByDescending(n => n.DataCriacao)
                    .Select(n => new
                    {
                        n.Id,
                        n.Mensagem,
                        n.Lida,
                        n.DataCriacao,
                        Tipo = n.Tipo != null ? n.Tipo.Nome : string.Empty
                    })
                    .ToListAsync();

                return Ok(notifs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("lida/{id}")]
        public async Task<IActionResult> MarcarLida(int id)
        {
            try
            {
                var userId = User.GetId();

                var notif = await _context.Notificacoes.FindAsync(id);
                if (notif == null) return NotFound();
                if (notif.UsuarioId != userId) return Forbid();

                notif.Lida = true;
                _context.Notificacoes.Update(notif);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
