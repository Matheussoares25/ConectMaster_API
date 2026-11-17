using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConectMaster.Migrations
{
    /// <inheritdoc />
    public partial class comprascomautorização : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Aprovado",
                table: "ChamadosCompras",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataAprovacao",
                table: "ChamadosCompras",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "standby",
                table: "ChamadosCompras",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Permissoes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 34, "Aprovar chamados e cotacao Compras" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DropColumn(
                name: "Aprovado",
                table: "ChamadosCompras");

            migrationBuilder.DropColumn(
                name: "DataAprovacao",
                table: "ChamadosCompras");

            migrationBuilder.DropColumn(
                name: "standby",
                table: "ChamadosCompras");
        }
    }
}
