using Microsoft.EntityFrameworkCore.Migrations;

namespace Catalog.Repository.Migrations
{
    public partial class CategoryTCKNControl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRequiredIdNumber",
                table: "Category",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRequiredIdNumber",
                table: "Category");
        }
    }
}
