using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConectMaster.Migrations
{
    /// <inheritdoc />
    public partial class EstoquePermissoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Estoques",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estoques", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PermissoesEstoques",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PerfilId = table.Column<int>(type: "int", nullable: false),
                    EstoqueId = table.Column<int>(type: "int", nullable: false),
                    PodeVisualizar = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PodeCriar = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PodeEditar = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PodeApagar = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissoesEstoques", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissoesEstoques_Estoques_EstoqueId",
                        column: x => x.EstoqueId,
                        principalTable: "Estoques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissoesEstoques_Perfis_PerfilId",
                        column: x => x.PerfilId,
                        principalTable: "Perfis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 14,
                column: "Name",
                value: "visual.estoques");

            migrationBuilder.CreateIndex(
                name: "IX_PermissoesEstoques_EstoqueId",
                table: "PermissoesEstoques",
                column: "EstoqueId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissoesEstoques_PerfilId",
                table: "PermissoesEstoques",
                column: "PerfilId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissoesEstoques");

            migrationBuilder.DropTable(
                name: "Estoques");

            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 14,
                column: "Name",
                value: "visual.estoqueTi");
        }
    }
}
