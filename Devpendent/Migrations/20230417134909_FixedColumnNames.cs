using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Devpendent.Migrations
{
    /// <inheritdoc />
    public partial class FixedColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "startDate",
                table: "Jobs",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "endDate",
                table: "Jobs",
                newName: "EndDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Jobs",
                newName: "startDate");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Jobs",
                newName: "endDate");
        }
    }
}
