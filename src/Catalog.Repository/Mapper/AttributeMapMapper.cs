using Catalog.Domain.AttributeAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Repository.Mapper
{
    public class AttributeMapMapper : BaseEntityMap<AttributeMap>
    {
        protected override void Map(EntityTypeBuilder<AttributeMap> eb)
        {
            eb.Property(x => x.AttributeId).HasColumnType("uniqueidentifier");
            eb.Property(x => x.AttributeValueId).HasColumnType("uniqueidentifier");

            eb.ToTable("AttributeMap");
        }
    }
}
