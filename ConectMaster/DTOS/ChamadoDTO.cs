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

        [Required]
        [MaxLength(50)]
        public string Categoria { get; set; } = string.Empty;

        [Required]
        public int Prioridade { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Aberto";
    }

    public sealed class ChamadoResponse
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public int Prioridade { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime DataAbertura { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public DateTime? DataFechamento { get; set; }
        public UsuarioResponse? Usuario { get; set; }
    }
}

