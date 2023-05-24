using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Devpendent.Migrations
{
    /// <inheritdoc />
    public partial class AdditionalUserInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactText",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Specialties",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactText",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Specialties",
                table: "AspNetUsers");
        }
    }
}
