using Catalog.Domain.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Repository.Mapper
{
    public class ProductDeliveryMapper : BaseEntityMap<ProductDelivery>
    {
        protected override void Map(EntityTypeBuilder<ProductDelivery> eb)
        {
            eb.Property(b => b.ProductId).HasColumnType("uniqueidentifier");
            eb.Property(b => b.SellerId).HasColumnType("uniqueidentifier");
            eb.Property(b => b.DeliveryId).HasColumnType("uniqueidentifier");
            eb.Property(b => b.CityId).HasColumnType("uniqueidentifier");
            eb.Property(b => b.DeliveryType).HasColumnType("int");

            eb.ToTable("ProductDelivery");
        }
    }
}
