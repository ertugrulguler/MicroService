using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Catalog.Repository.Migrations
{
    public partial class CategorySuggestedChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainSuggested",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "Suggested",
                table: "Category");

            migrationBuilder.AddColumn<Guid>(
                name: "SuggestedMainId",
                table: "Category",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SuggestedMainId",
                table: "Category");

            migrationBuilder.AddColumn<bool>(
                name: "MainSuggested",
                table: "Category",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Suggested",
                table: "Category",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
