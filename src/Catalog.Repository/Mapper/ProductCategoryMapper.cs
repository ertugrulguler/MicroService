using Catalog.Domain.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Repository.Mapper
{
    public class ProductCategoryMapper : BaseEntityMap<ProductCategory>
    {
        protected override void Map(EntityTypeBuilder<ProductCategory> eb)
        {
            eb.Property(x => x.CategoryId).HasColumnType("uniqueidentifier");
            eb.Property(x => x.ProductId).HasColumnType("uniqueidentifier");
            eb.ToTable("ProductCategory");
        }
    }
}
