using Catalog.Domain.BannerAggregate;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Repository.Mapper
{
    public class BannerLocationMapper : BaseEntityMap<BannerLocation>
    {
        protected override void Map(EntityTypeBuilder<BannerLocation> eb)
        {
            eb.Property(b => b.Title).HasColumnType("nvarchar(300)");
            eb.Property(b => b.Order).HasColumnType("int");
            eb.Property(b => b.BannerType).HasColumnType("int");
            eb.Property(b => b.Location).HasColumnType("int");
            eb.Property(b => b.Description).HasColumnType("nvarchar(500)");
            eb.Property(b => b.ActionId).HasColumnType("uniqueidentifier");
            eb.ToTable("BannerLocation");
        }
    }
}
