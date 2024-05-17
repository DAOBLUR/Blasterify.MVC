using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blasterify.Services.Migrations
{
    public partial class RentUpdated2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AdminUsers",
                table: "AdminUsers");

            migrationBuilder.RenameTable(
                name: "AdminUsers",
                newName: "AdminUser");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Rents",
                newName: "StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_AdminUsers_Email",
                table: "AdminUser",
                newName: "IX_AdminUser_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdminUser",
                table: "AdminUser",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AdminUser",
                table: "AdminUser");

            migrationBuilder.RenameTable(
                name: "AdminUser",
                newName: "AdminUsers");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "Rents",
                newName: "Status");

            migrationBuilder.RenameIndex(
                name: "IX_AdminUser_Email",
                table: "AdminUsers",
                newName: "IX_AdminUsers_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdminUsers",
                table: "AdminUsers",
                column: "Id");
        }
    }
}
