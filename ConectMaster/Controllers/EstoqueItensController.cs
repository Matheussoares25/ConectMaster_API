using ConectMaster.Bancodedados;
using ConectMaster.DTOS;
using ConectMaster.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConectMaster.Controllers
{
    [ApiController]
    [Route("api/estoques/{estoqueId:int}/itens")]
    [Authorize]
    public class EstoqueItensController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EstoqueItensController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstoqueItem>>> Get(int estoqueId)
        {
            var itens = await _context.EstoqueItens
                .AsNoTracking()
                .Where(i => i.EstoqueId == estoqueId)
                .OrderBy(i => i.Nome)
                .ToListAsync();

            return Ok(itens);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EstoqueItem>> GetById(int estoqueId, int id)
        {
            var item = await _context.EstoqueItens
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id && i.EstoqueId == estoqueId);

            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<EstoqueItem>> Post(int estoqueId, [FromBody] EstoqueItemDto dto)
        {
            var item = new EstoqueItem
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                Quantidade = dto.Quantidade,
                EstoqueId = estoqueId
            };

            _context.EstoqueItens.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { estoqueId, id = item.Id }, item);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int estoqueId, int id, [FromBody] EstoqueItemDto dto)
        {
            var item = await _context.EstoqueItens
                .FirstOrDefaultAsync(i => i.Id == id && i.EstoqueId == estoqueId);

            if (item == null)
                return NotFound();

            item.Nome = dto.Nome;
            item.Descricao = dto.Descricao;
            item.Quantidade = dto.Quantidade;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int estoqueId, int id)
        {
            var item = await _context.EstoqueItens
                .FirstOrDefaultAsync(i => i.Id == id && i.EstoqueId == estoqueId);

            if (item == null)
                return NotFound();

            _context.EstoqueItens.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
