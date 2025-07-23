using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WalletApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAppBankAccountIdToTransaction2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_BankAccounts_AppBankAccountId",
                table: "Transactions");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_BankAccounts_AppBankAccountId",
                table: "Transactions",
                column: "AppBankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_BankAccounts_AppBankAccountId",
                table: "Transactions");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_BankAccounts_AppBankAccountId",
                table: "Transactions",
                column: "AppBankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
