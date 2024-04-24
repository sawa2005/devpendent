using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Devpendent.Migrations
{
    /// <inheritdoc />
    public partial class ReviewAuthorAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Reviews");
        }
    }
}
