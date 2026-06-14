using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RevenueSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftwarePrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "YearlyCost",
                table: "Softwares",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YearlyCost",
                table: "Softwares");
        }
    }
}
