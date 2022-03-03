using Catalog.Domain.CategoryAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Repository.Mapper
{
    public class CategoryAttributeMapper : BaseEntityMap<CategoryAttribute>
    {
        protected override void Map(EntityTypeBuilder<CategoryAttribute> eb)
        {
            eb.Property(b => b.CategoryId).HasColumnType("uniqueidentifier");
            eb.Property(b => b.AttributeId).HasColumnType("uniqueidentifier");
            eb.Property(b => b.IsRequired).HasColumnType("bit");
            eb.Property(b => b.IsVariantable).HasColumnType("bit");
            eb.Property(b => b.IsListed).HasColumnType("bit");
            eb.Property(b => b.IsFilter).HasColumnType("bit");
            eb.Property(b => b.FilterOrder).HasColumnType("int");
            eb.Property(b => b.Order).HasColumnType("int");

            eb.ToTable("CategoryAttribute");
        }
    }
}