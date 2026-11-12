using System.ComponentModel.DataAnnotations;

namespace ConectMaster.Models
{
    public class Departamento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Descricao { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }
}
