using ConectMaster.Bancodedados;
using ConectMaster.DTOS;
using ConectMaster.Models.Adversos.Acesso;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConectMaster.Controllers
{
    [ApiController]
    [Route("api/acessos")]
    [Authorize]
    public class AcessosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AcessosController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Acesso>>> Get()
        {
            var acessos = await _context.Acessos
                .AsNoTracking()
                .Include(a => a.Veiculos)
                .OrderByDescending(a => a.Data_entrada)
                .ToListAsync();

            return Ok(acessos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Acesso>> GetById(int id)
        {
            var acesso = await _context.Acessos
                .AsNoTracking()
                .Include(a => a.Veiculos)
                .FirstOrDefaultAsync(a => a.Id == id);

            return acesso == null ? NotFound() : Ok(acesso);
        }

        [HttpPost]
        [AllowAnonymous]
       
        public async Task<ActionResult<Acesso>> Post([FromBody] AcessoDto dto)
        {
            if (dto.Veiculo.HasValue && !await _context.Veiculos.AnyAsync(v => v.Id == dto.Veiculo.Value))
                return BadRequest(new { error = "O veículo informado não existe." });

            var acesso = Map(dto);
            _context.Acessos.Add(acesso);
            await _context.SaveChangesAsync();

            await _context.Entry(acesso).Reference(a => a.Veiculos).LoadAsync();
            return CreatedAtAction(nameof(GetById), new { id = acesso.Id }, acesso);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] AcessoDto dto)
        {
            var acesso = await _context.Acessos.FirstOrDefaultAsync(a => a.Id == id);
            if (acesso == null)
                return NotFound();

            if (dto.Veiculo.HasValue && !await _context.Veiculos.AnyAsync(v => v.Id == dto.Veiculo.Value))
                return BadRequest(new { error = "O veículo informado não existe." });

            Update(acesso, dto);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var acesso = await _context.Acessos.FirstOrDefaultAsync(a => a.Id == id);
            if (acesso == null)
                return NotFound();

            _context.Acessos.Remove(acesso);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}/entrada")]
        public async Task<IActionResult> RegistrarEntrada(int id)
        {
            var acesso = await _context.Acessos.FirstOrDefaultAsync(a => a.Id == id);
            if (acesso == null)
                return NotFound();

            acesso.Data_entrada = DateTime.UtcNow;
            acesso.Verificado = true;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}/saida")]
        public async Task<IActionResult> RegistrarSaida(int id)
        {
            var acesso = await _context.Acessos.FirstOrDefaultAsync(a => a.Id == id);
            if (acesso == null)
                return NotFound();

            acesso.Data_saida = DateTime.UtcNow;
            acesso.Liberado = false;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private static Acesso Map(AcessoDto dto)
        {
            var acesso = new Acesso();
            Update(acesso, dto);
            return acesso;
        }

        private static void Update(Acesso acesso, AcessoDto dto)
        {
            acesso.Verificado = dto.Verificado;
            acesso.Observacao = dto.Observacao?.Trim();
            acesso.Liberado = dto.Liberado;
            acesso.Data_entrada = dto.DataEntrada;
            acesso.Despachante = dto.Despachante.Trim();
            acesso.Data_saida = dto.DataSaida;
            acesso.Servico = dto.Servico;
            acesso.Transportadora = dto.Transportadora;
            acesso.Motorista = dto.Motorista;
            acesso.Cnh = dto.Cnh;
            acesso.Placa = dto.Placa;
            acesso.Veiculo = dto.Veiculo;
            acesso.Veiculos = null;
            acesso.Veiculo_aberto = dto.VeiculoAberto;
            acesso.Numero_nf = dto.NumeroNf;
            acesso.Quantidade = dto.Quantidade;
            acesso.Duracao = dto.Duracao;
            acesso.Telefone_motorista = dto.TelefoneMotorista;
        }
    }
}
