using System.ComponentModel.DataAnnotations;

namespace ConectMaster.DTOS
{
    public class EstoqueItemDto
    {
        [Required]
        [MaxLength(150)]
        public string Nome { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Descricao { get; set; }

        [Range(typeof(decimal), "0", "999999999999")]
        public decimal Quantidade { get; set; }
    }

    public sealed class EstoqueItemResponse
    {
        public int Id { get; set; }
        public int EstoqueId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal Quantidade { get; set; }
    }

    public sealed class EstoqueItemRequest
    {
        [Required]
        [MaxLength(150)]
        public string Nome { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Descricao { get; set; }

        [Range(typeof(decimal), "0", "999999999999")]
        public decimal Quantidade { get; set; }
    }
}

