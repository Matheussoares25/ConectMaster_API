using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConectMaster.Migrations
{
    /// <inheritdoc />
    public partial class RailwayFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 13,
                column: "Name",
                value: "visual.estoqueTi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Views",
                keyColumn: "Id",
                keyValue: 13,
                column: "Name",
                value: "visual. estoqueTi");
        }
    }
}
