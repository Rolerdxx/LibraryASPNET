using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryASPNET.Migrations
{
    /// <inheritdoc />
    public partial class lastt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "books",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "books");
        }
    }
}
