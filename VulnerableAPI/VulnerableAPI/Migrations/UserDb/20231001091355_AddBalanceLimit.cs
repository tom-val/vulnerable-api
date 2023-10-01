using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VulnerableAPI.Migrations.UserDb
{
    /// <inheritdoc />
    public partial class AddBalanceLimit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BalanceLimit",
                table: "Ledgers",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BalanceLimit",
                table: "Ledgers");
        }
    }
}
