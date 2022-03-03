using Catalog.Domain.AttributeAggregate.ServiceModels;
using Catalog.Domain.CategoryAggregate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Domain.AttributeAggregate
{
    public interface IAttributeDomainService
    {
        Task<Dictionary<string, Guid>> GetAttributeValueNameWithMap(List<Guid> list, List<CategoryAttribute> attList);
        Task<Dictionary<string, AttributeIdAndRequiredList>> GetAttributeName(List<AttributeIdAndRequiredList> list);
        Task<Dictionary<string, Guid>> GetAttributeValueName(List<Guid> list);
    }
}
