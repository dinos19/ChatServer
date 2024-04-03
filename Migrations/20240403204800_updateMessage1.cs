using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatServer.Migrations
{
    /// <inheritdoc />
    public partial class updateMessage1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromUser",
                table: "ChatMessage");

            migrationBuilder.DropColumn(
                name: "ToUser",
                table: "ChatMessage");

            migrationBuilder.AddColumn<int>(
                name: "FromAccountId",
                table: "ChatMessage",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ToAccountId",
                table: "ChatMessage",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromAccountId",
                table: "ChatMessage");

            migrationBuilder.DropColumn(
                name: "ToAccountId",
                table: "ChatMessage");

            migrationBuilder.AddColumn<string>(
                name: "FromUser",
                table: "ChatMessage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ToUser",
                table: "ChatMessage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
