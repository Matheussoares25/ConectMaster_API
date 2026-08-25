using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ConectMaster.Migrations
{
    /// <inheritdoc />
    public partial class comestoques : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataFechamento",
                table: "Chamados",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Perfis",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Lider" });

            migrationBuilder.InsertData(
                table: "Permissoes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 28, "Visualizar estoques" });

            migrationBuilder.InsertData(
                table: "Views",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 12, "visual.portaria" },
                    { 13, "visual. estoqueTi" }
                });

            migrationBuilder.InsertData(
                table: "PerfilPermissoes",
                columns: new[] { "Id", "PerfilId", "PermissaoId" },
                values: new object[,]
                {
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
                    { 56, 2, 28 }
                });

            migrationBuilder.InsertData(
                table: "UsuarioView",
                columns: new[] { "Id", "UsuarioId", "ViewId" },
                values: new object[,]
                {
                    { 12, 1, 12 },
                    { 13, 1, 13 }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Email", "Nome", "PerfilId", "Ramal", "Senha", "Setor", "Telefone" },
                values: new object[] { 2, "f@f", "Cargo Lider", 2, null, "123", null, null });

            migrationBuilder.InsertData(
                table: "UsuarioView",
                columns: new[] { "Id", "UsuarioId", "ViewId" },
                values: new object[,]
                {
                    { 14, 2, 1 },
                    { 15, 2, 2 },
                    { 16, 2, 3 },
                    { 17, 2, 4 },
                    { 18, 2, 5 },
                    { 19, 2, 6 },
                    { 20, 2, 7 },
                    { 21, 2, 8 },
                    { 22, 2, 9 },
                    { 23, 2, 10 },
                    { 24, 2, 11 },
                    { 25, 2, 12 },
                    { 26, 2, 13 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PerfilPermissoes",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "PerfilPermissoes",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "PerfilPermissoes",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "PerfilPermissoes",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "PerfilPermissoes",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "PerfilPermissoes",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "PerfilPermissoes",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "PerfilPermissoes",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "PerfilPermissoes",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "PerfilPermissoes",
                keyColumn: "Id",
                keyValue: 37);

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
                table: "PerfilPermissoes",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "PerfilPermissoes",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "PerfilPermissoes",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "PerfilPermissoes",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "PerfilPermissoes",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "PerfilPermissoes",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "PerfilPermissoes",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "PerfilPermissoes",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "PerfilPermissoes",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "PerfilPermissoes",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "PerfilPermissoes",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "UsuarioView",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "UsuarioView",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "UsuarioView",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "UsuarioView",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "UsuarioView",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "UsuarioView",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "UsuarioView",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "UsuarioView",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "UsuarioView",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "UsuarioView",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "UsuarioView",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "UsuarioView",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "UsuarioView",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "UsuarioView",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "UsuarioView",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Perfis",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "DataFechamento",
                table: "Chamados");
        }
    }
}
