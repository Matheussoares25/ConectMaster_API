using ConectMaster.Bancodedados;
using ConectMaster.Models.Adversos.Acesso;
using ConectMaster.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConectMaster.Controllers
{
    [ApiController]
    [Route("api/veiculos")]
    [Authorize]
    public class VeiculosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VeiculosController(AppDbContext context) => _context = context;

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Veiculos>>> Get()
        {
            var veiculos = await _context.Veiculos
                .AsNoTracking()
                .OrderBy(v => v.Modelo)
                .ToListAsync();

            return Ok(veiculos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Veiculos>> GetById(int id)
        {
            var veiculo = await _context.Veiculos
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == id);

            return veiculo == null ? NotFound() : Ok(veiculo);
        }

        [HttpPost]
        public async Task<ActionResult<Veiculos>> Post([FromBody] Veiculos dto)
        {
            if (!User.TemPermissao("Inserir veiculo"))
                return Forbid();

            if (string.IsNullOrWhiteSpace(dto.Modelo))
                return BadRequest(new { error = "O modelo do veículo é obrigatório." });

            var veiculo = new Veiculos
            {
                Modelo = dto.Modelo.Trim()
            };

            _context.Veiculos.Add(veiculo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = veiculo.Id }, veiculo);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] Veiculos dto)
        {
            if (!User.TemPermissao("Editar veiculo"))
                return Forbid();

            var veiculo = await _context.Veiculos.FirstOrDefaultAsync(v => v.Id == id);
            if (veiculo == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(dto.Modelo))
                return BadRequest(new { error = "O modelo do veículo é obrigatório." });

            veiculo.Modelo = dto.Modelo.Trim();
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!User.TemPermissao("Apagar veiculo"))
                return Forbid();

            var veiculo = await _context.Veiculos.FirstOrDefaultAsync(v => v.Id == id);
            if (veiculo == null)
                return NotFound();

            var possuiAcessos = await _context.Acessos.AnyAsync(a => a.Veiculo == id);
            if (possuiAcessos)
                return Conflict(new { error = "Não é possível excluir um veículo vinculado a acessos." });

            _context.Veiculos.Remove(veiculo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
