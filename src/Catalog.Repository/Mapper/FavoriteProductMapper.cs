using Catalog.Domain.ProductAggregate;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Repository.Mapper
{
    public class FavoriteProductMapper : BaseEntityMap<FavoriteProduct>
    {
        protected override void Map(EntityTypeBuilder<FavoriteProduct> eb)
        {
            eb.Property(b => b.ProductId).HasColumnType("uniqueidentifier");
            eb.Property(b => b.CustomerId).HasColumnType("uniqueidentifier");

            eb.ToTable("FavoriteProduct");
        }
    }
}
