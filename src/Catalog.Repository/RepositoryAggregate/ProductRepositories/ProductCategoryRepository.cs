using Catalog.Domain.ProductAggregate;

namespace Catalog.Repository.RepositoryAggregate.ProductRepositories
{
    public class ProductCategoryRepository : GenericRepository<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(CatalogDbContext context) : base(context)
        {
        }
    }
}