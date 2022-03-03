using Catalog.Domain.BannerAggregate;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Repository.Mapper
{
    public class BannerMapper : BaseEntityMap<Banner>
    {
        protected override void Map(EntityTypeBuilder<Banner> eb)
        {
            eb.Property(b => b.ActionId).HasColumnType("uniqueidentifier");
            eb.Property(b => b.BannerLocationId).HasColumnType("uniqueidentifier");
            eb.Property(b => b.Name).HasColumnType("nvarchar(100)");
            eb.Property(b => b.Description).HasColumnType("nvarchar(500)");
            eb.Property(b => b.ActionType).HasColumnType("int");
            eb.Property(b => b.Order).HasColumnType("int");
            eb.Property(b => b.MMActionId).HasColumnType("int");
            eb.Property(b => b.ImageUrl).HasColumnType("nvarchar(500)");
            eb.Property(b => b.StartDate).HasColumnType("datetime");
            eb.Property(b => b.EndDate).HasColumnType("datetime");
            eb.Property(b => b.MinAndroidVersion).HasColumnType("nvarchar(100)");
            eb.Property(b => b.MinIosVersion).HasColumnType("nvarchar(100)");
            eb.ToTable("Banner");
        }
    }
}
