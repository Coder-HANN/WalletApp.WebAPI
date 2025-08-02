using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WalletApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCaptchaCodeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "UserDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "UserDetails",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "UserDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "UserDetails");
        }
    }
}
