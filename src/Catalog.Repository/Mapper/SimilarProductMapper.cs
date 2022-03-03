using Catalog.Domain.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Repository.Mapper
{
    public class SimilarProductMapper : BaseEntityMap<SimilarProduct>
    {
        protected override void Map(EntityTypeBuilder<SimilarProduct> eb)
        {
            eb.Property(x => x.ProductId).HasColumnType("uniqueidentifier");
            eb.Property(x => x.SecondProductId).HasColumnType("uniqueidentifier");

            eb.ToTable("SimilarProduct");
        }
    }
}
