using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineBanking.Migrations
{
    /// <inheritdoc />
    public partial class nullKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_bankAccounts_bankAccountID",
                table: "users");

            migrationBuilder.AlterColumn<int>(
                name: "bankAccountID",
                table: "users",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_users_bankAccounts_bankAccountID",
                table: "users",
                column: "bankAccountID",
                principalTable: "bankAccounts",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_bankAccounts_bankAccountID",
                table: "users");

            migrationBuilder.AlterColumn<int>(
                name: "bankAccountID",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_users_bankAccounts_bankAccountID",
                table: "users",
                column: "bankAccountID",
                principalTable: "bankAccounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
