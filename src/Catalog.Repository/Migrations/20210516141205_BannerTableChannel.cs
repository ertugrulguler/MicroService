using Microsoft.EntityFrameworkCore.Migrations;

namespace Catalog.Repository.Migrations
{
    public partial class BannerTableChannel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductChannelCode",
                table: "Banner");

            migrationBuilder.AddColumn<int>(
                name: "ProductChannelCode",
                table: "BannerLocation",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductChannelCode",
                table: "BannerLocation");

            migrationBuilder.AddColumn<int>(
                name: "ProductChannelCode",
                table: "Banner",
                type: "int",
                nullable: true);
        }
    }
}
