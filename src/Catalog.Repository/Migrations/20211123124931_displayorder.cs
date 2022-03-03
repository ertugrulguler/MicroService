using Microsoft.EntityFrameworkCore.Migrations;

namespace Catalog.Repository.Migrations
{
    public partial class displayorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "ProductSeller",
                type: "int",
                nullable: false,
                defaultValue: 10000);

            migrationBuilder.AddColumn<int>(
                name: "ProductFilterOrder",
                table: "Category",
                type: "int",
                nullable: false,
                defaultValue: 10000);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "ProductSeller");

            migrationBuilder.DropColumn(
                name: "ProductFilterOrder",
                table: "Category");
        }
    }
}
