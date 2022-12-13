using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineBanking.Migrations
{
    /// <inheritdoc />
    public partial class activation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "verificationCode",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "users");

            migrationBuilder.DropColumn(
                name: "verificationCode",
                table: "users");
        }
    }
}
