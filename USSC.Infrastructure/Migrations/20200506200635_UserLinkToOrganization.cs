using Microsoft.EntityFrameworkCore.Migrations;

namespace USSC.Infrastructure.Migrations
{
    public partial class UserLinkToOrganization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Users_RepresentativeId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_RepresentativeId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "RepresentativeId",
                table: "Organizations");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Organizations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_UserId",
                table: "Organizations",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Users_UserId",
                table: "Organizations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Users_UserId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_UserId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Organizations");

            migrationBuilder.AddColumn<int>(
                name: "RepresentativeId",
                table: "Organizations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_RepresentativeId",
                table: "Organizations",
                column: "RepresentativeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Users_RepresentativeId",
                table: "Organizations",
                column: "RepresentativeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
