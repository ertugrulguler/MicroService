using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Repository.Mapper
{
    public class CategoryImageMapper : BaseEntityMap<Domain.CategoryAggregate.CategoryImage>
    {
        protected override void Map(EntityTypeBuilder<Domain.CategoryAggregate.CategoryImage> eb)
        {
            eb.Property(b => b.Name).HasColumnType("nvarchar(100)");
            eb.Property(b => b.Url).HasColumnType("nvarchar(500)");
            eb.Property(b => b.Description).HasColumnType("nvarchar(250)");
            eb.Property(b => b.CategoryId).HasColumnType("uniqueidentifier");
            eb.ToTable("CategoryImage");
        }
    }

}
