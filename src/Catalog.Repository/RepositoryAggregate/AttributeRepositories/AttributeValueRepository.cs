using Catalog.Domain.AttributeAggregate;
using System.Collections.Generic;

namespace Catalog.Repository.RepositoryAggregate.AttributeRepositories
{
    public class AttributeValueRepository : GenericRepository<AttributeValue>, IAttributeValueRepository
    {
        public AttributeValueRepository(CatalogDbContext context) : base(context)
        {

        }
        public List<AttributeValue> GetAttributeValueOrder(List<AttributeValue> attValueIdList)
        {
            var list = new List<AttributeValue>();
            var order = 1;

            foreach (var attributeValue in attValueIdList)
            {
                var t = new AttributeValue(attributeValue.Id, attributeValue.Value, attributeValue.Code, attributeValue.IsActive, order, attributeValue.SeoName);
                list.Add(t);
                order++;
            }
            return list;
        }
    }
}
