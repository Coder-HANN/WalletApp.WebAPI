using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WalletApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAppBankAccountIdToWallet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AppBankAccountId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AppBankAccountId",
                table: "Transactions",
                column: "AppBankAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_BankAccounts_AppBankAccountId",
                table: "Transactions",
                column: "AppBankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_BankAccounts_AppBankAccountId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_AppBankAccountId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "AppBankAccountId",
                table: "Transactions");
        }
    }
}
