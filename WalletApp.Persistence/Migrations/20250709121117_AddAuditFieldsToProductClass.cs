using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WalletApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditFieldsToProductClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedUser",
                table: "WalletTransfers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedUser",
                table: "Wallets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedUser",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedUser",
                table: "UserDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedUser",
                table: "Transections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedUser",
                table: "ProviderBank",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedUser",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedUser",
                table: "BankTransections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedUser",
                table: "BankaHesaps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedUser",
                table: "WalletTransfers");

            migrationBuilder.DropColumn(
                name: "CreatedUser",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "CreatedUser",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedUser",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "CreatedUser",
                table: "Transections");

            migrationBuilder.DropColumn(
                name: "CreatedUser",
                table: "ProviderBank");

            migrationBuilder.DropColumn(
                name: "CreatedUser",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CreatedUser",
                table: "BankTransections");

            migrationBuilder.DropColumn(
                name: "CreatedUser",
                table: "BankaHesaps");
        }
    }
}
