using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineBanking.Migrations
{
    /// <inheritdoc />
    public partial class transactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_bankAccounts_BankAccountID",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_BankAccountID",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "BankAccountID",
                table: "Transaction");

            migrationBuilder.AddColumn<int>(
                name: "fromID",
                table: "Transaction",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_fromID",
                table: "Transaction",
                column: "fromID");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_bankAccounts_fromID",
                table: "Transaction",
                column: "fromID",
                principalTable: "bankAccounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_bankAccounts_fromID",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_fromID",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "fromID",
                table: "Transaction");

            migrationBuilder.AddColumn<int>(
                name: "BankAccountID",
                table: "Transaction",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_BankAccountID",
                table: "Transaction",
                column: "BankAccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_bankAccounts_BankAccountID",
                table: "Transaction",
                column: "BankAccountID",
                principalTable: "bankAccounts",
                principalColumn: "ID");
        }
    }
}
