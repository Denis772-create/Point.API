using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class qrCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QrCode",
                table: "Cards",
                newName: "QrCode_Code");

            migrationBuilder.AlterColumn<string>(
                name: "CardNumber",
                table: "Cards",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QrCode_Code",
                table: "Cards",
                newName: "QrCode");

            migrationBuilder.AlterColumn<string>(
                name: "CardNumber",
                table: "Cards",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldMaxLength: 12);
        }
    }
}
