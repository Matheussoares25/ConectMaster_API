using ConectMaster.Bancodedados;
using ConectMaster.DTOS;
using ConectMaster.Helpers;
using ConectMaster.Migrations;
using ConectMaster.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<IEnumerable<chamado>>> Get()
        {
            if (!User.TemPermissao("Visualizar chamados"))
                return Forbid();
            try
            {
                var chamados = await _context.Chamados.Select(c => new
                {
                    c.Id,
                    c.Titulo,
                    c.Descricao,
                    c.Status,
                    c.DataAbertura,
                    c.DataAlteracao,
                    c.Categoria,
                    c.Prioridade,
                    usuario = new UsuarioDTO
                    {
                        Id = c.Usuario.Id,
                        Nome = c.Usuario.Nome,
                        Email = c.Usuario.Email,
                        Setor = c.Usuario.Setor
                    }
                }).ToListAsync();

                return Ok(chamados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<chamado>> Get(int id)
        {
            try
            {
                var item = await _context.Chamados.Include(c => c.Usuario).FirstOrDefaultAsync(c => c.Id == id);
                if (item == null) return NotFound();
                return item;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("me")]
        public async Task<ActionResult> Me()
        {
            try
            {
                var userId = User.GetId();
                var chamados = await _context.Chamados
                    .Where(c => c.idUsuario == userId)
                    .Select(c => new
                    {
                        c.Id,
                        c.Titulo,
                        c.Descricao,
                        c.Status,
                        c.DataAbertura,
                        Usuario = new UsuarioDTO
                        {
                            Id = c.Usuario.Id,
                            Nome = c.Usuario.Nome,
                            Email = c.Usuario.Email
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

        [HttpPost]
        public async Task<ActionResult<chamado>> Post(chamado model)
        {
            if (!User.TemPermissao("Criar chamados"))
                return Forbid();

            try
            {
                model.idUsuario = User.GetId();
                model.DataAbertura = DateTime.Now;

                _context.Chamados.Add(model);
                await _context.SaveChangesAsync();

                _context.LogsAuditoria.Add(new Models.LogAuditoria
                {
                    UsuarioId = User.GetId(),
                    UsuarioName = User.GetName(),
                    Acao = "Criar",
                    Entidade = "Chamado",
                    EntidadeId = model.Id,
                    Detalhes = System.Text.Json.JsonSerializer.Serialize(new
                    {
                        Usuario = User.GetName(), 
                        criou = "novo chamado"
                    })
                });

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { id = model.Id }, model);
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
                var chamado = await _context.Chamados
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (chamado == null)
                    return NotFound();

                var statusAnterior = chamado.Status;

                // Atualiza os campos permitidos
                chamado.Status = chamadoDTO.Status;
                chamado.DataAlteracao = DateTime.Now;

                _context.Chamados.Update(chamado);
                await _context.SaveChangesAsync();

                try
                {
                    _context.LogsAuditoria.Add(new Models.LogAuditoria
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

        [HttpDelete("{id}")]
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
                    _context.LogsAuditoria.Add(new Models.LogAuditoria
                    {
                        UsuarioId = User.GetId(),
                        Acao = "Excluir",
                        Entidade = "Chamado",
                        EntidadeId = id,
                        Detalhes = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            Usuario = User.GetName(),
                            Apagou = item.Titulo,
                            categoria = item.Categoria,

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
    }
}
