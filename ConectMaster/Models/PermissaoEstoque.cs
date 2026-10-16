namespace ConectMaster.Models
{
    public class PermissaoEstoque
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuarios? Usuario { get; set; } = null!;

        public int EstoqueId { get; set; }
        public Estoque? Estoque { get; set; } = null!;

        public bool PodeVisualizar { get; set; }
        public bool PodeCriar { get; set; }
        public bool PodeEditar { get; set; }
        public bool PodeApagar { get; set; }
    }
}
