using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ConectMaster.Bancodedados;
using ConectMaster.Models;
using ConectMaster.Helpers;
using ConectMaster.DTOS;

namespace ConectMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DepartamentosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DepartamentosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartamentoDto.DepartamentoResponse>>> Get()
        {
            try
            {
                var departamentos = await _context.Departamentos
                    .Select(d => new DepartamentoDto.DepartamentoResponse
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Descricao = d.Descricao,
                        DataCriacao = d.DataCriacao
                    })
                    .ToListAsync();

                return Ok(departamentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DepartamentoDto.DepartamentoResponse>> GetById(int id)
        {
            try
            {
                var departamento = await _context.Departamentos.FindAsync(id);

                if (departamento == null)
                    return NotFound(new { error = "Departamento não encontrado" });

                return Ok(new DepartamentoDto.DepartamentoResponse
                {
                    Id = departamento.Id,
                    Name = departamento.Name,
                    Descricao = departamento.Descricao,
                    DataCriacao = departamento.DataCriacao
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<DepartamentoDto.DepartamentoResponse>> Post(DepartamentoDto.DepartamentoRequest dto)
        {
            if (!User.TemPermissao("Criar departamentos"))
                return Forbid();

            try
            {
                var exists = await _context.Departamentos.AnyAsync(d => d.Name == dto.Name);
                if (exists)
                    return Conflict(new { error = "Departamento com este nome já existe" });

                var departamento = new Departamento
                {
                    Name = dto.Name,
                    Descricao = dto.Descricao,
                    DataCriacao = DateTime.Now
                };

                _context.Departamentos.Add(departamento);
                await _context.SaveChangesAsync();

                try
                {
                    _context.LogsAuditoria.Add(new LogAuditoria
                    {
                        UsuarioId = User.GetId(),
                        Acao = "Criar",
                        Entidade = "Departamento",
                        EntidadeId = departamento.Id,
                        Detalhes = System.Text.Json.JsonSerializer.Serialize(dto)
                    });
                    await _context.SaveChangesAsync();
                }
                catch { }

                return CreatedAtAction(nameof(GetById), new { id = departamento.Id }, new DepartamentoDto.DepartamentoResponse
                {
                    Id = departamento.Id,
                    Name = departamento.Name,
                    Descricao = departamento.Descricao,
                    DataCriacao = departamento.DataCriacao
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, DepartamentoDto.DepartamentoRequest dto)
        {
            if (!User.TemPermissao("Editar departamentos"))
                return Forbid();

            try
            {
                var departamento = await _context.Departamentos.FindAsync(id);

                if (departamento == null)
                    return NotFound(new { error = "Departamento não encontrado" });

                var exists = await _context.Departamentos.AnyAsync(d => d.Name == dto.Name && d.Id != id);
                if (exists)
                    return Conflict(new { error = "Departamento com este nome já existe" });

                departamento.Name = dto.Name;
                departamento.Descricao = dto.Descricao;

                _context.Departamentos.Update(departamento);
                await _context.SaveChangesAsync();

                try
                {
                    _context.LogsAuditoria.Add(new LogAuditoria
                    {
                        UsuarioId = User.GetId(),
                        Acao = "Editar",
                        Entidade = "Departamento",
                        EntidadeId = departamento.Id,
                        Detalhes = System.Text.Json.JsonSerializer.Serialize(dto)
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
        public async Task<IActionResult> Delete(int id)
        {
            if (!User.TemPermissao("Apagar departamentos"))
                return Forbid();

            try
            {
                var departamento = await _context.Departamentos.FindAsync(id);

                if (departamento == null)
                    return NotFound(new { error = "Departamento não encontrado" });

                _context.Departamentos.Remove(departamento);
                await _context.SaveChangesAsync();

                try
                {
                    _context.LogsAuditoria.Add(new LogAuditoria
                    {
                        UsuarioId = User.GetId(),
                        Acao = "Excluir",
                        Entidade = "Departamento",
                        EntidadeId = id,
                        Detalhes = $"Departamento: {departamento.Name}"
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
