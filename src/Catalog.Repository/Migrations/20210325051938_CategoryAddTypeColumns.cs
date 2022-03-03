using Microsoft.EntityFrameworkCore.Migrations;

namespace Catalog.Repository.Migrations
{
    public partial class CategoryAddTypeColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Virtual",
                table: "Category");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Category",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Category");

            migrationBuilder.AddColumn<bool>(
                name: "Virtual",
                table: "Category",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
