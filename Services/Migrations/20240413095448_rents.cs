using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Migrations
{
    public partial class rents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "RentDuration",
                table: "Rents");

            migrationBuilder.RenameColumn(
                name: "RentDate",
                table: "Rents",
                newName: "BuyDate");

            migrationBuilder.CreateTable(
                name: "RentItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    RentId = table.Column<int>(type: "int", nullable: false),
                    RentDuration = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentItems", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RentItems");

            migrationBuilder.RenameColumn(
                name: "BuyDate",
                table: "Rents",
                newName: "RentDate");

            migrationBuilder.AddColumn<int>(
                name: "MovieId",
                table: "Rents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RentDuration",
                table: "Rents",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
