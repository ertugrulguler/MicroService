using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Catalog.Repository.Migrations
{
    public partial class CategoryAttributeValueMap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FilterOrder",
                table: "CategoryAttribute",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsFilter",
                table: "CategoryAttribute",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Category",
                type: "nvarchar(15)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "AttributeValue",
                type: "nvarchar(15)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Attribute",
                type: "nvarchar(15)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CategoryAttributeValueMap",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryAttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryAttributeValueMap", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttributeMap_AttributeId",
                table: "AttributeMap",
                column: "AttributeId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_AttributeMap_Attribute_AttributeId",
            //    table: "AttributeMap",
            //    column: "AttributeId",
            //    principalTable: "Attribute",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttributeMap_Attribute_AttributeId",
                table: "AttributeMap");

            migrationBuilder.DropTable(
                name: "CategoryAttributeValueMap");

            migrationBuilder.DropIndex(
                name: "IX_AttributeMap_AttributeId",
                table: "AttributeMap");

            migrationBuilder.DropColumn(
                name: "FilterOrder",
                table: "CategoryAttribute");

            migrationBuilder.DropColumn(
                name: "IsFilter",
                table: "CategoryAttribute");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Category",
                type: "nvarchar(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "AttributeValue",
                type: "nvarchar(500)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Attribute",
                type: "nvarchar(500)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldNullable: true);
        }
    }
}
