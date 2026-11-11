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
    [Route("api/[controller]")]
    [Authorize]
    public class ServicosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServicosController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Servico>> Get(int id)
        {
            try
            {
                var item = await _context.Servicos.FirstOrDefaultAsync(s => s.Id == id);
                if (item == null) return NotFound();
                return item;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicosResponseDto>>> Get()
        {
            if (!User.TemPermissao("Visualizar servicos"))
                return Forbid();

            var servicos = await _context.Servicos
                .OrderByDescending(o => o.DataAbertura)
                .Select(s => new ServicosResponseDto
                {
                    Id = s.Id,
                    Titulo = s.Titulo,
                    Email = s.Email,
                    Setor = s.Setor,
                    Categoria = s.Categoria,
                    Prioridade = s.Prioridade,
                    Status = s.Status,
                    DataAbertura = s.DataAbertura,
                    Descricao = s.Descricao
                })
                .ToListAsync();

            return Ok(servicos);
        }

        [HttpPost]
        public async Task<ActionResult<ServicosResponseDto>> Post([FromBody] OrdemServicoFreteDto dto)
        {
            if (!User.TemPermissao("Criar servico"))
                return Forbid();

            var servico = new Servico
            {
                Titulo = dto.Titulo,
                Email = dto.Email,
                Setor = dto.Setor,
                Categoria = dto.Categoria,
                Prioridade = dto.Prioridade,

                ClienteNome = dto.Cliente?.Nome,
                ClienteDocumento = dto.Cliente?.Documento,
                ClienteTelefone = dto.Cliente?.Telefone,
                ClienteContato = dto.Cliente?.Contato,

                DescricaoCarga = dto.Carga?.Descricao,
                PesoBruto = dto.Carga?.PesoBruto,
                Volume = dto.Carga?.Volume,
                QtdVolumes = dto.Carga?.QtdVolumes,
                ValorMercadoria = dto.Carga?.ValorMercadoria,
                NaturezaCarga = dto.Carga?.NaturezaCarga,

                EnderecoColeta = dto.Rota?.EnderecoColeta,
                EnderecoEntrega = dto.Rota?.EnderecoEntrega,
                DataColeta = dto.Rota?.DataColeta,
                DataEntrega = dto.Rota?.DataEntrega,

                PlacaVeiculo = dto.Transporte?.PlacaVeiculo,
                TipoVeiculo = dto.Transporte?.TipoVeiculo,
                MotoristaNome = dto.Transporte?.MotoristaNome,
                MotoristaTelefone = dto.Transporte?.MotoristaTelefone,

                NumeroNfe = dto.Fiscal?.NumeroNfe,
                NumeroCte = dto.Fiscal?.NumeroCte,

                ValorFrete = dto.Valores?.ValorFrete,
                FormaPagamento = dto.Valores?.FormaPagamento,

                Descricao = dto.Descricao,
                SolicitanteId = User.GetId(),
                Status = "Pendente",
                DataAbertura = DateTime.Now
            };

            _context.Servicos.Add(servico);
            await _context.SaveChangesAsync();

            _context.LogsAuditoria.Add(new LogAuditoria
            {
                UsuarioId = User.GetId(),
                UsuarioName = User.GetName(),
                Acao = "Criar",
                Entidade = "Servico",
                EntidadeId = servico.Id,
                Detalhes = System.Text.Json.JsonSerializer.Serialize(dto)
            });

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = servico.Id }, new ServicosResponseDto
            {
                Id = servico.Id,
                Titulo = servico.Titulo,
                Email = servico.Email,
                Setor = servico.Setor,
                Categoria = servico.Categoria,
                Prioridade = servico.Prioridade,
                Status = servico.Status,
                DataAbertura = servico.DataAbertura
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Servico model)
        {
            if (!User.TemPermissao("Autorizar ou Negar OS"))
                return Forbid();
            try
            {
                if (id != model.Id) return BadRequest();
                if (!await _context.Servicos.AnyAsync(x => x.Id == id)) return NotFound();

                _context.Entry(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                try
                {
                    _context.LogsAuditoria.Add(new Models.LogAuditoria
                    {
                        UsuarioId = User.GetId(),
                        Acao = "Editar",
                        Entidade = "Servico",
                        EntidadeId = model.Id,
                        Detalhes = System.Text.Json.JsonSerializer.Serialize(model)
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var item = await _context.Servicos.FindAsync(id);
                if (item == null) return NotFound();

                var detalhes = System.Text.Json.JsonSerializer.Serialize(item);

                _context.Servicos.Remove(item);
                await _context.SaveChangesAsync();

                try
                {
                    _context.LogsAuditoria.Add(new Models.LogAuditoria
                    {
                        UsuarioId = User.GetId(),
                        Acao = "Excluir",
                        Entidade = "Servico",
                        EntidadeId = id,
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
