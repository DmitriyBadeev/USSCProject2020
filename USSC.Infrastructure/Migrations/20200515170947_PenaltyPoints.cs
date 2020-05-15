using Microsoft.EntityFrameworkCore.Migrations;

namespace USSC.Infrastructure.Migrations
{
    public partial class PenaltyPoints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PenaltyPoints",
                table: "Employees",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PenaltyPoints",
                table: "Employees");
        }
    }
}
