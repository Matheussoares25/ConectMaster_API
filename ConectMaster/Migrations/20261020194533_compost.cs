using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ConectMaster.Migrations
{
    /// <inheritdoc />
    public partial class compost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Estoques",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estoques", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Formularios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CamposJson = table.Column<string>(type: "text", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formularios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogsAuditoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Acao = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Entidade = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EntidadeId = table.Column<int>(type: "integer", nullable: true),
                    Detalhes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    DataHora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogsAuditoria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Perfis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perfis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoNotificacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoNotificacoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Views",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Views", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstoqueItens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Quantidade = table.Column<decimal>(type: "numeric", nullable: false),
                    EstoqueId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstoqueItens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstoqueItens_Estoques_EstoqueId",
                        column: x => x.EstoqueId,
                        principalTable: "Estoques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Senha = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Ramal = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Telefone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Setor = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PerfilId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Perfis_PerfilId",
                        column: x => x.PerfilId,
                        principalTable: "Perfis",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PerfilPermissoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PerfilId = table.Column<int>(type: "integer", nullable: false),
                    PermissaoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfilPermissoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerfilPermissoes_Perfis_PerfilId",
                        column: x => x.PerfilId,
                        principalTable: "Perfis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PerfilPermissoes_Permissoes_PermissaoId",
                        column: x => x.PermissaoId,
                        principalTable: "Permissoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chamados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    idUsuario = table.Column<int>(type: "integer", nullable: false),
                    Titulo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    DataAbertura = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataFechamento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Prioridade = table.Column<int>(type: "integer", nullable: false),
                    Categoria = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Setor = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chamados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chamados_Usuarios_idUsuario",
                        column: x => x.idUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Historicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    TipoServico = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    DataAbertura = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataSolucao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Historicos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notificacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    TipoNotificacaoId = table.Column<int>(type: "integer", nullable: false),
                    Mensagem = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Lida = table.Column<bool>(type: "boolean", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notificacoes_TipoNotificacoes_TipoNotificacaoId",
                        column: x => x.TipoNotificacaoId,
                        principalTable: "TipoNotificacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notificacoes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissoesEstoques",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    EstoqueId = table.Column<int>(type: "integer", nullable: false),
                    PodeVisualizar = table.Column<bool>(type: "boolean", nullable: false),
                    PodeCriar = table.Column<bool>(type: "boolean", nullable: false),
                    PodeEditar = table.Column<bool>(type: "boolean", nullable: false),
                    PodeApagar = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissoesEstoques", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissoesEstoques_Estoques_EstoqueId",
                        column: x => x.EstoqueId,
                        principalTable: "Estoques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissoesEstoques_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Servicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titulo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Setor = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Categoria = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Prioridade = table.Column<int>(type: "integer", nullable: false),
                    ClienteNome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ClienteDocumento = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ClienteTelefone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ClienteContato = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DescricaoCarga = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    PesoBruto = table.Column<decimal>(type: "numeric", nullable: true),
                    Volume = table.Column<decimal>(type: "numeric", nullable: true),
                    QtdVolumes = table.Column<int>(type: "integer", nullable: true),
                    ValorMercadoria = table.Column<decimal>(type: "numeric", nullable: true),
                    NaturezaCarga = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EnderecoColeta = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    EnderecoEntrega = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    DataColeta = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataEntrega = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PlacaVeiculo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    TipoVeiculo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    MotoristaNome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    MotoristaTelefone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    NumeroNfe = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    NumeroCte = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ValorFrete = table.Column<decimal>(type: "numeric", nullable: true),
                    FormaPagamento = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Descricao = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    SolicitanteId = table.Column<int>(type: "integer", nullable: false),
                    DataAbertura = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Servicos_Usuarios_SolicitanteId",
                        column: x => x.SolicitanteId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioView",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ViewId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioView", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioView_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioView_Views_ViewId",
                        column: x => x.ViewId,
                        principalTable: "Views",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Estoques",
                columns: new[] { "Id", "Ativo", "Nome" },
                values: new object[,]
                {
                    { 1, true, "TI" },
                    { 2, true, "Compras" },
                    { 3, true, "Expedicao" }
                });

            migrationBuilder.InsertData(
                table: "Perfis",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Administrador" },
                    { 2, "Lider" }
                });

            migrationBuilder.InsertData(
                table: "Permissoes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Visualizar chamados" },
                    { 2, "Criar chamados" },
                    { 3, "Editar chamados" },
                    { 4, "Apagar chamados" },
                    { 5, "Visualizar historicos" },
                    { 6, "Editar historicos" },
                    { 7, "Apagar historicos" },
                    { 8, "Visualizar permissoes" },
                    { 9, "Criar permissoes" },
                    { 10, "Editar permissoes" },
                    { 11, "Apagar permissoes" },
                    { 12, "Visualizar perfis" },
                    { 13, "Criar perfis" },
                    { 14, "Editar perfis" },
                    { 15, "Apagar perfis" },
                    { 16, "Visualizar usuarios" },
                    { 17, "Criar Usuario" },
                    { 18, "Editar usuarios" },
                    { 19, "Apagar usuarios" },
                    { 20, "Visualizar perfilviews" },
                    { 21, "Criar usuarioviews" },
                    { 22, "Apagar perfilviews" },
                    { 23, "Apagar usuarioviws" },
                    { 24, "Visualizar views" },
                    { 25, "Criar servico" },
                    { 26, "Visualizar servicos" },
                    { 27, "Autorizar ou Negar OS" },
                    { 28, "Visualizar estoques" },
                    { 29, "Visualizar formularios" },
                    { 30, "Criar formularios" },
                    { 31, "Editar formularios" },
                    { 32, "Apagar formularios" }
                });

            migrationBuilder.InsertData(
                table: "TipoNotificacoes",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Novo chamado" },
                    { 2, "Novo serviço" }
                });

            migrationBuilder.InsertData(
                table: "Views",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "visual.abrirchamado" },
                    { 2, "visual.ordemservico" },
                    { 3, "visual.historicochamados" },
                    { 4, "visual.painelcontrole" },
                    { 5, "visual.abriruber" },
                    { 6, "visual.geralusuarios" },
                    { 7, "visual.permissoes" },
                    { 8, "visual.relatorios" },
                    { 9, "visual.logs" },
                    { 10, "visual.configuracoes" },
                    { 11, "visual.servicosgeral" },
                    { 12, "visual.portaria" },
                    { 13, "visual.formularios" },
                    { 14, "visual.estoques" }
                });

            migrationBuilder.InsertData(
                table: "PerfilPermissoes",
                columns: new[] { "Id", "PerfilId", "PermissaoId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 1, 2 },
                    { 3, 1, 3 },
                    { 4, 1, 4 },
                    { 5, 1, 5 },
                    { 6, 1, 6 },
                    { 7, 1, 7 },
                    { 8, 1, 8 },
                    { 9, 1, 9 },
                    { 10, 1, 10 },
                    { 11, 1, 11 },
                    { 12, 1, 12 },
                    { 13, 1, 13 },
                    { 14, 1, 14 },
                    { 15, 1, 15 },
                    { 16, 1, 16 },
                    { 17, 1, 17 },
                    { 18, 1, 18 },
                    { 19, 1, 19 },
                    { 20, 1, 20 },
                    { 21, 1, 21 },
                    { 22, 1, 22 },
                    { 23, 1, 23 },
                    { 24, 1, 24 },
                    { 25, 1, 25 },
                    { 26, 1, 26 },
                    { 27, 1, 27 },
                    { 28, 1, 28 },
                    { 29, 2, 1 },
                    { 30, 2, 2 },
                    { 31, 2, 3 },
                    { 32, 2, 4 },
                    { 33, 2, 5 },
                    { 34, 2, 6 },
                    { 35, 2, 7 },
                    { 36, 2, 8 },
                    { 37, 2, 9 },
                    { 38, 2, 10 },
                    { 39, 2, 11 },
                    { 40, 2, 12 },
                    { 41, 2, 13 },
                    { 42, 2, 14 },
                    { 43, 2, 15 },
                    { 44, 2, 16 },
                    { 45, 2, 17 },
                    { 46, 2, 18 },
                    { 47, 2, 19 },
                    { 48, 2, 20 },
                    { 49, 2, 21 },
                    { 50, 2, 22 },
                    { 51, 2, 23 },
                    { 52, 2, 24 },
                    { 53, 2, 25 },
                    { 54, 2, 26 },
                    { 55, 2, 27 },
                    { 56, 2, 28 },
                    { 57, 1, 29 },
                    { 58, 1, 30 },
                    { 59, 1, 31 },
                    { 60, 1, 32 },
                    { 61, 2, 29 },
                    { 62, 2, 30 },
                    { 63, 2, 31 },
                    { 64, 2, 32 }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Email", "Nome", "PerfilId", "Ramal", "Senha", "Setor", "Telefone" },
                values: new object[,]
                {
                    { 1, "t@t", "Administrador", 1, null, "123", null, null },
                    { 2, "f@f", "Cargo Lider", 2, null, "123", null, null }
                });

            migrationBuilder.InsertData(
                table: "PermissoesEstoques",
                columns: new[] { "Id", "EstoqueId", "PodeApagar", "PodeCriar", "PodeEditar", "PodeVisualizar", "UsuarioId" },
                values: new object[,]
                {
                    { 1, 1, true, true, true, true, 1 },
                    { 2, 2, true, true, true, true, 1 },
                    { 3, 3, true, true, true, true, 1 },
                    { 4, 1, true, true, true, true, 2 },
                    { 5, 2, true, true, true, true, 2 },
                    { 6, 3, true, true, true, true, 2 }
                });

            migrationBuilder.InsertData(
                table: "UsuarioView",
                columns: new[] { "Id", "UsuarioId", "ViewId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 1, 2 },
                    { 3, 1, 3 },
                    { 4, 1, 4 },
                    { 5, 1, 5 },
                    { 6, 1, 6 },
                    { 7, 1, 7 },
                    { 8, 1, 8 },
                    { 9, 1, 9 },
                    { 10, 1, 10 },
                    { 11, 1, 11 },
                    { 12, 1, 12 },
                    { 13, 1, 13 },
                    { 14, 1, 14 },
                    { 15, 2, 1 },
                    { 16, 2, 2 },
                    { 17, 2, 3 },
                    { 18, 2, 4 },
                    { 19, 2, 5 },
                    { 20, 2, 6 },
                    { 21, 2, 7 },
                    { 22, 2, 8 },
                    { 23, 2, 9 },
                    { 24, 2, 10 },
                    { 25, 2, 11 },
                    { 26, 2, 12 },
                    { 27, 2, 13 },
                    { 28, 2, 14 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chamados_idUsuario",
                table: "Chamados",
                column: "idUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_EstoqueItens_EstoqueId",
                table: "EstoqueItens",
                column: "EstoqueId");

            migrationBuilder.CreateIndex(
                name: "IX_Historicos_UsuarioId",
                table: "Historicos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_TipoNotificacaoId",
                table: "Notificacoes",
                column: "TipoNotificacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_UsuarioId",
                table: "Notificacoes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_PerfilPermissoes_PerfilId",
                table: "PerfilPermissoes",
                column: "PerfilId");

            migrationBuilder.CreateIndex(
                name: "IX_PerfilPermissoes_PermissaoId",
                table: "PerfilPermissoes",
                column: "PermissaoId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissoesEstoques_EstoqueId",
                table: "PermissoesEstoques",
                column: "EstoqueId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissoesEstoques_UsuarioId_EstoqueId",
                table: "PermissoesEstoques",
                columns: new[] { "UsuarioId", "EstoqueId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Servicos_SolicitanteId",
                table: "Servicos",
                column: "SolicitanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_PerfilId",
                table: "Usuarios",
                column: "PerfilId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioView_UsuarioId",
                table: "UsuarioView",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioView_ViewId",
                table: "UsuarioView",
                column: "ViewId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chamados");

            migrationBuilder.DropTable(
                name: "EstoqueItens");

            migrationBuilder.DropTable(
                name: "Formularios");

            migrationBuilder.DropTable(
                name: "Historicos");

            migrationBuilder.DropTable(
                name: "LogsAuditoria");

            migrationBuilder.DropTable(
                name: "Notificacoes");

            migrationBuilder.DropTable(
                name: "PerfilPermissoes");

            migrationBuilder.DropTable(
                name: "PermissoesEstoques");

            migrationBuilder.DropTable(
                name: "Servicos");

            migrationBuilder.DropTable(
                name: "UsuarioView");

            migrationBuilder.DropTable(
                name: "TipoNotificacoes");

            migrationBuilder.DropTable(
                name: "Permissoes");

            migrationBuilder.DropTable(
                name: "Estoques");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Views");

            migrationBuilder.DropTable(
                name: "Perfis");
        }
    }
}
