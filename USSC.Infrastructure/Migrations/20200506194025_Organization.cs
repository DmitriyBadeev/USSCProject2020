using Microsoft.EntityFrameworkCore.Migrations;

namespace USSC.Infrastructure.Migrations
{
    public partial class Organization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "Employees",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    INN = table.Column<string>(nullable: true),
                    OGRN = table.Column<string>(nullable: true),
                    RepresentativeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organizations_Users_RepresentativeId",
                        column: x => x.RepresentativeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_OrganizationId",
                table: "Employees",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_RepresentativeId",
                table: "Organizations",
                column: "RepresentativeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Organizations_OrganizationId",
                table: "Employees",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Organizations_OrganizationId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Employees_OrganizationId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Employees");
        }
    }
}
