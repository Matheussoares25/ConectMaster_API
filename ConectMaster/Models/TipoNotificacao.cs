using System.ComponentModel.DataAnnotations;

namespace ConectMaster.Models
{
    public class TipoNotificacao
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Nome { get; set; }
    }
}
