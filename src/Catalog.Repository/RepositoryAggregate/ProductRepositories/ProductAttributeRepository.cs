using Catalog.Domain.ProductAggregate;

namespace Catalog.Repository.RepositoryAggregate.ProductRepositories
{
    public class ProductAttributeRepository : GenericRepository<ProductAttribute>, IProductAttributeRepository
    {
        public ProductAttributeRepository(CatalogDbContext context) : base(context)
        {
        }
    }
}