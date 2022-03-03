using Catalog.Domain.AttributeAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Repository.Mapper
{
    public class AttributeMapper : BaseEntityMap<Attribute>
    {
        protected override void Map(EntityTypeBuilder<Attribute> eb)
        {
            eb.Property(x => x.Code).HasColumnType("nvarchar(15)");
            eb.Property(b => b.Name).HasColumnType("nvarchar(100)");
            eb.Property(b => b.Description).HasColumnType("nvarchar(3000)");
            eb.Property(b => b.SeoName).HasColumnType("nvarchar(100)");
            eb.HasMany(b => b.AttributeValues)
              .WithOne()
              .HasForeignKey("AttributeId")
              .OnDelete(DeleteBehavior.Cascade);

            eb.ToTable("Attribute");
        }
    }
}