using System.Collections.Generic;

namespace Catalog.Domain.AttributeAggregate
{
    public interface IAttributeValueRepository : IGenericRepository<AttributeValue>
    {
        List<AttributeValue> GetAttributeValueOrder(List<AttributeValue> attValueIdList);
    }
}
