using ConectMaster.Bancodedados;
using ConectMaster.Helpers;
using ConectMaster.Models.TI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ConectMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DpMoveisController : ControllerBase
    {
        private const int MaxFileSize = 10 * 1024 * 1024;

        private readonly AppDbContext _context;

        public DpMoveisController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (!User.TemPermissao("Visualizar DpMoveis"))
                return Forbid();

            var registros = await _context.DpMoveis
                .AsNoTracking()
                .OrderBy(item => item.Departamento)
                .ThenBy(item => item.Responsavel)
                .Select(item => new
                {
                    item.Id,
                    item.Departamento,
                    item.Aparelho,
                    item.Responsavel,
                    item.DataDoTermo,
                    item.NomeArquivoTermo,
                    PossuiTermoPdf = item.ConteudoTermoPdf != null && item.ConteudoTermoPdf.Length > 0
                })
                .ToListAsync();

            return Ok(registros);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!User.TemPermissao("Visualizar DpMoveis"))
                return Forbid();

            var registro = await _context.DpMoveis
                .AsNoTracking()
                .Where(item => item.Id == id)
                .Select(item => new
                {
                    item.Id,
                    item.Departamento,
                    item.Aparelho,
                    item.Responsavel,
                    item.DataDoTermo,
                    item.NomeArquivoTermo,
                    PossuiTermoPdf = item.ConteudoTermoPdf != null && item.ConteudoTermoPdf.Length > 0
                })
                .FirstOrDefaultAsync();

            return registro == null ? NotFound() : Ok(registro);
        }

        [HttpGet("{id:int}/termo")]
        public async Task<IActionResult> DownloadTermo(int id)
        {
            if (!User.TemPermissao("Visualizar DpMoveis"))
                return Forbid();

            var registro = await _context.DpMoveis
                .AsNoTracking()
                .Where(item => item.Id == id)
                .Select(item => new { item.ConteudoTermoPdf, item.NomeArquivoTermo })
                .FirstOrDefaultAsync();

            if (registro == null)
                return NotFound();

            if (registro.ConteudoTermoPdf == null || registro.ConteudoTermoPdf.Length == 0)
                return NotFound(new { error = "Este cadastro não possui um termo PDF." });

            return File(registro.ConteudoTermoPdf, "application/pdf", registro.NomeArquivoTermo ?? "termo.pdf");
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(MaxFileSize + 64 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize + 64 * 1024)]
        public async Task<IActionResult> Post(
            [FromForm] string departamento,
            [FromForm] string aparelho,
            [FromForm] string responsavel,
            [FromForm] DateTime dataDoTermo,
            [FromForm] IFormFile? termoPdf)
        {
            if (!User.TemPermissao("Criar DpMoveis"))
                return Forbid();

            if (string.IsNullOrWhiteSpace(departamento) ||
                string.IsNullOrWhiteSpace(aparelho) ||
                string.IsNullOrWhiteSpace(responsavel))
                return BadRequest(new { error = "Departamento, aparelho e responsável são obrigatórios." });

            if (departamento.Trim().Length > 100 || aparelho.Trim().Length > 150 || responsavel.Trim().Length > 100)
                return BadRequest(new { error = "Um ou mais campos excedem o tamanho permitido." });

            if (termoPdf == null || termoPdf.Length == 0)
                return BadRequest(new { error = "Anexe o termo em formato PDF." });

            if (termoPdf.Length > MaxFileSize)
                return BadRequest(new { error = "O PDF deve ter no máximo 10 MB." });

            if (!string.Equals(Path.GetExtension(termoPdf.FileName), ".pdf", StringComparison.OrdinalIgnoreCase))
                return BadRequest(new { error = "O arquivo anexado deve ter extensão PDF." });

            var nomeArquivo = Path.GetFileName(termoPdf.FileName.Replace('\\', '/'));
            if (nomeArquivo.Length > 255)
                return BadRequest(new { error = "O nome do arquivo deve ter no máximo 255 caracteres." });

            await using var memoryStream = new MemoryStream((int)termoPdf.Length);
            await termoPdf.CopyToAsync(memoryStream);
            var conteudo = memoryStream.ToArray();

            if (conteudo.Length < 5 || Encoding.ASCII.GetString(conteudo, 0, 5) != "%PDF-")
                return BadRequest(new { error = "O conteúdo do arquivo não é um PDF válido." });

            var registro = new DpMoveis
            {
                Departamento = departamento.Trim(),
                Aparelho = aparelho.Trim(),
                Responsavel = responsavel.Trim(),
                DataDoTermo = dataDoTermo,
                NomeArquivoTermo = nomeArquivo,
                ConteudoTermoPdf = conteudo
            };

            _context.DpMoveis.Add(registro);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = registro.Id }, new
            {
                registro.Id,
                registro.Departamento,
                registro.Aparelho,
                registro.Responsavel,
                registro.DataDoTermo,
                registro.NomeArquivoTermo,
                PossuiTermoPdf = true
            });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] DpMoveis request)
        {
            if (!User.TemPermissao("Editar DpMoveis"))
                return Forbid();

            var registro = await _context.DpMoveis.FirstOrDefaultAsync(item => item.Id == id);
            if (registro == null)
                return NotFound();

            registro.Departamento = request.Departamento.Trim();
            registro.Aparelho = request.Aparelho.Trim();
            registro.Responsavel = request.Responsavel.Trim();
            registro.DataDoTermo = request.DataDoTermo;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!User.TemPermissao("Apagar DpMoveis"))
                return Forbid();

            var registro = await _context.DpMoveis.FirstOrDefaultAsync(item => item.Id == id);
            if (registro == null)
                return NotFound();

            _context.DpMoveis.Remove(registro);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
