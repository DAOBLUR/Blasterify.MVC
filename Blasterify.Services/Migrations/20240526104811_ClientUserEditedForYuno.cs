using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blasterify.Services.Migrations
{
    public partial class ClientUserEditedForYuno : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreRentItems");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "ClientUsers",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "ClientUsers",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "YunoId",
                table: "ClientUsers",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "ClientUsers");

            migrationBuilder.DropColumn(
                name: "YunoId",
                table: "ClientUsers");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "ClientUsers",
                newName: "Username");

            migrationBuilder.CreateTable(
                name: "PreRentItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    RentDuration = table.Column<int>(type: "int", nullable: false),
                    RentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreRentItems", x => x.Id);
                });
        }
    }
}
