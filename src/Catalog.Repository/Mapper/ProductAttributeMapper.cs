using Catalog.Domain.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Repository.Mapper
{
    public class ProductAttributeMapper : BaseEntityMap<ProductAttribute>
    {
        protected override void Map(EntityTypeBuilder<ProductAttribute> eb)
        {
            eb.Property(b => b.ProductId).HasColumnType("uniqueidentifier");
            eb.Property(b => b.AttributeId).HasColumnType("uniqueidentifier");
            eb.Property(b => b.AttributeValueId).HasColumnType("uniqueidentifier");
            eb.Property(b => b.IsVariantable).HasColumnType("bit");

            eb.ToTable("ProductAttribute");
        }
    }
}
