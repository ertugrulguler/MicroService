using Microsoft.EntityFrameworkCore.Migrations;

namespace Catalog.Repository.Migrations
{
    public partial class ProductStockChannelCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChannelCode",
                table: "ProductStock",
                type: "nvarchar(25)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChannelCode",
                table: "ProductStock");
        }
    }
}
