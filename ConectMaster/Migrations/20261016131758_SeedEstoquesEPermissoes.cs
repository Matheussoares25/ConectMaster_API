using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ConectMaster.Migrations
{
    /// <inheritdoc />
    public partial class SeedEstoquesEPermissoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                table: "PermissoesEstoques",
                columns: new[] { "Id", "EstoqueId", "PerfilId", "PodeApagar", "PodeCriar", "PodeEditar", "PodeVisualizar" },
                values: new object[,]
                {
                    { 1, 1, 1, true, true, true, true },
                    { 2, 2, 1, true, true, true, true },
                    { 3, 3, 1, true, true, true, true },
                    { 4, 1, 2, true, true, true, true },
                    { 5, 2, 2, true, true, true, true },
                    { 6, 3, 2, true, true, true, true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PermissoesEstoques",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PermissoesEstoques",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PermissoesEstoques",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PermissoesEstoques",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PermissoesEstoques",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PermissoesEstoques",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Estoques",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Estoques",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Estoques",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
