using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ConectMaster.Migrations
{
    /// <inheritdoc />
    public partial class comemails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DpMoveis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Departamento = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Aparelho = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Responsavel = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DataDoTermo = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DpMoveis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Emails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Responsavel = table.Column<string>(type: "text", nullable: false),
                    Departamentoid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Emails_Departamentos_Departamentoid",
                        column: x => x.Departamentoid,
                        principalTable: "Departamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permissoes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 38, "Visualizar emails" },
                    { 39, "Criar emails" },
                    { 40, "Editar emails" },
                    { 41, "Apagar emails" },
                    { 42, "Visualizar DpMoveis" },
                    { 43, "Criar DpMoveis" },
                    { 44, "Editar DpMoveis" },
                    { 45, "Apagar DpMoveis" }
                });

            migrationBuilder.InsertData(
                table: "UsuarioView",
                columns: new[] { "Id", "UsuarioId", "ViewId" },
                values: new object[] { 30, 1, 30 });

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "visual.emailsti");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "visual.ordemservico");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "visual.painelcontrole");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "visual.abriruber");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "visual.geralusuarios");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "visual.permissoes");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 9,
                column: "Name",
                value: "visual.relatorios");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 10,
                column: "Name",
                value: "visual.logs");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 11,
                column: "Name",
                value: "visual.configuracoes");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 12,
                column: "Name",
                value: "visual.servicosgeral");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 13,
                column: "Name",
                value: "visual.portaria");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 14,
                column: "Name",
                value: "visual.estoques");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 15,
                column: "Name",
                value: "visual.departamentos");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 16,
                column: "Name",
                value: "visual.abrirchamadocompras");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 17,
                column: "Name",
                value: "visual.historicochamadocompras");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 18,
                column: "Name",
                value: "visual.historicostandby");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 19,
                column: "Name",
                value: "visual.adm");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 20,
                column: "Name",
                value: "visual.compras");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 21,
                column: "Name",
                value: "visual.ti");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 22,
                column: "Name",
                value: "visual.sgdt");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 23,
                column: "Name",
                value: "visual.representantes");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 24,
                column: "Name",
                value: "visual.comercial");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 25,
                column: "Name",
                value: "visual.im");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 26,
                column: "Name",
                value: "visual.limpeza");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 27,
                column: "Name",
                value: "visual.qualidade");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 28,
                column: "Name",
                value: "visual.pcp");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 29,
                column: "Name",
                value: "visual.fatum");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 30,
                column: "Name",
                value: "visual.expedicao");

            migrationBuilder.InsertData(
                table: "Views",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 31, "visual.estoque" },
                    { 32, "visual.portaria" },
                    { 33, "visual.dpmoveis" }
                });

            migrationBuilder.InsertData(
                table: "PerfilPermissoes",
                columns: new[] { "Id", "PerfilId", "PermissaoId" },
                values: new object[,]
                {
                    { 38, 1, 38 },
                    { 39, 1, 39 },
                    { 40, 1, 40 },
                    { 41, 1, 41 },
                    { 42, 1, 42 },
                    { 43, 1, 43 },
                    { 44, 1, 44 },
                    { 45, 1, 45 }
                });

            migrationBuilder.InsertData(
                table: "UsuarioView",
                columns: new[] { "Id", "UsuarioId", "ViewId" },
                values: new object[,]
                {
                    { 31, 1, 31 },
                    { 32, 1, 32 },
                    { 33, 1, 33 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Emails_Departamentoid",
                table: "Emails",
                column: "Departamentoid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DpMoveis");

            migrationBuilder.DropTable(
                name: "Emails");

            migrationBuilder.DeleteData(
                table: "PerfilPermissoes",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "PerfilPermissoes",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "PerfilPermissoes",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "PerfilPermissoes",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "PerfilPermissoes",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "PerfilPermissoes",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "PerfilPermissoes",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "PerfilPermissoes",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "UsuarioView",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "UsuarioView",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "UsuarioView",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "UsuarioView",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "visual.ordemservico");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "visual.painelcontrole");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "visual.abriruber");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "visual.geralusuarios");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "visual.permissoes");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "visual.relatorios");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 9,
                column: "Name",
                value: "visual.logs");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 10,
                column: "Name",
                value: "visual.configuracoes");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 11,
                column: "Name",
                value: "visual.servicosgeral");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 12,
                column: "Name",
                value: "visual.portaria");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 13,
                column: "Name",
                value: "visual.estoques");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 14,
                column: "Name",
                value: "visual.departamentos");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 15,
                column: "Name",
                value: "visual.abrirchamadocompras");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 16,
                column: "Name",
                value: "visual.historicochamadocompras");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 17,
                column: "Name",
                value: "visual.adm");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 18,
                column: "Name",
                value: "visual.compras");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 19,
                column: "Name",
                value: "visual.ti");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 20,
                column: "Name",
                value: "visual.sgdt");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 21,
                column: "Name",
                value: "visual.representantes");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 22,
                column: "Name",
                value: "visual.comercial");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 23,
                column: "Name",
                value: "visual.im");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 24,
                column: "Name",
                value: "visual.limpeza");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 25,
                column: "Name",
                value: "visual.qualidade");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 26,
                column: "Name",
                value: "visual.pcp");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 27,
                column: "Name",
                value: "visual.fatum");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 28,
                column: "Name",
                value: "visual.expedicao");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 29,
                column: "Name",
                value: "visual.estoque");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 30,
                column: "Name",
                value: "visual.portaria");
        }
    }
}
