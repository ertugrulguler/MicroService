using Catalog.Domain.BannerAggregate;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Repository.Mapper
{
    public class BannerTypeMapper : BaseEntityMap<BannerType>
    {
        protected override void Map(EntityTypeBuilder<BannerType> eb)
        {
            eb.Property(b => b.Type).HasColumnType("int");
            eb.Property(b => b.Name).HasColumnType("nvarchar(100)");
            eb.Property(b => b.Description).HasColumnType("nvarchar(500)");
            eb.ToTable("BannerType");
        }
    }
}
