using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ConectMaster.Migrations
{
    /// <inheritdoc />
    public partial class usuarinamer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status",
                table: "Chamados",
                newName: "Status");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioName",
                table: "LogsAuditoria",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Permissoes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 25, "Criar servico" },
                    { 26, "Visualizar servicos" }
                });

            migrationBuilder.InsertData(
                table: "Views",
                columns: new[] { "Id", "Name" },
                values: new object[] { 11, "visual.servicosgeral" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DropColumn(
                name: "UsuarioName",
                table: "LogsAuditoria");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Chamados",
                newName: "status");
        }
    }
}
