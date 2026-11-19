using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConectMaster.Models.Compras
{
    public class Chamados_Compras
    {
        [Key]
        public int Id { get; set; }

        public int idUsuario { get; set; }

        [ForeignKey(nameof(idUsuario))]
        public Usuarios? Usuario { get; set; }

        [MaxLength(100)]
        public string Titulo { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string Descricao { get; set; } = string.Empty;

        public DateTime DataAbertura { get; set; }

        public DateTime DataAlteracao { get; set; }

        public DateTime? DataFechamento { get; set; }

        public DateTime? DataAprovacao { get; set; }

        public bool Aprovado { get; set; } = false;

        public bool standby { get; set; } = false; // aguardando aprovação do gestor

        public int Prioridade { get; set; } = 1;

        
        [MaxLength(100)]
        public string Categoria { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Setor { get; set; } = string.Empty;

        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Status { get; set; } = "Aberto";



    }
}
