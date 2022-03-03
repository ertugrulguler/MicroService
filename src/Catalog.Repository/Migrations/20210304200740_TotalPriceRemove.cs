using Microsoft.EntityFrameworkCore.Migrations;

namespace Catalog.Repository.Migrations
{
    public partial class TotalPriceRemove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImage_Image_ImageId",
                table: "ProductImage");

            migrationBuilder.DropIndex(
                name: "IX_ProductImage_ImageId",
                table: "ProductImage");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "ProductPrice");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ProductImage",
                type: "nvarchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ProductImage",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "ProductImage",
                type: "nvarchar(500)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductImage_ImageId",
                table: "ProductImage",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductGroup_ProductId",
                table: "ProductGroup",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductGroup_Product_ProductId",
                table: "ProductGroup",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImage_Image_ImageId",
                table: "ProductImage",
                column: "ImageId",
                principalTable: "Image",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductGroup_Product_ProductId",
                table: "ProductGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductImage_Image_ImageId",
                table: "ProductImage");

            migrationBuilder.DropIndex(
                name: "IX_ProductImage_ImageId",
                table: "ProductImage");

            migrationBuilder.DropIndex(
                name: "IX_ProductGroup_ProductId",
                table: "ProductGroup");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ProductImage");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ProductImage");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "ProductImage");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "ProductPrice",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_ProductImage_ImageId",
                table: "ProductImage",
                column: "ImageId",
                unique: true,
                filter: "[ImageId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImage_Image_ImageId",
                table: "ProductImage",
                column: "ImageId",
                principalTable: "Image",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
