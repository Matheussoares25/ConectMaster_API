using ConectMaster.Bancodedados;
using ConectMaster.DTOS;
using ConectMaster.Helpers;
using ConectMaster.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConectMaster.Controllers
{
    [ApiController]
    [Route("api/estoques")]
    [Authorize]
    public class EstoquesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EstoquesController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estoque>>> Get()
        {
            var estoques = await _context.Estoques
                .AsNoTracking()
                .OrderBy(e => e.Nome)
                .ToListAsync();

            return Ok(estoques);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Estoque>> GetById(int id)
        {
            var estoque = await _context.Estoques.AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            return estoque == null ? NotFound() : Ok(estoque);
        }

        [HttpPost]
        public async Task<ActionResult<Estoque>> Post([FromBody] EstoqueDto dto)
        {
            if (!User.TemPermissao("Criar formularios"))
                return Forbid();

            var nome = dto.Nome.Trim();
            if (string.IsNullOrWhiteSpace(nome))
                return BadRequest(new { error = "O nome do estoque é obrigatório." });

            if (await _context.Estoques.AnyAsync(e => e.Nome == nome))
                return Conflict(new { error = "Já existe um estoque com esse nome." });

            var estoque = new Estoque { Nome = nome, Ativo = dto.Ativo };
            _context.Estoques.Add(estoque);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = estoque.Id }, estoque);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] EstoqueDto dto)
        {
            var estoque = await _context.Estoques.FirstOrDefaultAsync(e => e.Id == id);
            if (estoque == null)
                return NotFound();

            var nome = dto.Nome.Trim();
            if (string.IsNullOrWhiteSpace(nome))
                return BadRequest(new { error = "O nome do estoque é obrigatório." });

            if (await _context.Estoques.AnyAsync(e => e.Id != id && e.Nome == nome))
                return Conflict(new { error = "Já existe um estoque com esse nome." });

            estoque.Nome = nome;
            estoque.Ativo = dto.Ativo;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var estoque = await _context.Estoques.FirstOrDefaultAsync(e => e.Id == id);
            if (estoque == null)
                return NotFound();
            if (await _context.EstoqueItens.AnyAsync(i => i.EstoqueId == id))
                return Conflict(new { error = "Não é possível excluir um estoque que possui itens." });

            _context.Estoques.Remove(estoque);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
