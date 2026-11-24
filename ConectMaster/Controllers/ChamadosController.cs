using ConectMaster.Bancodedados;
using ConectMaster.DTOS;
using ConectMaster.Helpers;
using ConectMaster.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConectMaster.Models.Financeiro;
using ConectMaster.Models.TI;
namespace ConectMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChamadosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ChamadosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChamadoResponse>>> Get()
        {
            if (!User.TemPermissao("Visualizar chamados"))
                return Forbid();

            var chamados = await _context.Chamados
                .Select(c => new ChamadoResponse
                {
                    Id = c.Id,
                    Titulo = c.Titulo,
                    Descricao = c.Descricao,
                    Status = c.Status,
                    DataAbertura = c.DataAbertura,
                    DataAlteracao = c.DataAlteracao,
                    DataFechamento = c.DataFechamento,
                    Categoria = c.Categoria,
                    Prioridade = c.Prioridade,
                    Setor = c.Setor,
                    Email = c.Email,
                    Usuario = c.Usuario == null ? null : new UsuarioResponse
                    {
                        Id = c.Usuario.Id,
                        Nome = c.Usuario.Nome,
                        Email = c.Usuario.Email,
                        DepartamentoId = c.Usuario.DepartamentoId,
                        DepartamentoNome = c.Usuario.Departamento != null ? c.Usuario.Departamento.Nome : null
                    }
                })
                .ToListAsync();

            return Ok(chamados);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ChamadoResponse>> Get(int id)
        {
            var chamado = await _context.Chamados
                .Where(c => c.Id == id)
                .Select(c => new ChamadoResponse
                {
                    Id = c.Id,
                    Titulo = c.Titulo,
                    Descricao = c.Descricao,
                    Status = c.Status,
                    DataAbertura = c.DataAbertura,
                    DataAlteracao = c.DataAlteracao,
                    DataFechamento = c.DataFechamento,
                    Categoria = c.Categoria,
                    Prioridade = c.Prioridade,
                    Setor = c.Setor,
                    Email = c.Email,
                    Usuario = c.Usuario == null ? null : new UsuarioResponse
                    {
                        Id = c.Usuario.Id,
                        Nome = c.Usuario.Nome,
                        Email = c.Usuario.Email,
                        DepartamentoId = c.Usuario.DepartamentoId,
                        DepartamentoNome = c.Usuario.Departamento != null ? c.Usuario.Departamento.Nome : null
                    }
                })
                .FirstOrDefaultAsync();

            if (chamado == null) return NotFound();
            if (!User.TemPermissao("Visualizar chamados")) return Forbid();
            return Ok(chamado);
        }

        [HttpGet("me")]
        public async Task<ActionResult<IEnumerable<ChamadoResponse>>> Me()
        {
            try
            {
                var userId = User.GetId();
                var chamados = await _context.Chamados
                    .Where(c => c.idUsuario == userId)
                    .Include(c => c.Usuario)
                    .ThenInclude(u => u.Departamento)
                    .Select(c => new ChamadoResponse
                    {
                        Id = c.Id,
                        Titulo = c.Titulo,
                        Descricao = c.Descricao,
                        Categoria = c.Categoria,
                        Prioridade = c.Prioridade,
                        Status = c.Status,
                        DataAbertura = c.DataAbertura,
                        DataAlteracao = c.DataAlteracao,
                        DataFechamento = c.DataFechamento,
                        Setor = c.Setor,
                        Email = c.Email,
                        Usuario = new UsuarioResponse
                        {
                            Id = c.Usuario!.Id,
                            Nome = c.Usuario.Nome,
                            Email = c.Usuario.Email,
                            DepartamentoNome = c.Usuario.Departamento != null ? c.Usuario.Departamento.Nome : null
                        }
                    })
                    .ToListAsync();

                return Ok(chamados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("{id}/fechar")]
        public async Task<IActionResult> Fechar(int id, ChamadoDTO chamadoDTO)
        {
            if (!User.TemPermissao("Editar chamados"))
                return Forbid();

            try
            {
                var chamado = await _context.Chamados.FirstOrDefaultAsync(x => x.Id == id);
                if (chamado == null) return NotFound();

                var statusAnterior = chamado.Status;

                chamado.Status = chamadoDTO.Status;
                chamado.DataFechamento = DateTime.UtcNow;
                chamado.DataAlteracao = DateTime.UtcNow ;

                _context.Chamados.Update(chamado);
                await _context.SaveChangesAsync();

                try
                {
                    _context.LogsAuditoria.Add(new LogAuditoria
                    {
                        UsuarioId = User.GetId(),
                        UsuarioName = User.GetName(),
                        Acao = "Fechar",
                        Entidade = "Chamado",
                        EntidadeId = chamado.Id,
                        Detalhes = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            usuario = User.GetName(),
                            Antes = statusAnterior,
                            Depois = chamado.Status
                        })
                    });

                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost()]
        public async Task<ActionResult<ChamadoResponse>> Post(ChamadoCriacaoDto dto)
        {
            if (!User.TemPermissao("Criar chamados"))
                return Forbid();

            try
            {
                var agora = DateTime.UtcNow;
                var chamado = new Chamados_Ti
                {
                    Titulo = dto.Titulo,
                    Descricao = dto.Descricao ?? string.Empty,
                    Categoria = dto.Categoria,
                    Prioridade = dto.Prioridade,
                    Status = dto.Status,
                    Setor = dto.Setor,
                    Email = dto.Email,
                    idUsuario = User.GetId(),
                    DataAbertura = agora,
                    DataAlteracao = agora
                };

                _context.Chamados.Add(chamado);
                await _context.SaveChangesAsync();

                _context.LogsAuditoria.Add(new LogAuditoria
                {
                    UsuarioId = User.GetId(),
                    UsuarioName = User.GetName(),
                    Acao = "Criar",
                    Entidade = "Chamado",
                    EntidadeId = chamado.Id,
                    Detalhes = System.Text.Json.JsonSerializer.Serialize(dto)
                });

                await _context.SaveChangesAsync();

                var usuario = await _context.Usuarios.FindAsync(chamado.idUsuario);
                return CreatedAtAction(nameof(Get), new { id = chamado.Id }, new ChamadoResponse
                {
                    Id = chamado.Id,
                    Titulo = chamado.Titulo,
                    Descricao = chamado.Descricao,
                    Status = chamado.Status,
                    Categoria = chamado.Categoria,
                    Prioridade = chamado.Prioridade,
                    Setor = chamado.Setor,
                    Email = chamado.Email,
                    DataAbertura = chamado.DataAbertura,
                    DataAlteracao = chamado.DataAlteracao
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ChamadoDTO chamadoDTO)
        {
            if (!User.TemPermissao("Editar chamados"))
                return Forbid();

            try
            {
                var chamado = await _context.Chamados.FirstOrDefaultAsync(x => x.Id == id);
                if (chamado == null) return NotFound();

                var statusAnterior = chamado.Status;

                chamado.Status = chamadoDTO.Status;
                chamado.DataAlteracao =  DateTime.UtcNow;

                _context.Chamados.Update(chamado);
                await _context.SaveChangesAsync();

                try
                {
                    _context.LogsAuditoria.Add(new LogAuditoria
                    {
                        UsuarioId = User.GetId(),
                        UsuarioName = User.GetName(),
                        Acao = "Editar",
                        Entidade = "Chamado",
                        EntidadeId = chamado.Id,
                        Detalhes = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            usuario = User.GetName(),
                            Antes = statusAnterior,
                            Depois = chamado.Status
                        })
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

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!User.TemPermissao("Apagar chamados"))
                return Forbid();

            try
            {
                var item = await _context.Chamados.FindAsync(id);
                if (item == null) return NotFound();
                var detalhes = System.Text.Json.JsonSerializer.Serialize(item);

                _context.Chamados.Remove(item);
                await _context.SaveChangesAsync();

                try
                {
                    _context.LogsAuditoria.Add(new LogAuditoria
                    {
                        UsuarioId = User.GetId(),
                        UsuarioName = User.GetName(),
                        Acao = "Excluir",
                        Entidade = "Chamado",
                        EntidadeId = item.Id,
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
