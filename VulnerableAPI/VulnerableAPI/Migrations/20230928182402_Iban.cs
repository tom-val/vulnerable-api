using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VulnerableAPI.Migrations
{
    /// <inheritdoc />
    public partial class Iban : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Iban",
                table: "Ledgers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Iban",
                table: "Ledgers");
        }
    }
}
