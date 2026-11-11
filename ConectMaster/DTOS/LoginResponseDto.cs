namespace ConectMaster.DTOS
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public UsuarioRespostaDto User { get; set; } = new();
        public string Message { get; set; } = string.Empty;
    }

    public class UsuarioRespostaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Ramal { get; set; }
        public string? Telefone { get; set; }
        public string? Setor { get; set; }
        public string? Perfil { get; set; }
        public List<string> Views { get; set; } = new();
    }
}
