using System.ComponentModel.DataAnnotations;

namespace ConectMaster.Models
{
    public class EstoqueItem
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Nome { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Descricao { get; set; }

        [Range(typeof(decimal), "0", "999999999999")]
        public decimal Quantidade { get; set; }

        public int EstoqueId { get; set; }
        public Estoque? Estoque { get; set; } = null!;
    }
}
