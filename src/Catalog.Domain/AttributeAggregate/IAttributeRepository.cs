using Catalog.Domain.ValueObject.StoreProcedure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Domain.AttributeAggregate
{
    public interface IAttributeRepository : IGenericRepository<Attribute>
    {
        Task<List<Guid>> GetAttributeMapList(List<Guid> attIdList);
        Task<List<AttributeFilter>> GetProductAttributeFilter(List<Guid> categoryId,
            List<Guid> sellerIdList, List<List<Guid>> attributeIds, List<string> salePriceList, List<Guid> brandIdList,
            List<string> codeList,
            List<Guid> searchList, List<Guid> bannedSellers, int productChannel,
            List<Guid> sellerList);
        Task<List<AttributeFilter>> GetProductAttributeFilter(string attributeFilterQuery);
    }
}