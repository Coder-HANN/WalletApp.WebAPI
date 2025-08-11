using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WalletApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixBankTransactionRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactions_ProviderBanks_SourceBankId",
                table: "BankTransactions");

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactions_TargetBankId",
                table: "BankTransactions",
                column: "TargetBankId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactions_ProviderBanks_TargetBankId",
                table: "BankTransactions",
                column: "TargetBankId",
                principalTable: "ProviderBanks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactions_ProviderBanks_TargetBankId",
                table: "BankTransactions");

            migrationBuilder.DropIndex(
                name: "IX_BankTransactions_TargetBankId",
                table: "BankTransactions");

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactions_ProviderBanks_SourceBankId",
                table: "BankTransactions",
                column: "SourceBankId",
                principalTable: "ProviderBanks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
