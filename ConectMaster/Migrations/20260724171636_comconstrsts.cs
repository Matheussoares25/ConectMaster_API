using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConectMaster.Migrations
{
    /// <inheritdoc />
    public partial class comconstrsts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Servicos_Usuarios_idUsuario",
                table: "Servicos");

            migrationBuilder.DropIndex(
                name: "IX_Servicos_idUsuario",
                table: "Servicos");

            migrationBuilder.DropColumn(
                name: "idUsuario",
                table: "Servicos");

            migrationBuilder.CreateIndex(
                name: "IX_Servicos_SolicitanteId",
                table: "Servicos",
                column: "SolicitanteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Servicos_Usuarios_SolicitanteId",
                table: "Servicos",
                column: "SolicitanteId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Servicos_Usuarios_SolicitanteId",
                table: "Servicos");

            migrationBuilder.DropIndex(
                name: "IX_Servicos_SolicitanteId",
                table: "Servicos");

            migrationBuilder.AddColumn<int>(
                name: "idUsuario",
                table: "Servicos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Servicos_idUsuario",
                table: "Servicos",
                column: "idUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Servicos_Usuarios_idUsuario",
                table: "Servicos",
                column: "idUsuario",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }
    }
}
