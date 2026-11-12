namespace ConectMaster.DTOS
{
    public sealed class ServicosResponseDto
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Email { get; set; }
        public string? Setor { get; set; }
        public string? Categoria { get; set; }
        public int Prioridade { get; set; }
        public string? Status { get; set; }
        public DateTime DataAbertura { get; set; }
        public string? Descricao { get; set; }
    }

    public sealed class ServicosRequest
    {
        public string? Titulo { get; set; }
        public string? Email { get; set; }
        public string? Setor { get; set; }
        public string? Categoria { get; set; }
        public int Prioridade { get; set; }
    }

    public class AtualizarStatusDto
    {
        public string Status { get; set; } = string.Empty;
    }
}


