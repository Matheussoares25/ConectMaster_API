using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConectMaster.Migrations
{
    /// <inheritdoc />
    public partial class comdatafechamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UsuarioName",
                table: "LogsAuditoria",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataFechamento",
                table: "Chamados",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataFechamento",
                table: "Chamados");

            migrationBuilder.UpdateData(
                table: "LogsAuditoria",
                keyColumn: "UsuarioName",
                keyValue: null,
                column: "UsuarioName",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioName",
                table: "LogsAuditoria",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
