using Catalog.Domain.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Repository.Mapper
{
    public class ProductGroupMapper : BaseEntityMap<ProductGroup>
    {
        protected override void Map(EntityTypeBuilder<ProductGroup> eb)
        {
            eb.Property(b => b.ProductId).HasColumnType("uniqueidentifier");
            eb.Property(b => b.GroupCode).HasColumnType("nvarchar(100)");

            eb.ToTable("ProductGroup");
        }
    }
}
