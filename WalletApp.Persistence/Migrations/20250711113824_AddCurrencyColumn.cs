using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WalletApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencyColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Transections_TransectionId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletTransfers_Transections_TransectionId",
                table: "WalletTransfers");

            migrationBuilder.DropTable(
                name: "BankTransections");

            migrationBuilder.DropTable(
                name: "Transections");

            migrationBuilder.RenameColumn(
                name: "TransectionId",
                table: "WalletTransfers",
                newName: "TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_WalletTransfers_TransectionId",
                table: "WalletTransfers",
                newName: "IX_WalletTransfers_TransactionId");

            migrationBuilder.RenameColumn(
                name: "TransectionId",
                table: "Payments",
                newName: "TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_TransectionId",
                table: "Payments",
                newName: "IX_Payments_TransactionId");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Wallets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WalletId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BankTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    ProviderBankId = table.Column<int>(type: "int", nullable: false),
                    Iban = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    TargetBankId = table.Column<int>(type: "int", nullable: false),
                    SourceBankId = table.Column<int>(type: "int", nullable: false),
                    Commission = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankTransactions_ProviderBank_SourceBankId",
                        column: x => x.SourceBankId,
                        principalTable: "ProviderBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BankTransactions_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactions_SourceBankId",
                table: "BankTransactions",
                column: "SourceBankId");

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactions_TransactionId",
                table: "BankTransactions",
                column: "TransactionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_WalletId",
                table: "Transactions",
                column: "WalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Transactions_TransactionId",
                table: "Payments",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletTransfers_Transactions_TransactionId",
                table: "WalletTransfers",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Transactions_TransactionId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletTransfers_Transactions_TransactionId",
                table: "WalletTransfers");

            migrationBuilder.DropTable(
                name: "BankTransactions");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Wallets");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "WalletTransfers",
                newName: "TransectionId");

            migrationBuilder.RenameIndex(
                name: "IX_WalletTransfers_TransactionId",
                table: "WalletTransfers",
                newName: "IX_WalletTransfers_TransectionId");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "Payments",
                newName: "TransectionId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_TransactionId",
                table: "Payments",
                newName: "IX_Payments_TransectionId");

            migrationBuilder.CreateTable(
                name: "Transections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WalletId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Created = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transections_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BankTransections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceBankId = table.Column<int>(type: "int", nullable: false),
                    TransectionId = table.Column<int>(type: "int", nullable: false),
                    Commission = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Iban = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProviderBankId = table.Column<int>(type: "int", nullable: false),
                    TargetBankId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankTransections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankTransections_ProviderBank_SourceBankId",
                        column: x => x.SourceBankId,
                        principalTable: "ProviderBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BankTransections_Transections_TransectionId",
                        column: x => x.TransectionId,
                        principalTable: "Transections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankTransections_SourceBankId",
                table: "BankTransections",
                column: "SourceBankId");

            migrationBuilder.CreateIndex(
                name: "IX_BankTransections_TransectionId",
                table: "BankTransections",
                column: "TransectionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transections_WalletId",
                table: "Transections",
                column: "WalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Transections_TransectionId",
                table: "Payments",
                column: "TransectionId",
                principalTable: "Transections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletTransfers_Transections_TransectionId",
                table: "WalletTransfers",
                column: "TransectionId",
                principalTable: "Transections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
