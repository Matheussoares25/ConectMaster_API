using ConectMaster.Bancodedados;

namespace ConectMaster.Helpers
{
    public static class Notificar
    {
        public static void EnviarNotificacao(AppDbContext context, int usuarioId, int tipoNotificacaoId, string mensagem)
        {
            if (context == null) return;

            var notificacao = new Models.Notificacao
            {
                UsuarioId = usuarioId,
                TipoNotificacaoId = tipoNotificacaoId,
                Mensagem = mensagem,
                Lida = false,
                DataCriacao = DateTime.UtcNow
            };

            context.Notificacoes.Add(notificacao);
            context.SaveChanges();
        }
    }
}
