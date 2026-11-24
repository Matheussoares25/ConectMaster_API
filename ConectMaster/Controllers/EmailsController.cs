using System.ComponentModel.DataAnnotations;
using ConectMaster.Bancodedados;
using ConectMaster.Helpers;
using ConectMaster.Models.TI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConectMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmailsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmailsController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Emails>>> Get()
        {
            if (!User.TemPermissao("Visualizar emails"))
                return Forbid();

            var emails = await _context.Emails
                .AsNoTracking()
                .Include(email => email.Departamento)
                .OrderBy(email => email.Responsavel)
                .ThenBy(email => email.Email)
                .ToListAsync();

            return Ok(emails);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Emails>> GetById(int id)
        {
            if (!User.TemPermissao("Visualizar emails"))
                return Forbid();

            var email = await _context.Emails
                .AsNoTracking()
                .Include(item => item.Departamento)
                .FirstOrDefaultAsync(item => item.Id == id);

            return email == null ? NotFound() : Ok(email);
        }

        [HttpPost]
        public async Task<ActionResult<Emails>> Post([FromBody] Emails request)
        {
            if (!User.TemPermissao("Criar emails"))
                return Forbid();

            var validationError = await ValidateRequest(request);
            if (validationError != null)
                return BadRequest(new { error = validationError });

            var email = new Emails
            {
                Email = request.Email.Trim(),
                Responsavel = request.Responsavel.Trim(),
                Departamentoid = request.Departamentoid
            };

            _context.Emails.Add(email);
            await _context.SaveChangesAsync();
            await _context.Entry(email).Reference(item => item.Departamento).LoadAsync();

            return CreatedAtAction(nameof(GetById), new { id = email.Id }, email);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] Emails request)
        {
            if (!User.TemPermissao("Editar emails"))
                return Forbid();

            var email = await _context.Emails.FirstOrDefaultAsync(item => item.Id == id);
            if (email == null)
                return NotFound();

            var validationError = await ValidateRequest(request);
            if (validationError != null)
                return BadRequest(new { error = validationError });

            email.Email = request.Email.Trim();
            email.Responsavel = request.Responsavel.Trim();
            email.Departamentoid = request.Departamentoid;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!User.TemPermissao("Apagar emails"))
                return Forbid();

            var email = await _context.Emails.FirstOrDefaultAsync(item => item.Id == id);
            if (email == null)
                return NotFound();

            _context.Emails.Remove(email);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private async Task<string?> ValidateRequest(Emails request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                !new EmailAddressAttribute().IsValid(request.Email))
                return "Informe um endereço de e-mail válido.";

            if (string.IsNullOrWhiteSpace(request.Responsavel))
                return "O responsável é obrigatório.";

            if (!await _context.Departamentos.AnyAsync(departamento => departamento.Id == request.Departamentoid))
                return "O departamento informado não existe.";

            return null;
        }
    }
}
