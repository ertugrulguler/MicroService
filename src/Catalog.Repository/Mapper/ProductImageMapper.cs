using Catalog.Domain.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Repository.Mapper
{
    public class ProductImageMapper : BaseEntityMap<ProductImage>
    {
        protected override void Map(EntityTypeBuilder<ProductImage> eb)
        {
            eb.Property(b => b.Name).HasColumnType("nvarchar(100)");
            eb.Property(b => b.Url).HasColumnType("nvarchar(500)");
            eb.Property(b => b.Description).HasColumnType("nvarchar(250)");
            eb.Property(b => b.ProductId).HasColumnType("uniqueidentifier");
            eb.Property(b => b.SellerId).HasColumnType("uniqueidentifier");
            eb.Property(b => b.SortOrder).HasColumnType("int");
            eb.Property(b => b.IsDefault).HasColumnType("bit");

            eb.ToTable("ProductImage");
        }
    }
}