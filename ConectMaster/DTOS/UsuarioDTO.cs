using System.ComponentModel.DataAnnotations;

namespace ConectMaster.DTOS
{
    // Entrada: criar e editar
    public sealed class UsuarioRequest
    {
        [Required, MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        // Só preenchida ao criar, ou ao trocar a senha
        [MinLength(6), MaxLength(100)]
        public string? Senha { get; set; }

        [MaxLength(10)]
        public string? Ramal { get; set; }

        [MaxLength(20)]
        public string? Telefone { get; set; }

        public int? DepartamentoId { get; set; }

        public int? PerfilId { get; set; }
    }

    // Saída: nunca inclui senha
    public sealed class UsuarioResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Ramal { get; set; }
        public string? Telefone { get; set; }
        public int? DepartamentoId { get; set; }
        public string? DepartamentoNome { get; set; }
        public int? PerfilId { get; set; }
        public PerfilResumo? Perfil { get; set; }
    }

    public sealed class PerfilResumo
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}