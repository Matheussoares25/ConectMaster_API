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
                new Views { Id = 11 , Name = "visual.servicosgeral"}
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
                new Permissao { Id = 27, Name = "Autorizar ou Negar OS" }
            );

            // Seed perfil Administrador
            modelBuilder.Entity<Perfil>().HasData(
                new Perfil { Id = 1, Name = "Administrador" }
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
                new PerfilPermissao { Id = 27, PerfilId = 1, PermissaoId = 27 }
            );

        }
    }

}