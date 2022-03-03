using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Catalog.Repository.Migrations
{
    public partial class ProductImageIdRemove : Migration
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
                name: "ImageId",
                table: "ProductImage");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "ProductImage",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductImage_ImageId",
                table: "ProductImage",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImage_Image_ImageId",
                table: "ProductImage",
                column: "ImageId",
                principalTable: "Image",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
