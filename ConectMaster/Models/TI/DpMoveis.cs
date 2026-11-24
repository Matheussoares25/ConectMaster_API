using System.ComponentModel.DataAnnotations;

namespace ConectMaster.Models.TI
{
    public class DpMoveis
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Departamento { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Aparelho { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Responsavel { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime DataDoTermo { get; set; }

        [MaxLength(255)]
        public string? NomeArquivoTermo { get; set; }

        public byte[]? ConteudoTermoPdf { get; set; }
    }
}
