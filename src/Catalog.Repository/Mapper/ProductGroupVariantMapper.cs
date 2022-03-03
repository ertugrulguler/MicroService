using Catalog.Domain.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Repository.Mapper
{
    public class ProductGroupVariantMapper : BaseEntityMap<ProductGroupVariant>
    {
        protected override void Map(EntityTypeBuilder<ProductGroupVariant> eb)
        {
            eb.Property(b => b.AttributeId).HasColumnType("uniqueidentifier");
            eb.Property(b => b.ProductGroupCode).HasColumnType("nvarchar(100)");
            //eb.Ignore(b => b.ProductGroupCode);

            eb.ToTable("ProductGroupVariant");
        }
    }
}
