using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineBanking.Migrations
{
    /// <inheritdoc />
    public partial class bankAccounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_BankAccount_BankAccountID",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_users_BankAccount_bankAccountID",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BankAccount",
                table: "BankAccount");

            migrationBuilder.RenameTable(
                name: "BankAccount",
                newName: "bankAccounts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bankAccounts",
                table: "bankAccounts",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_bankAccounts_BankAccountID",
                table: "Transaction",
                column: "BankAccountID",
                principalTable: "bankAccounts",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_users_bankAccounts_bankAccountID",
                table: "users",
                column: "bankAccountID",
                principalTable: "bankAccounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_bankAccounts_BankAccountID",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_users_bankAccounts_bankAccountID",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bankAccounts",
                table: "bankAccounts");

            migrationBuilder.RenameTable(
                name: "bankAccounts",
                newName: "BankAccount");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BankAccount",
                table: "BankAccount",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_BankAccount_BankAccountID",
                table: "Transaction",
                column: "BankAccountID",
                principalTable: "BankAccount",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_users_BankAccount_bankAccountID",
                table: "users",
                column: "bankAccountID",
                principalTable: "BankAccount",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
