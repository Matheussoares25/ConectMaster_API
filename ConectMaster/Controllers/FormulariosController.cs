using System.Text.Json;
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
    public class FormulariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FormulariosController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Formulario>>> Get()
        {
            if (!User.TemPermissao("Visualizar formularios"))
                return Forbid();

            var formularios = await _context.Formularios
                .AsNoTracking()
                .OrderBy(f => f.Nome)
                .ToListAsync();

            return Ok(formularios);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Formulario>> GetById(int id)
        {
            if (!User.TemPermissao("Visualizar formularios"))
                return Forbid();

            var formulario = await _context.Formularios
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            return formulario == null ? NotFound() : Ok(formulario);
        }

        [HttpPost]
        public async Task<ActionResult<Formulario>> Post([FromBody] FormularioDto dto)
        {
            if (!User.TemPermissao("Criar formularios"))
                return Forbid();

            var erroJson = ValidarCamposJson(dto.CamposJson);
            if (erroJson != null)
                return BadRequest(new { error = erroJson });

            var formulario = new Formulario
            {
                Nome = dto.Nome.Trim(),
                Descricao = dto.Descricao,
                CamposJson = dto.CamposJson,
                Ativo = dto.Ativo,
                DataCriacao = DateTime.UtcNow
            };

            _context.Formularios.Add(formulario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = formulario.Id }, formulario);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] FormularioDto dto)
        {
            if (!User.TemPermissao("Editar formularios"))
                return Forbid();

            var erroJson = ValidarCamposJson(dto.CamposJson);
            if (erroJson != null)
                return BadRequest(new { error = erroJson });

            var formulario = await _context.Formularios
                .FirstOrDefaultAsync(f => f.Id == id);

            if (formulario == null)
                return NotFound();

            formulario.Nome = dto.Nome.Trim();
            formulario.Descricao = dto.Descricao;
            formulario.CamposJson = dto.CamposJson;
            formulario.Ativo = dto.Ativo;
            formulario.DataAlteracao = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!User.TemPermissao("Apagar formularios"))
                return Forbid();

            var formulario = await _context.Formularios
                .FirstOrDefaultAsync(f => f.Id == id);

            if (formulario == null)
                return NotFound();

            _context.Formularios.Remove(formulario);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private static string? ValidarCamposJson(string camposJson)
        {
            try
            {
                using var documento = JsonDocument.Parse(camposJson);
                if (documento.RootElement.ValueKind is not JsonValueKind.Array and not JsonValueKind.Object)
                    return "CamposJson deve conter um objeto ou uma lista JSON.";

                return null;
            }
            catch (JsonException)
            {
                return "CamposJson contém um JSON inválido.";
            }
        }
    }
}
