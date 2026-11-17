using System.ComponentModel.DataAnnotations;

namespace ConectMaster.DTOS
{
    public class ChamadoDTO
    {
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;
    }

    public class ChamadoCriacaoDto
    {
        [Required]
        [MaxLength(150)]
        public string Titulo { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Descricao { get; set; }

        [MaxLength(50)]
        public string Categoria { get; set; } = string.Empty;

        public int Prioridade { get; set; } = 1;

        [Range(1, int.MaxValue)]
        public int TipoChamadoId { get; set; } = 1;

        public string Setor { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Aberto";

        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; set; } = string.Empty;

    }

    public sealed class ChamadoResponse
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public int Prioridade { get; set; }
        public int TipoChamadoId { get; set; }
        public string TipoChamadoNome { get; set; } = string.Empty;
        public string Setor { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime DataAbertura { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public DateTime? DataFechamento { get; set; }
        public UsuarioResponse? Usuario { get; set; }
    }
}

