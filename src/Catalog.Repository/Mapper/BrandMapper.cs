using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Repository.Mapper
{
    public class BrandMapper : BaseEntityMap<Domain.BrandAggregate.Brand>
    {
        protected override void Map(EntityTypeBuilder<Domain.BrandAggregate.Brand> eb)
        {
            eb.Property(b => b.Name).HasColumnType("nvarchar(250)");
            eb.Property(b => b.SeoName).HasColumnType("nvarchar(250)");
            eb.Property(b => b.LogoUrl).HasColumnType("nvarchar(500)");
            eb.Property(b => b.WebSite).HasColumnType("nvarchar(500)");
            eb.ToTable("Brand");
        }
    }

}
