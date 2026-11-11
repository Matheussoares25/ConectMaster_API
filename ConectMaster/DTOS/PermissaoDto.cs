using System.ComponentModel.DataAnnotations;

namespace ConectMaster.DTOS
{
    public class PermissaoDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
    }

    public sealed class PermissaoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public sealed class PermissaoRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}


