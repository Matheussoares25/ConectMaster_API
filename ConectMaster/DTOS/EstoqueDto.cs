using System.ComponentModel.DataAnnotations;

namespace ConectMaster.DTOS
{
    public class EstoqueDto
    {
        [Required]
        [MaxLength(150)]
        public string Nome { get; set; } = string.Empty;

        public bool Ativo { get; set; } = true;
    }
}
