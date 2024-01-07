using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryASPNET.Migrations
{
    /// <inheritdoc />
    public partial class AddResetTokenColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetToken",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetTokenExpiration",
                table: "users",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetToken",
                table: "users");

            migrationBuilder.DropColumn(
                name: "ResetTokenExpiration",
                table: "users");
        }
    }
}
