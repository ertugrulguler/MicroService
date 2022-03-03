using Catalog.Domain.BannerAggregate;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Repository.Mapper
{
    public class BannerActionTypeMapper : BaseEntityMap<BannerActionType>
    {
        protected override void Map(EntityTypeBuilder<BannerActionType> eb)
        {
            eb.Property(b => b.Type).HasColumnType("nvarchar(100)");
            eb.Property(b => b.Name).HasColumnType("nvarchar(100)");
            eb.Property(b => b.Description).HasColumnType("nvarchar(500)");
            eb.ToTable("BannerActionType");
        }
    }
}
