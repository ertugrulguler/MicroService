using Catalog.Domain.BannerAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Repository.Mapper
{
    public class BannerFiltersMapper : BaseEntityMap<BannerFilters>
    {
        protected override void Map(EntityTypeBuilder<BannerFilters> eb)
        {
            eb.Property(b => b.ActionId).HasColumnType("uniqueidentifier");
            eb.Property(b => b.BannerId).HasColumnType("uniqueidentifier");
            eb.Property(b => b.BannerFilterType).HasColumnType("int");
            eb.ToTable("BannerFilters");
        }
    }
}