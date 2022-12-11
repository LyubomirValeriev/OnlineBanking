using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineBanking.Migrations
{
    /// <inheritdoc />
    public partial class transaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Transaction",
                newName: "Memo");

            migrationBuilder.AddColumn<double>(
                name: "amount",
                table: "Transaction",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<double>(
                name: "Balance",
                table: "bankAccounts",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "amount",
                table: "Transaction");

            migrationBuilder.RenameColumn(
                name: "Memo",
                table: "Transaction",
                newName: "Description");

            migrationBuilder.AlterColumn<int>(
                name: "Balance",
                table: "bankAccounts",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }
    }
}
