using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Domain.ProductAggregate
{
    public interface IProductDomainService
    {
        Task<List<ProductGroup>> SaveProductGroup(List<ProductGroup> productGroups, Guid productId);
        Task<List<ProductGroupVariant>> SaveProductGroupVariant(string groupCode, Guid categoryId, IReadOnlyCollection<ProductAttribute> productAttribute);
    }
}
