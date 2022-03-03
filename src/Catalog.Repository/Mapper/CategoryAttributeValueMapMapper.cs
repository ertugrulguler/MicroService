using Catalog.Domain.CategoryAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Repository.Mapper
{
    public class CategoryAttributeValueMapMapper : BaseEntityMap<CategoryAttributeValueMap>
    {
        protected override void Map(EntityTypeBuilder<CategoryAttributeValueMap> eb)
        {
            eb.Property(b => b.CategoryAttributeId).HasColumnType("uniqueidentifier");
            eb.Property(b => b.AttributeValueId).HasColumnType("uniqueidentifier");

            eb.ToTable("CategoryAttributeValueMap");
        }
    }
}
