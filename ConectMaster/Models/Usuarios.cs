using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security;

namespace ConectMaster.Models
{
    public class Usuarios
    {
        [Key]
        public int? Id { get; set; }


        [MaxLength(100)]
        public string? Nome { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Senha { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string? Email { get; set; } = string.Empty;

        [MaxLength(10)]
        public string? Ramal { get; set; }

        [MaxLength(20)]
        public string? Telefone { get; set; }

        [MaxLength(100)]
        public string? Setor { get; set; }

        // Perfil
        public int? PerfilId { get; set; }
        [ForeignKey("PerfilId")]
        public Perfil? Perfil { get; set; }
        // Placeholder: propriedade viewidpermissao removida — usar tabela separada PerfilView para permissões de visualização.
        // Comentário mantido para consistência do modelo.
    }
}