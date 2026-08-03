using System.Security.Claims;

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
    }
}
