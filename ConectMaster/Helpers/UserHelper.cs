using System.Security.Claims;

namespace ConectMaster.Helpers
{
    public static class UserHelper
    {
        public static int GetId(this ClaimsPrincipal user)
        {
            if (user == null) return 0;
            var idClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "id");
            if (idClaim == null) return 0;
            if (int.TryParse(idClaim.Value, out var id)) return id;
            return 0;
        }

        public static string GetName(this ClaimsPrincipal user)
        {
            if (user == null)
                return "";

            var nameClaim = user.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.Name || c.Type == "Name");

            return nameClaim?.Value ?? "";
        }
    }
}
