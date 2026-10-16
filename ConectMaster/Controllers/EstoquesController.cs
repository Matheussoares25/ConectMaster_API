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
            var usuarioId = User.GetId();
            if (usuarioId <= 0)
                return Forbid();

            var estoques = await _context.PermissoesEstoques
                .AsNoTracking()
                .Where(p => p.UsuarioId == usuarioId && p.PodeVisualizar)
                .Select(p => p.Estoque!)
                .OrderBy(e => e.Nome)
                .ToListAsync();

            return Ok(estoques);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Estoque>> GetById(int id)
        {
            if (!await User.TemPermissaoEstoqueAsync(_context, id, p => p.PodeVisualizar))
            {
                if (!await _context.Estoques.AnyAsync(e => e.Id == id))
                    return NotFound();

                return Forbid();
            }

            var estoque = await _context.Estoques.AsNoTracking()
                .FirstAsync(e => e.Id == id);

            return Ok(estoque);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<Estoque>> Post([FromBody] EstoqueDto dto)
        {
            var nome = dto.Nome.Trim();
            if (string.IsNullOrWhiteSpace(nome))
                return BadRequest(new { error = "O nome do estoque é obrigatório." });

            if (await _context.Estoques.AnyAsync(e => e.Nome == nome))
                return Conflict(new { error = "Já existe um estoque com esse nome." });

            var usuarioId = User.GetId();
            if (usuarioId <= 0 || !await _context.Usuarios.AnyAsync(u => u.Id == usuarioId))
                return Forbid();

            await using var transaction = await _context.Database.BeginTransactionAsync();

            var estoque = new Estoque { Nome = nome, Ativo = dto.Ativo };
            _context.Estoques.Add(estoque);
            await _context.SaveChangesAsync();

            var permissaoCriador = new PermissaoEstoque
            {
                UsuarioId = usuarioId,
                EstoqueId = estoque.Id,
                PodeVisualizar = true,
                PodeCriar = true,
                PodeEditar = true,
                PodeApagar = true
            };
            _context.PermissoesEstoques.Add(permissaoCriador);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return CreatedAtAction(nameof(GetById), new { id = estoque.Id }, estoque);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] EstoqueDto dto)
        {
            if (!await User.TemPermissaoEstoqueAsync(_context, id, p => p.PodeEditar))
            {
                if (!await _context.Estoques.AnyAsync(e => e.Id == id))
                    return NotFound();

                return Forbid();
            }

            var nome = dto.Nome.Trim();
            if (string.IsNullOrWhiteSpace(nome))
                return BadRequest(new { error = "O nome do estoque é obrigatório." });

            if (await _context.Estoques.AnyAsync(e => e.Id != id && e.Nome == nome))
                return Conflict(new { error = "Já existe um estoque com esse nome." });

            var estoque = await _context.Estoques.FirstAsync(e => e.Id == id);
            estoque.Nome = nome;
            estoque.Ativo = dto.Ativo;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await User.TemPermissaoEstoqueAsync(_context, id, p => p.PodeApagar))
            {
                if (!await _context.Estoques.AnyAsync(e => e.Id == id))
                    return NotFound();

                return Forbid();
            }

            var estoque = await _context.Estoques.FirstAsync(e => e.Id == id);
            if (await _context.EstoqueItens.AnyAsync(i => i.EstoqueId == id))
                return Conflict(new { error = "Não é possível excluir um estoque que possui itens." });

            _context.Estoques.Remove(estoque);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id:int}/permissoes")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<IEnumerable<PermissaoEstoque>>> GetPermissoes(int id)
        {
            if (!await _context.Estoques.AnyAsync(e => e.Id == id))
                return NotFound();

            var permissoes = await _context.PermissoesEstoques
                .AsNoTracking()
                .Where(p => p.EstoqueId == id)
                .OrderBy(p => p.UsuarioId)
                .ToListAsync();

            return Ok(permissoes);
        }

        [HttpPut("{id:int}/permissoes/{usuarioId:int}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> PutPermissao(int id, int usuarioId, [FromBody] PermissaoEstoqueDto dto)
        {
            if (!await _context.Estoques.AnyAsync(e => e.Id == id))
                return NotFound(new { error = "Estoque não encontrado." });

            if (!await _context.Usuarios.AnyAsync(u => u.Id == usuarioId))
                return NotFound(new { error = "Usuário não encontrado." });

            var permissao = await _context.PermissoesEstoques
                .FirstOrDefaultAsync(p => p.EstoqueId == id && p.UsuarioId == usuarioId);

            if (permissao == null)
            {
                permissao = new PermissaoEstoque { EstoqueId = id, UsuarioId = usuarioId };
                _context.PermissoesEstoques.Add(permissao);
            }

            permissao.PodeVisualizar = dto.PodeVisualizar;
            permissao.PodeCriar = dto.PodeCriar;
            permissao.PodeEditar = dto.PodeEditar;
            permissao.PodeApagar = dto.PodeApagar;

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
