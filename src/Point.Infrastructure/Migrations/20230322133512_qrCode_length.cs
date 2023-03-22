using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class qrCode_length : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "QrCode_Code",
                table: "Cards",
                type: "nvarchar(2500)",
                maxLength: 2500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "QrCode_Code",
                table: "Cards",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2500)",
                oldMaxLength: 2500);
        }
    }
}
