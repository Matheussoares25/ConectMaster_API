using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConectMaster.Models
{
    public class Notificacao
    {
        [Key]
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuarios? Usuario { get; set; }

        public int TipoNotificacaoId { get; set; }

        [ForeignKey("TipoNotificacaoId")]
        public TipoNotificacao? Tipo { get; set; }

        [MaxLength(2000)]
        public string Mensagem { get; set; }

        public bool Lida { get; set; } = false;

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }
}
