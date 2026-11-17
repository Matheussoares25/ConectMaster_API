using ConectMaster.Bancodedados;
using ConectMaster.DTOS;
using ConectMaster.Helpers;
using ConectMaster.Models;
using ConectMaster.Models.Compras;
using ConectMaster.Models.Financeiro;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConectMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChamadosComprasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ChamadosComprasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChamadoComprasResponse>>> Get()
        {
            if (!User.TemPermissao("Visualizar chamados Compras"))
                return Forbid();

            var chamados = await _context.ChamadosCompras
                .Include(chamado => chamado.Usuario)
                    .ThenInclude(usuario => usuario!.Departamento)
                .ToListAsync();

            return Ok(chamados.Select(MapearResposta));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ChamadoComprasResponse>> GetById(int id)
        {
            if (!User.TemPermissao("Visualizar chamados Compras"))
                return Forbid();

            var chamado = await _context.ChamadosCompras
                .Include(item => item.Usuario)
                    .ThenInclude(usuario => usuario!.Departamento)
                .FirstOrDefaultAsync(item => item.Id == id);

            if (chamado == null) return NotFound();
            return Ok(MapearResposta(chamado));
        }

        [HttpGet("me")]
        public async Task<ActionResult<IEnumerable<ChamadoComprasResponse>>> Me()
        {
            var userId = User.GetId();
            var chamados = await _context.ChamadosCompras
                .Where(chamado => chamado.idUsuario == userId)
                .Include(chamado => chamado.Usuario)
                    .ThenInclude(usuario => usuario!.Departamento)
                .ToListAsync();

            return Ok(chamados.Select(MapearResposta));
        }

        [HttpPost]
        public async Task<ActionResult<ChamadoComprasResponse>> Post(ChamadoComprasCriacaoDto dto)
        {
            if (!User.TemPermissao("Abrir chamado Compras"))
                return Forbid();

            var agora = DateTime.UtcNow;
            var chamado = new Chamados_Compras
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao ?? string.Empty,
                Categoria = dto.Categoria,
                Prioridade = dto.Prioridade,
                Setor = dto.Setor,
                Email = dto.Email,
                Status = dto.Status,
                idUsuario = User.GetId(),
                DataAbertura = agora,
                DataAlteracao = agora
            };

            _context.ChamadosCompras.Add(chamado);
            await _context.SaveChangesAsync();
            await RegistrarLogAsync("Criar", chamado.Id, new
            {
                Depois = chamado
            });

            chamado.Usuario = await _context.Usuarios
                .Include(usuario => usuario.Departamento)
                .FirstOrDefaultAsync(usuario => usuario.Id == chamado.idUsuario);

            return CreatedAtAction(nameof(GetById), new { id = chamado.Id }, MapearResposta(chamado));
        }

        [HttpPut("{id:int}/fechar")]
        public async Task<IActionResult> Fechar(int id, ChamadoComprasDTO dto)
        {
            if (!User.TemPermissao("Editar chamados Compras"))
                return Forbid();

            var chamado = await _context.ChamadosCompras.FindAsync(id);
            if (chamado == null) return NotFound();

            var statusAnterior = chamado.Status;
            chamado.Status = dto.Status;
            chamado.DataFechamento = DateTime.UtcNow;
            chamado.DataAlteracao = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            await RegistrarLogAsync("Fechar", chamado.Id, new
            {
                Antes = new { Status = statusAnterior },
                Depois = new { Status = chamado.Status, chamado.DataFechamento }
            });
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, ChamadoComprasDTO dto)
        {
            if (!User.TemPermissao("Editar chamados Compras"))
                return Forbid();

            var chamado = await _context.ChamadosCompras.FindAsync(id);
            if (chamado == null) return NotFound();

            var statusAnterior = chamado.Status;
            chamado.Status = dto.Status;
            chamado.DataAlteracao = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            await RegistrarLogAsync("Editar", chamado.Id, new
            {
                Antes = new { Status = statusAnterior },
                Depois = new { Status = chamado.Status }
            });
            return NoContent();
        }

        [HttpPatch("{id:int}/aprovar")]
        public async Task<IActionResult> Aprovar(int id)
        {
            if (!User.TemPermissao("Aprovar chamados e cotacao Compras"))
                return Forbid();

            var chamado = await _context.ChamadosCompras.FindAsync(id);
            if (chamado == null) return NotFound();

            var aprovadoAnteriormente = chamado.Aprovado;
            var standbyAnterior = chamado.standby;
            chamado.DataAprovacao = DateTime.UtcNow;
            chamado.Aprovado = false;
            chamado.standby = false;
            await _context.SaveChangesAsync();
            await RegistrarLogAsync("Aprovar", chamado.Id, new
            {
                Antes = new { Aprovado = aprovadoAnteriormente, Standby = standbyAnterior },
                Depois = new { chamado.Aprovado, Standby = chamado.standby, chamado.DataAprovacao }
            });
            return NoContent();
        }

        [HttpPatch("{id:int}/standby")]
        public async Task<IActionResult> Standby(int id)
        {
            if (!User.TemPermissao("Editar chamados Compras"))
                return Forbid();
            var chamado = await _context.ChamadosCompras.FindAsync(id);
            if (chamado == null) return NotFound();

            var standbyAnterior = chamado.standby;
            chamado.standby = true;
            await _context.SaveChangesAsync();
            await RegistrarLogAsync("Colocar em standby", chamado.Id, new
            {
                Antes = new { Standby = standbyAnterior },
                Depois = new { Standby = chamado.standby }
            });
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!User.TemPermissao("Apagar chamados Compras"))
                return Forbid();

            var chamado = await _context.ChamadosCompras.FindAsync(id);
            if (chamado == null) return NotFound();

            var detalhes = System.Text.Json.JsonSerializer.Serialize(chamado);
            _context.ChamadosCompras.Remove(chamado);
            await _context.SaveChangesAsync();
            await RegistrarLogAsync("Excluir", id, new { Antes = detalhes });
            return NoContent();
        }

        private async Task RegistrarLogAsync(string acao, int chamadoId, object detalhes)
        {
            _context.LogsAuditoria.Add(new LogAuditoria
            {
                UsuarioId = User.GetId(),
                UsuarioName = User.GetName(),
                Acao = acao,
                Entidade = "Chamado Compras",
                EntidadeId = chamadoId,
                Detalhes = System.Text.Json.JsonSerializer.Serialize(detalhes)
            });

            await _context.SaveChangesAsync();
        }

        private static ChamadoComprasResponse MapearResposta(Chamados_Compras chamado)
        {
            return new ChamadoComprasResponse
            {
                Id = chamado.Id,
                Titulo = chamado.Titulo,
                Descricao = chamado.Descricao,
                Categoria = chamado.Categoria,
                Prioridade = chamado.Prioridade,
                Setor = chamado.Setor,
                Email = chamado.Email,
                Status = chamado.Status,
                Aprovado = chamado.Aprovado,
                standby = chamado.standby,
                DataAprovacao = chamado.DataAprovacao,
                DataAbertura = chamado.DataAbertura,
                DataAlteracao = chamado.DataAlteracao,
                DataFechamento = chamado.DataFechamento,
                Usuario = chamado.Usuario == null ? null : new UsuarioResponse
                {
                    Id = chamado.Usuario.Id,
                    Nome = chamado.Usuario.Nome,
                    Email = chamado.Usuario.Email,
                    DepartamentoId = chamado.Usuario.DepartamentoId,
                    DepartamentoNome = chamado.Usuario.Departamento?.Nome
                }
            };
        }
    }
}
