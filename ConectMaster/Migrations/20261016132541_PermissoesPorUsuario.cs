using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConectMaster.Migrations
{
    /// <inheritdoc />
    public partial class PermissoesPorUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissoesEstoques_Perfis_PerfilId",
                table: "PermissoesEstoques");

            migrationBuilder.DropIndex(
                name: "IX_PermissoesEstoques_PerfilId",
                table: "PermissoesEstoques");

            migrationBuilder.RenameColumn(
                name: "PerfilId",
                table: "PermissoesEstoques",
                newName: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissoesEstoques_UsuarioId_EstoqueId",
                table: "PermissoesEstoques",
                columns: new[] { "UsuarioId", "EstoqueId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PermissoesEstoques_Usuarios_UsuarioId",
                table: "PermissoesEstoques",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissoesEstoques_Usuarios_UsuarioId",
                table: "PermissoesEstoques");

            migrationBuilder.DropIndex(
                name: "IX_PermissoesEstoques_UsuarioId_EstoqueId",
                table: "PermissoesEstoques");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "PermissoesEstoques",
                newName: "PerfilId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissoesEstoques_PerfilId",
                table: "PermissoesEstoques",
                column: "PerfilId");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissoesEstoques_Perfis_PerfilId",
                table: "PermissoesEstoques",
                column: "PerfilId",
                principalTable: "Perfis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
