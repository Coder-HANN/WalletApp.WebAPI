using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WalletApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTargetBankColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactions_ProviderBanks_TargetBankId",
                table: "BankTransactions");

            migrationBuilder.RenameColumn(
                name: "TargetBankId",
                table: "BankTransactions",
                newName: "TargetProviderBankId");

            migrationBuilder.RenameIndex(
                name: "IX_BankTransactions_TargetBankId",
                table: "BankTransactions",
                newName: "IX_BankTransactions_TargetProviderBankId");

            migrationBuilder.AddColumn<Guid>(
                name: "TargetAppBankAccountId",
                table: "BankTransactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactions_TargetAppBankAccountId",
                table: "BankTransactions",
                column: "TargetAppBankAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactions_BankAccounts_TargetAppBankAccountId",
                table: "BankTransactions",
                column: "TargetAppBankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactions_ProviderBanks_TargetProviderBankId",
                table: "BankTransactions",
                column: "TargetProviderBankId",
                principalTable: "ProviderBanks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactions_BankAccounts_TargetAppBankAccountId",
                table: "BankTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactions_ProviderBanks_TargetProviderBankId",
                table: "BankTransactions");

            migrationBuilder.DropIndex(
                name: "IX_BankTransactions_TargetAppBankAccountId",
                table: "BankTransactions");

            migrationBuilder.DropColumn(
                name: "TargetAppBankAccountId",
                table: "BankTransactions");

            migrationBuilder.RenameColumn(
                name: "TargetProviderBankId",
                table: "BankTransactions",
                newName: "TargetBankId");

            migrationBuilder.RenameIndex(
                name: "IX_BankTransactions_TargetProviderBankId",
                table: "BankTransactions",
                newName: "IX_BankTransactions_TargetBankId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactions_ProviderBanks_TargetBankId",
                table: "BankTransactions",
                column: "TargetBankId",
                principalTable: "ProviderBanks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
