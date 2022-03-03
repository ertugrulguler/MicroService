using System.Collections.Generic;

namespace Catalog.Domain.ProductAggregate
{
    public interface IProductGroupVariantRepository : IGenericRepository<ProductGroupVariant>
    {
        List<ProductGroupVariant> GetProductVariantListWithinGroupCodeList(List<string> groupCodes);
    }


}