using Catalog.Domain.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Repository.Mapper
{
    public class ProductChannelMapper : BaseEntityMap<ProductChannel>
    {
        protected override void Map(EntityTypeBuilder<ProductChannel> eb)
        {
            eb.Property(x => x.ProductId).HasColumnType("uniqueidentifier");
            eb.Property(x => x.ChannelCode).HasColumnType("int");

            eb.ToTable("ProductChannel");

        }
    }
}