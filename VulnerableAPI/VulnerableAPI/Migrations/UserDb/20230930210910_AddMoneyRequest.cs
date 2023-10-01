using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VulnerableAPI.Migrations.UserDb
{
    /// <inheritdoc />
    public partial class AddMoneyRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "MoneyRequests",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "MoneyRequests",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RequestReason",
                table: "MoneyRequests",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "MoneyRequests");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "MoneyRequests");

            migrationBuilder.DropColumn(
                name: "RequestReason",
                table: "MoneyRequests");
        }
    }
}
