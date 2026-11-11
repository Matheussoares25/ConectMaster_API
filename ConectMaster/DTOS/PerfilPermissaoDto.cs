using System.ComponentModel.DataAnnotations;

namespace ConectMaster.DTOS
{
    public class PerfilPermissaoDto
    {
        [Required]
        public int PerfilId { get; set; }

        [Required]
        public int PermissaoId { get; set; }
    }

    public sealed class PerfilPermissaoResponse
    {
        public int Id { get; set; }
        public int PerfilId { get; set; }
        public int PermissaoId { get; set; }
        public string? PerfilNome { get; set; }
        public string? PermissaoNome { get; set; }
    }
}

