using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConectMaster.Models
{
    public class Historico
    {
        [Key]
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        [ForeignKey(nameof(UsuarioId))]
        public Usuarios Usuario { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string TipoServico { get; set; } = string.Empty;

        public DateTime DataAbertura { get; set; }

        public DateTime? DataSolucao { get; set; }
    }
}