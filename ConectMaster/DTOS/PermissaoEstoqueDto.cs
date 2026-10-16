namespace ConectMaster.DTOS
{
    public class PermissaoEstoqueDto
    {
        public bool PodeVisualizar { get; set; }
        public bool PodeCriar { get; set; }
        public bool PodeEditar { get; set; }
        public bool PodeApagar { get; set; }
    }
}
