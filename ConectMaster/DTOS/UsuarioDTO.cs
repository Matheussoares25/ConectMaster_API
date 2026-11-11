using System.ComponentModel.DataAnnotations;

namespace ConectMaster.DTOS
{
    public class UsuarioDTO
    {
        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Ramal { get; set; }

        [MaxLength(20)]
        public string? Telefone { get; set; }

        [MaxLength(100)]
        public string? Setor { get; set; }

        public int? PerfilId { get; set; }
    }

    public sealed class UsuarioResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Ramal { get; set; }
        public string? Telefone { get; set; }
        public string? Setor { get; set; }
        public int? PerfilId { get; set; }
    }

    public sealed class UsuarioRequest
    {
        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Ramal { get; set; }

        [MaxLength(20)]
        public string? Telefone { get; set; }

        [MaxLength(100)]
        public string? Setor { get; set; }

        public int? PerfilId { get; set; }
    }
}


