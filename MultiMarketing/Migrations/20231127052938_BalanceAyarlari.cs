using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiMarketing.Migrations
{
    /// <inheritdoc />
    public partial class BalanceAyarlari : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Balance",
                table: "Registers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusBalance",
                table: "Registers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Registers");

            migrationBuilder.DropColumn(
                name: "BonusBalance",
                table: "Registers");
        }
    }
}
