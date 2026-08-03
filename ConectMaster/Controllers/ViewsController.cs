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
    public class ViewsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ViewsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Views>>> Get()
        {
            if (!User.TemPermissao("Visualizar views"))
                return Forbid();

            try
            {
                var items = await _context.Views.ToListAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
