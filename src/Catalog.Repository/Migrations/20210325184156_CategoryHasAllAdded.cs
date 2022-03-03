using Microsoft.EntityFrameworkCore.Migrations;

namespace Catalog.Repository.Migrations
{
    public partial class CategoryHasAllAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasAll",
                table: "Category",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasAll",
                table: "Category");
        }
    }
}
