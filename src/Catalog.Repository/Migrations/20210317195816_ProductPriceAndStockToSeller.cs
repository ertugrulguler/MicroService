using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Catalog.Repository.Migrations
{
    public partial class ProductPriceAndStockToSeller : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductPrice");

            migrationBuilder.DropTable(
                name: "ProductStock");

            migrationBuilder.DropColumn(
                name: "ChannelCode",
                table: "Product");

            migrationBuilder.AddColumn<Guid>(
                name: "CurrencyId",
                table: "ProductSeller",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DiscountId",
                table: "ProductSeller",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InstallmentCount",
                table: "ProductSeller",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ListPrice",
                table: "ProductSeller",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SalePrice",
                table: "ProductSeller",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "StockCount",
                table: "ProductSeller",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VatRate",
                table: "ProductSeller",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Desi",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "ProductSeller");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "ProductSeller");

            migrationBuilder.DropColumn(
                name: "InstallmentCount",
                table: "ProductSeller");

            migrationBuilder.DropColumn(
                name: "ListPrice",
                table: "ProductSeller");

            migrationBuilder.DropColumn(
                name: "SalePrice",
                table: "ProductSeller");

            migrationBuilder.DropColumn(
                name: "StockCount",
                table: "ProductSeller");

            migrationBuilder.DropColumn(
                name: "VatRate",
                table: "ProductSeller");

            migrationBuilder.DropColumn(
                name: "Desi",
                table: "Product");

            migrationBuilder.AddColumn<string>(
                name: "ChannelCode",
                table: "Product",
                type: "nvarchar(25)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ProductPrice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChannelCode = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiscountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InstallmentCount = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ListPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ProductSellerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatRate = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPrice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPrice_ProductSeller_ProductSellerId",
                        column: x => x.ProductSellerId,
                        principalTable: "ProductSeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductStock",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChannelCode = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ProductSellerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StockCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductStock_ProductSeller_ProductSellerId",
                        column: x => x.ProductSellerId,
                        principalTable: "ProductSeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrice_ProductSellerId",
                table: "ProductPrice",
                column: "ProductSellerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStock_ProductSellerId",
                table: "ProductStock",
                column: "ProductSellerId");
        }
    }
}
