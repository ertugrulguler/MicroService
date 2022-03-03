using Catalog.Domain.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Repository.Mapper
{
    public class ProductSellerMapper : BaseEntityMap<ProductSeller>
    {
        protected override void Map(EntityTypeBuilder<ProductSeller> eb)
        {
            eb.Property(x => x.ProductId).HasColumnType("uniqueidentifier");
            eb.Property(x => x.SellerId).HasColumnType("uniqueidentifier");
            eb.Property(b => b.StockCode).HasColumnType("nvarchar(100)");
            eb.Property(b => b.StockCount).HasColumnType("int");
            eb.Property(b => b.CurrencyId).HasColumnType("uniqueidentifier");
            eb.Property(b => b.ListPrice).HasColumnType("decimal(18,2)");
            eb.Property(b => b.SalePrice).HasColumnType("decimal(18,2)");
            eb.Property(b => b.DiscountId).HasColumnType("uniqueidentifier");
            eb.Property(b => b.InstallmentCount).HasColumnType("int");
            eb.Property(b => b.DisplayOrder).HasColumnType("int");
            eb.Property(b => b.DisplayOrder).HasDefaultValue(10000);
            eb.ToTable("ProductSeller");
        }
    }
}
