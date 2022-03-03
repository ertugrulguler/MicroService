using Catalog.Domain.BannerAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Repository.Mapper
{
    public class BannerFilterTypeMapper : BaseEntityMap<BannerFilterType>
    {
        protected override void Map(EntityTypeBuilder<BannerFilterType> eb)
        {
            eb.Property(b => b.Type).HasColumnType("int");
            eb.Property(b => b.Description).HasColumnType("nvarchar(500)");
            eb.Property(b => b.Name).HasColumnType("nvarchar(100)");
            eb.ToTable("BannerFilterType");
        }
    }
}