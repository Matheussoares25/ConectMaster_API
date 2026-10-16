using Microsoft.EntityFrameworkCore;
using ConectMaster.Models;

namespace ConectMaster.Bancodedados
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<chamado> Chamados { get; set; }

        public DbSet<Servico> Servicos { get; set; }

        public DbSet<Historico> Historicos { get; set; }

        public DbSet<Perfil> Perfis { get; set; }
        public DbSet<Permissao> Permissoes { get; set; }
        public DbSet<PerfilPermissao> PerfilPermissoes { get; set; }
        public DbSet<LogAuditoria> LogsAuditoria { get; set; }
        public DbSet<Views> Views { get; set; }
        public DbSet<UsuarioView> UsuarioView { get; set; }
        public DbSet<TipoNotificacao> TipoNotificacoes { get; set; }
        public DbSet<Notificacao> Notificacoes { get; set; }
        public DbSet<Estoque> Estoques { get; set; }
        public DbSet<PermissaoEstoque> PermissoesEstoques { get; set; }
        public DbSet<EstoqueItem> EstoqueItens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Views>().HasData(
                new Views { Id = 1, Name = "visual.abrirchamado" },
                new Views { Id = 2, Name = "visual.ordemservico" },
                new Views { Id = 3, Name = "visual.historicochamados" },
                new Views { Id = 4, Name = "visual.painelcontrole" },
                new Views { Id = 5, Name = "visual.abriruber" },
                new Views { Id = 6, Name = "visual.geralusuarios" },
                new Views { Id = 7, Name = "visual.permissoes" },
                new Views { Id = 8, Name = "visual.relatorios" },
                new Views { Id = 9, Name = "visual.logs" },
                new Views { Id = 10, Name = "visual.configuracoes" },
                new Views { Id = 11 , Name = "visual.servicosgeral"},
                new Views { Id = 12, Name = "visual.portaria"},
                new Views { Id = 13, Name = "visual.formularios"},
                new Views { Id = 14, Name = "visual.estoques"}
            );



            modelBuilder.Entity<Permissao>().HasData(
                new Permissao { Id = 1, Name = "Visualizar chamados" },
                new Permissao { Id = 2, Name = "Criar chamados" },
                new Permissao { Id = 3, Name = "Editar chamados" },
                new Permissao { Id = 4, Name = "Apagar chamados" },

                new Permissao { Id = 5, Name = "Visualizar historicos" },
                new Permissao { Id = 6, Name = "Editar historicos" },
                new Permissao { Id = 7, Name = "Apagar historicos" },

                new Permissao { Id = 8, Name = "Visualizar permissoes" },
                new Permissao { Id = 9, Name = "Criar permissoes" },
                new Permissao { Id = 10, Name = "Editar permissoes" },
                new Permissao { Id = 11, Name = "Apagar permissoes" },

                new Permissao { Id = 12, Name = "Visualizar perfis" },
                new Permissao { Id = 13, Name = "Criar perfis" },
                new Permissao { Id = 14, Name = "Editar perfis" },
                new Permissao { Id = 15, Name = "Apagar perfis" },

                new Permissao { Id = 16, Name = "Visualizar usuarios" },
                new Permissao { Id = 17, Name = "Criar Usuario" },
                new Permissao { Id = 18, Name = "Editar usuarios" },
                new Permissao { Id = 19, Name = "Apagar usuarios" },

                new Permissao { Id = 20, Name = "Visualizar perfilviews" },
                new Permissao { Id = 21, Name = "Criar usuarioviews" },
                new Permissao { Id = 22, Name = "Apagar perfilviews" },
                new Permissao { Id = 23, Name = "Apagar usuarioviws" },

                new Permissao { Id = 24, Name = "Visualizar views" },
                new Permissao { Id = 25, Name = "Criar servico"},
                new Permissao { Id = 26, Name = "Visualizar servicos" },
                new Permissao { Id = 27, Name = "Autorizar ou Negar OS" },

                new Permissao { Id = 28, Name = "Visualizar estoques"}
            );

            // Seed perfil Administrador
            modelBuilder.Entity<Perfil>().HasData(
                new Perfil { Id = 1, Name = "Administrador" },
                new Perfil { Id = 2, Name = "Lider"}
            );

            modelBuilder.Entity<Estoque>().HasData(
                new Estoque { Id = 1, Nome = "TI", Ativo = true },
                new Estoque { Id = 2, Nome = "Compras", Ativo = true },
                new Estoque { Id = 3, Nome = "Expedicao", Ativo = true }
            );

            modelBuilder.Entity<PermissaoEstoque>()
                .HasIndex(p => new { p.UsuarioId, p.EstoqueId })
                .IsUnique();

            modelBuilder.Entity<PermissaoEstoque>().HasData(
                new PermissaoEstoque { Id = 1, UsuarioId = 1, EstoqueId = 1, PodeVisualizar = true, PodeCriar = true, PodeEditar = true, PodeApagar = true },
                new PermissaoEstoque { Id = 2, UsuarioId = 1, EstoqueId = 2, PodeVisualizar = true, PodeCriar = true, PodeEditar = true, PodeApagar = true },
                new PermissaoEstoque { Id = 3, UsuarioId = 1, EstoqueId = 3, PodeVisualizar = true, PodeCriar = true, PodeEditar = true, PodeApagar = true },
                new PermissaoEstoque { Id = 4, UsuarioId = 2, EstoqueId = 1, PodeVisualizar = true, PodeCriar = true, PodeEditar = true, PodeApagar = true },
                new PermissaoEstoque { Id = 5, UsuarioId = 2, EstoqueId = 2, PodeVisualizar = true, PodeCriar = true, PodeEditar = true, PodeApagar = true },
                new PermissaoEstoque { Id = 6, UsuarioId = 2, EstoqueId = 3, PodeVisualizar = true, PodeCriar = true, PodeEditar = true, PodeApagar = true }
            );

            // Atribui todas as permissões ao perfil Administrador
            modelBuilder.Entity<PerfilPermissao>().HasData(
                new PerfilPermissao { Id = 1, PerfilId = 1, PermissaoId = 1 },
                new PerfilPermissao { Id = 2, PerfilId = 1, PermissaoId = 2 },
                new PerfilPermissao { Id = 3, PerfilId = 1, PermissaoId = 3 },
                new PerfilPermissao { Id = 4, PerfilId = 1, PermissaoId = 4 },
                new PerfilPermissao { Id = 5, PerfilId = 1, PermissaoId = 5 },
                new PerfilPermissao { Id = 6, PerfilId = 1, PermissaoId = 6 },
                new PerfilPermissao { Id = 7, PerfilId = 1, PermissaoId = 7 },
                new PerfilPermissao { Id = 8, PerfilId = 1, PermissaoId = 8 },
                new PerfilPermissao { Id = 9, PerfilId = 1, PermissaoId = 9 },
                new PerfilPermissao { Id = 10, PerfilId = 1, PermissaoId = 10 },
                new PerfilPermissao { Id = 11, PerfilId = 1, PermissaoId = 11 },
                new PerfilPermissao { Id = 12, PerfilId = 1, PermissaoId = 12 },
                new PerfilPermissao { Id = 13, PerfilId = 1, PermissaoId = 13 },
                new PerfilPermissao { Id = 14, PerfilId = 1, PermissaoId = 14 },
                new PerfilPermissao { Id = 15, PerfilId = 1, PermissaoId = 15 },
                new PerfilPermissao { Id = 16, PerfilId = 1, PermissaoId = 16 },
                new PerfilPermissao { Id = 17, PerfilId = 1, PermissaoId = 17 },
                new PerfilPermissao { Id = 18, PerfilId = 1, PermissaoId = 18 },
                new PerfilPermissao { Id = 19, PerfilId = 1, PermissaoId = 19 },
                new PerfilPermissao { Id = 20, PerfilId = 1, PermissaoId = 20 },
                new PerfilPermissao { Id = 21, PerfilId = 1, PermissaoId = 21 },
                new PerfilPermissao { Id = 22, PerfilId = 1, PermissaoId = 22 },
                new PerfilPermissao { Id = 23, PerfilId = 1, PermissaoId = 23 },
                new PerfilPermissao { Id = 24, PerfilId = 1, PermissaoId = 24 },
                new PerfilPermissao { Id = 25, PerfilId = 1, PermissaoId = 25 },
                new PerfilPermissao { Id = 26, PerfilId = 1, PermissaoId = 26 },
                new PerfilPermissao { Id = 27, PerfilId = 1, PermissaoId = 27 },
                new PerfilPermissao { Id = 28, PerfilId = 1, PermissaoId = 28 },

                new PerfilPermissao { Id = 29, PerfilId = 2, PermissaoId = 1 },
                new PerfilPermissao { Id = 30, PerfilId = 2, PermissaoId = 2 },
                new PerfilPermissao { Id = 31, PerfilId = 2, PermissaoId = 3 },
                new PerfilPermissao { Id = 32, PerfilId = 2, PermissaoId = 4 },
                new PerfilPermissao { Id = 33, PerfilId = 2, PermissaoId = 5 },
                new PerfilPermissao { Id = 34, PerfilId = 2, PermissaoId = 6 },
                new PerfilPermissao { Id = 35, PerfilId = 2, PermissaoId = 7 },
                new PerfilPermissao { Id = 36, PerfilId = 2, PermissaoId = 8 },
                new PerfilPermissao { Id = 37, PerfilId = 2, PermissaoId = 9 },
                new PerfilPermissao { Id = 38, PerfilId = 2, PermissaoId = 10 },
                new PerfilPermissao { Id = 39, PerfilId = 2, PermissaoId = 11 },
                new PerfilPermissao { Id = 40, PerfilId = 2, PermissaoId = 12 },
                new PerfilPermissao { Id = 41, PerfilId = 2, PermissaoId = 13 },
                new PerfilPermissao { Id = 42, PerfilId = 2, PermissaoId = 14 },
                new PerfilPermissao { Id = 43, PerfilId = 2, PermissaoId = 15 },
                new PerfilPermissao { Id = 44, PerfilId = 2, PermissaoId = 16 },
                new PerfilPermissao { Id = 45, PerfilId = 2, PermissaoId = 17 },
                new PerfilPermissao { Id = 46, PerfilId = 2, PermissaoId = 18 },
                new PerfilPermissao { Id = 47, PerfilId = 2, PermissaoId = 19 },
                new PerfilPermissao { Id = 48, PerfilId = 2, PermissaoId = 20 },
                new PerfilPermissao { Id = 49, PerfilId = 2, PermissaoId = 21 },
                new PerfilPermissao { Id = 50, PerfilId = 2, PermissaoId = 22 },
                new PerfilPermissao { Id = 51, PerfilId = 2, PermissaoId = 23 },
                new PerfilPermissao { Id = 52, PerfilId = 2, PermissaoId = 24 },
                new PerfilPermissao { Id = 53, PerfilId = 2, PermissaoId = 25 },
                new PerfilPermissao { Id = 54, PerfilId = 2, PermissaoId = 26 },
                new PerfilPermissao { Id = 55, PerfilId = 2, PermissaoId = 27 },
                new PerfilPermissao { Id = 56, PerfilId = 2, PermissaoId = 28 }


            );

            // Seed tipos de notificação
            modelBuilder.Entity<TipoNotificacao>().HasData(
                new TipoNotificacao { Id = 1, Nome = "Novo chamado" },
                new TipoNotificacao { Id = 2, Nome = "Novo serviço" }
            );

            // Seed usuário inicial e vinculação a todas as views
            modelBuilder.Entity<Usuarios>().HasData(
                new Usuarios { Id = 1, Nome = "Administrador", Email = "t@t", Senha = "123", PerfilId = 1 },
                new Usuarios { Id = 2, Nome = "Cargo Lider", Email = "f@f", Senha = "123", PerfilId = 2}
            );

            modelBuilder.Entity<UsuarioView>().HasData(
                new UsuarioView { Id = 1, ViewId = 1, UsuarioId = 1 },
                new UsuarioView { Id = 2, ViewId = 2, UsuarioId = 1 },
                new UsuarioView { Id = 3, ViewId = 3, UsuarioId = 1 },
                new UsuarioView { Id = 4, ViewId = 4, UsuarioId = 1 },
                new UsuarioView { Id = 5, ViewId = 5, UsuarioId = 1 },
                new UsuarioView { Id = 6, ViewId = 6, UsuarioId = 1 },
                new UsuarioView { Id = 7, ViewId = 7, UsuarioId = 1 },
                new UsuarioView { Id = 8, ViewId = 8, UsuarioId = 1 },
                new UsuarioView { Id = 9, ViewId = 9, UsuarioId = 1 },
                new UsuarioView { Id = 10, ViewId = 10, UsuarioId = 1 },
                new UsuarioView { Id = 11, ViewId = 11, UsuarioId = 1 },
                new UsuarioView { Id = 12, ViewId = 12, UsuarioId = 1 },
                new UsuarioView { Id = 13, ViewId = 13, UsuarioId = 1 },
                new UsuarioView { Id = 14, ViewId = 14, UsuarioId = 1 },

                new UsuarioView { Id = 15, ViewId = 1, UsuarioId = 2 },
                new UsuarioView { Id = 16, ViewId = 2, UsuarioId = 2 },
                new UsuarioView { Id = 17, ViewId = 3, UsuarioId = 2 },
                new UsuarioView { Id = 18, ViewId = 4, UsuarioId = 2 },
                new UsuarioView { Id = 19, ViewId = 5, UsuarioId = 2 },
                new UsuarioView { Id = 20, ViewId = 6, UsuarioId = 2 },
                new UsuarioView { Id = 21, ViewId = 7, UsuarioId = 2 },
                new UsuarioView { Id = 22, ViewId = 8, UsuarioId = 2 },
                new UsuarioView { Id = 23, ViewId = 9, UsuarioId = 2 },
                new UsuarioView { Id = 24, ViewId = 10, UsuarioId = 2 },
                new UsuarioView { Id = 25, ViewId = 11, UsuarioId = 2 },
                new UsuarioView { Id = 26, ViewId = 12, UsuarioId = 2 },
                new UsuarioView { Id = 27, ViewId = 13, UsuarioId = 2 },
                new UsuarioView { Id = 28, ViewId = 14, UsuarioId = 2 }
            );

        }
    }

}