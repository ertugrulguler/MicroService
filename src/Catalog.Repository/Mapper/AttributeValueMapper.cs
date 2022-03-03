using Catalog.Domain.AttributeAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Repository.Mapper
{
    public class AttributeValueMapper : BaseEntityMap<AttributeValue>
    {
        protected override void Map(EntityTypeBuilder<AttributeValue> eb)
        {
            eb.Property(x => x.AttributeId).HasColumnType("uniqueidentifier");
            eb.Property(x => x.Value).HasColumnType("nvarchar(500)");
            eb.Property(x => x.Unit).HasColumnType("nvarchar(50)");
            eb.Property(x => x.Order).HasColumnType("int");
            eb.Property(b => b.Code).HasColumnType("nvarchar(15)");
            eb.Property(b => b.SeoName).HasColumnType("nvarchar(500)");
            eb.ToTable("AttributeValue");
        }
    }
}
