using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Catalog.Repository.Migrations
{
    public partial class CategoryChannelCodeOmitted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChannelCode",
                table: "Category");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "CategoryImage",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryImage_CategoryId",
                table: "CategoryImage",
                column: "CategoryId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryImage_Category_CategoryId",
                table: "CategoryImage",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryImage_Category_CategoryId",
                table: "CategoryImage");

            migrationBuilder.DropIndex(
                name: "IX_CategoryImage_CategoryId",
                table: "CategoryImage");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "CategoryImage",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "ChannelCode",
                table: "Category",
                type: "nvarchar(25)",
                nullable: false,
                defaultValue: "");
        }
    }
}
