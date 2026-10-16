using System.Security.Claims;
using ConectMaster.Bancodedados;
using ConectMaster.Models;
using Microsoft.EntityFrameworkCore;

namespace ConectMaster.Helpers
{
    public static class PermissaoHelper
    {
        public static bool TemPermissao(
        this ClaimsPrincipal user,
        string permissao)
        {
            return user.Claims.Any(c =>
                c.Type == "permission" &&
                c.Value == permissao);
        }

        public static async Task<bool> TemPermissaoEstoqueAsync(
            this ClaimsPrincipal user,
            AppDbContext context,
            int estoqueId,
            Func<PermissaoEstoque, bool> acao)
        {
            var usuarioId = user.GetId();
            if (usuarioId <= 0 || estoqueId <= 0)
                return false;

            var permissao = await context.PermissoesEstoques
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId && p.EstoqueId == estoqueId);

            return permissao != null && acao(permissao);
        }
    }
}
