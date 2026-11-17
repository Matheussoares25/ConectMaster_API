using ConectMaster.Bancodedados;
using ConectMaster.DTOS;
using ConectMaster.Helpers;
using ConectMaster.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConectMaster.Models.Financeiro;

namespace ConectMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<Usuarios> _hasher;

        public UsuariosController(AppDbContext context, IPasswordHasher<Usuarios> hasher)
        {
            _context = context;
            _hasher = hasher;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioResponse>>> Get(CancellationToken ct)
        {
            if (!User.TemPermissao("Visualizar usuarios"))
                return Forbid();

            var usuarios = await _context.Usuarios
                .AsNoTracking()
                .Select(u => new UsuarioResponse
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email,
                    Ramal = u.Ramal,
                    Telefone = u.Telefone,
                    DepartamentoId = u.DepartamentoId,
                    DepartamentoNome = u.Departamento != null ? u.Departamento.Nome : null,
                    PerfilId = u.PerfilId,
                    Perfil = u.Perfil != null
                        ? new PerfilResumo { Id = u.Perfil.Id, Name = u.Perfil.Name }
                        : null
                })
                .ToListAsync(ct);

            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioResponse>> Get(int id)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .Where(u => u.Id == id)
                    .Select(u => new UsuarioResponse
                    {
                        Id = u.Id,
                        Nome = u.Nome,
                        Email = u.Email,
                        Ramal = u.Ramal,
                        Telefone = u.Telefone,
                        DepartamentoId = u.DepartamentoId,
                        PerfilId = u.PerfilId
                    })
                    .FirstOrDefaultAsync();
                if (usuario == null) return NotFound();
                return usuario;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioResponse>> Post(UsuarioRequest request, CancellationToken ct)
        {
            if (!User.TemPermissao("Criar Usuario"))
                return Forbid();

            var emailNormalizado = request.Email.Trim().ToLowerInvariant();

            if (await _context.Usuarios.AnyAsync(x => x.Email == emailNormalizado, ct))
                return Conflict(new { error = "Email já cadastrado." });

            if (string.IsNullOrWhiteSpace(request.Senha))
                return BadRequest(new { error = "Senha é obrigatória." });

            var usuario = new Usuarios
            {
                Nome = request.Nome.Trim(),
                Email = emailNormalizado,
                Ramal = request.Ramal,
                Telefone = request.Telefone,
                DepartamentoId = request.DepartamentoId,
                PerfilId = request.PerfilId,
            };
            usuario.Senha = _hasher.HashPassword(usuario, request.Senha);

            _context.Usuarios.Add(usuario);

            // Auditoria na mesma transação do cadastro
            _context.LogsAuditoria.Add(new LogAuditoria
            {
                UsuarioId = User.GetId(),
                UsuarioName = User.GetName(),
                Acao = "Criar",
                Entidade = "Usuario",
                EntidadeId = usuario.Id,   // ainda é 0 aqui, ver abaixo
                Detalhes = $"Usuário '{usuario.Nome}' criado.",   // não serializa o request, pois contém a senha
            });

            await _context.SaveChangesAsync(ct);

            return CreatedAtAction(nameof(Get), new { id = usuario.Id }, new UsuarioResponse
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Ramal = usuario.Ramal,
                Telefone = usuario.Telefone,
                DepartamentoId = usuario.DepartamentoId,
                PerfilId = usuario.PerfilId,
            });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, UsuarioRequest request, CancellationToken ct)
        {
            if (!User.TemPermissao("Editar usuarios"))
                return Forbid();

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (usuario is null)
                return NotFound();

            var emailNormalizado = request.Email.Trim().ToLowerInvariant();

            if (await _context.Usuarios.AnyAsync(x => x.Email == emailNormalizado && x.Id != id, ct))
                return Conflict(new { error = "Email já cadastrado." });

            usuario.Nome = request.Nome.Trim();
            usuario.Email = emailNormalizado;
            usuario.Ramal = request.Ramal;
            usuario.Telefone = request.Telefone;
            usuario.DepartamentoId = request.DepartamentoId;
            usuario.PerfilId = request.PerfilId;

            // Só troca a senha se o campo foi preenchido
            if (!string.IsNullOrWhiteSpace(request.Senha))
                usuario.Senha = _hasher.HashPassword(usuario, request.Senha);

            await using var tx = await _context.Database.BeginTransactionAsync(ct);

            _context.LogsAuditoria.Add(new LogAuditoria
            {
                UsuarioId = User.GetId(),
                UsuarioName = User.GetName(),
                Acao = "Editar",
                Entidade = "Usuario",
                EntidadeId = id,
                // Sem o request serializado: ele contém a senha
                Detalhes = $"Usuário '{usuario.Nome}' (id {id}) atualizado."
                    + (string.IsNullOrWhiteSpace(request.Senha) ? "" : " Senha alterada."),
            });

            await _context.SaveChangesAsync(ct);   // grava usuário e log juntos
            await tx.CommitAsync(ct);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!User.TemPermissao("Apagar usuarios"))
                return Forbid();
            try
            {
                var item = await _context.Usuarios.FindAsync(id);
                if (item == null) return NotFound();

                var detalhes = System.Text.Json.JsonSerializer.Serialize(new
                {
                    item.Id,
                    item.Nome,
                    item.Email,
                    item.Ramal,
                    item.Telefone,
                    item.DepartamentoId,
                    item.PerfilId
                });

                _context.Usuarios.Remove(item);
                await _context.SaveChangesAsync();

                try
                {
                    _context.LogsAuditoria.Add(new LogAuditoria
                    {
                        UsuarioId = User.GetId(),
                        Acao = "Excluir",
                        Entidade = "Usuario",
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
