using Microsoft.EntityFrameworkCore.Migrations;

namespace USSC.Infrastructure.Migrations
{
    public partial class FixPosition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Employees_EmployeeId",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_Positions_EmployeeId",
                table: "Positions");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "Positions",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Positions_EmployeeId",
                table: "Positions",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_Employees_EmployeeId",
                table: "Positions",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Employees_EmployeeId",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_Positions_EmployeeId",
                table: "Positions");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "Positions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Positions_EmployeeId",
                table: "Positions",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_Employees_EmployeeId",
                table: "Positions",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
