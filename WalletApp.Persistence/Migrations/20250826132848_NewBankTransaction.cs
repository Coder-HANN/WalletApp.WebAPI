using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WalletApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NewBankTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BankRoutes",
                table: "BankRoutes");

            migrationBuilder.DropColumn(
                name: "TargetBankCode",
                table: "BankRoutes");

            migrationBuilder.DropColumn(
                name: "ProviderBankCode",
                table: "BankRoutes");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "BankRoutes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "BankRoutes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "SourceBankId",
                table: "BankRoutes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TargetBankId",
                table: "BankRoutes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BankRoutes",
                table: "BankRoutes",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BankRoutes",
                table: "BankRoutes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BankRoutes");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "BankRoutes");

            migrationBuilder.DropColumn(
                name: "SourceBankId",
                table: "BankRoutes");

            migrationBuilder.DropColumn(
                name: "TargetBankId",
                table: "BankRoutes");

            migrationBuilder.AddColumn<string>(
                name: "TargetBankCode",
                table: "BankRoutes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProviderBankCode",
                table: "BankRoutes",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BankRoutes",
                table: "BankRoutes",
                column: "TargetBankCode");
        }
    }
}
