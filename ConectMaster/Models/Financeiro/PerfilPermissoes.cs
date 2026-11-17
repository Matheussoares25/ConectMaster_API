using System.ComponentModel.DataAnnotations.Schema;

namespace ConectMaster.Models.Financeiro
{
    public class PerfilPermissao
    {
        public int Id { get; set; }

        public int PerfilId { get; set; }

        [ForeignKey(nameof(PerfilId))]
        public Perfil? Perfil { get; set; } = null!;

        public int PermissaoId { get; set; }

        [ForeignKey(nameof(PermissaoId))]
        public Permissao? Permissao { get; set; } = null!;
    }
}