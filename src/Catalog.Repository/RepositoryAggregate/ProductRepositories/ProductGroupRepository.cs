using Catalog.Domain.ProductAggregate;

namespace Catalog.Repository.RepositoryAggregate.ProductRepositories
{
    public class ProductGroupRepository : GenericRepository<ProductGroup>, IProductGroupRepository
    {
        public ProductGroupRepository(CatalogDbContext context) : base(context)
        {
        }

    }
}
