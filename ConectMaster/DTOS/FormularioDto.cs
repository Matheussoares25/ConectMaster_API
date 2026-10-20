using System.ComponentModel.DataAnnotations;

namespace ConectMaster.DTOS
{
    public class FormularioDto
    {
        [Required]
        [MaxLength(150)]
        public string Nome { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Descricao { get; set; }

        [Required]
        public string CamposJson { get; set; } = "[]";

        public bool Ativo { get; set; } = true;
    }
}
