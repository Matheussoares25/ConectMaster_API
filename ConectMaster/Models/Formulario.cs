using System.ComponentModel.DataAnnotations;

namespace ConectMaster.Models
{
    public class Formulario
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Nome { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Descricao { get; set; }

        [Required]
        public string CamposJson { get; set; } = "[]";

        public bool Ativo { get; set; } = true;
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataAlteracao { get; set; }
    }
}
