using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConectMaster.Migrations
{
    /// <inheritdoc />
    public partial class comdatafechamento2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataFechamento",
                table: "Chamados",
                newName: "DataAlteracao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataAlteracao",
                table: "Chamados",
                newName: "DataFechamento");
        }
    }
}
