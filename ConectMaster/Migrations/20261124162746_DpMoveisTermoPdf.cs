using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConectMaster.Migrations
{
    /// <inheritdoc />
    public partial class DpMoveisTermoPdf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ConteudoTermoPdf",
                table: "DpMoveis",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomeArquivoTermo",
                table: "DpMoveis",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConteudoTermoPdf",
                table: "DpMoveis");

            migrationBuilder.DropColumn(
                name: "NomeArquivoTermo",
                table: "DpMoveis");
        }
    }
}
