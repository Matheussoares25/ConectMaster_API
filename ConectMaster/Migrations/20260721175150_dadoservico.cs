using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ConectMaster.Migrations
{
    /// <inheritdoc />
    public partial class dadoservico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LogsAuditoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Acao = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Entidade = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EntidadeId = table.Column<int>(type: "int", nullable: true),
                    Detalhes = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataHora = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogsAuditoria", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Perfis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perfis", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Permissoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissoes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Servicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Titulo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Setor = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Categoria = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Prioridade = table.Column<int>(type: "int", nullable: false),
                    ClienteNome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClienteDocumento = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClienteTelefone = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClienteContato = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DescricaoCarga = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PesoBruto = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Volume = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    QtdVolumes = table.Column<int>(type: "int", nullable: true),
                    ValorMercadoria = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    NaturezaCarga = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EnderecoColeta = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EnderecoEntrega = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataColeta = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataEntrega = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PlacaVeiculo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoVeiculo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MotoristaNome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MotoristaTelefone = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NumeroNfe = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NumeroCte = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ValorFrete = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    FormaPagamento = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SolicitanteId = table.Column<int>(type: "int", nullable: false),
                    DataAbertura = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Views",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Views", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Senha = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ramal = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Setor = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PerfilId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Perfis_PerfilId",
                        column: x => x.PerfilId,
                        principalTable: "Perfis",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PerfilPermissoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PerfilId = table.Column<int>(type: "int", nullable: false),
                    PermissaoId = table.Column<int>(type: "int", nullable: false)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Chamados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    idUsuario = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataAbertura = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Prioridade = table.Column<int>(type: "int", nullable: false),
                    Categoria = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Setor = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Historicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    TipoServico = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataAbertura = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataSolucao = table.Column<DateTime>(type: "datetime(6)", nullable: true)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UsuarioView",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ViewId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Perfis",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Administrador" });

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
                    { 24, "Visualizar views" }
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
                    { 10, "visual.configuracoes" }
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
                    { 24, 1, 24 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chamados_idUsuario",
                table: "Chamados",
                column: "idUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Historicos_UsuarioId",
                table: "Historicos",
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
                name: "Historicos");

            migrationBuilder.DropTable(
                name: "LogsAuditoria");

            migrationBuilder.DropTable(
                name: "PerfilPermissoes");

            migrationBuilder.DropTable(
                name: "Servicos");

            migrationBuilder.DropTable(
                name: "UsuarioView");

            migrationBuilder.DropTable(
                name: "Permissoes");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Views");

            migrationBuilder.DropTable(
                name: "Perfis");
        }
    }
}
