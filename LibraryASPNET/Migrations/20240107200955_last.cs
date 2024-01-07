using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryASPNET.Migrations
{
    /// <inheritdoc />
    public partial class last : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "books");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "books",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "books");

            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "books",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
