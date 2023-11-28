using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiMarketing.Migrations
{
    /// <inheritdoc />
    public partial class Yeniroller : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rollers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rollers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rollers_Registers_UserId",
                        column: x => x.UserId,
                        principalTable: "Registers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rollers_UserId",
                table: "Rollers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rollers");
        }
    }
}
