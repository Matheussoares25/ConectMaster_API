using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ConectMaster.Models
{
    public class chamado
    {
        [Key]
        public int Id { get; set; }

        public int idUsuario { get; set; }
        [ForeignKey("idUsuario")]
        public Usuarios? Usuario { get; set; }

        [MaxLength(100)]
        public string Titulo { get; set; }

        [MaxLength(2000)]
        public string Descricao { get; set; }

        public DateTime DataAbertura { get; set; }
    
        public DateTime DataAlteracao { get; set; }

        public int Prioridade { get; set; } = 1;

        [MaxLength(100)]
        public string Categoria { get; set; } 

        [MaxLength(100)]
        public string Setor { get; set;  }

        [MaxLength(150)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }  


    }
}
