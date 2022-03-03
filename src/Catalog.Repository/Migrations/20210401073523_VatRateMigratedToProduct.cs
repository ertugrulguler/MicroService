using Microsoft.EntityFrameworkCore.Migrations;

namespace Catalog.Repository.Migrations
{
    public partial class VatRateMigratedToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VatRate",
                table: "ProductSeller");

            migrationBuilder.AddColumn<int>(
                name: "VatRate",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VatRate",
                table: "Product");

            migrationBuilder.AddColumn<int>(
                name: "VatRate",
                table: "ProductSeller",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
