using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blasterify.Services.Migrations
{
    public partial class MerchantOrderAndCheckoutSessionAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreRents");

            migrationBuilder.AddColumn<string>(
                name: "CheckoutSession",
                table: "Rents",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MerchantOrderId",
                table: "ClientUsers",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckoutSession",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "MerchantOrderId",
                table: "ClientUsers");

            migrationBuilder.CreateTable(
                name: "PreRents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientUserId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreRents", x => x.Id);
                });
        }
    }
}
