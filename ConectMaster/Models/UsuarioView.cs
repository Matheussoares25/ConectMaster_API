using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConectMaster.Models
{
    public class UsuarioView
    {
        [Key]
        public int Id { get; set; }

        // FK para Views
        public int ViewId { get; set; }
        [ForeignKey("ViewId")]
        public Views? View { get; set; }

        // FK para Usuarios
        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public Usuarios? Usuario { get; set; }
    }
}
