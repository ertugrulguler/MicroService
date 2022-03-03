using Catalog.Domain.ProductAggregate;
using System.Collections.Generic;
using System.Linq;

namespace Catalog.Repository.RepositoryAggregate.ProductRepositories
{
    public class ProductGroupVariantRepository : GenericRepository<ProductGroupVariant>, IProductGroupVariantRepository
    {
        public ProductGroupVariantRepository(CatalogDbContext context) : base(context)
        {
        }

        public List<ProductGroupVariant> GetProductVariantListWithinGroupCodeList(List<string> groupCodes)
        {
            return _entities.AsEnumerable()
                .Where(x => groupCodes.Any(w => x.ProductGroupCode.Contains(w))).ToList();
        }
    }
}