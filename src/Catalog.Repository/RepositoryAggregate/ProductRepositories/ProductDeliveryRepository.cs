using Catalog.Domain.ProductAggregate;

namespace Catalog.Repository.RepositoryAggregate.ProductRepositories
{
    public class ProductDeliveryRepository : GenericRepository<ProductDelivery>, IProductDeliveryRepository
    {
        public ProductDeliveryRepository(CatalogDbContext context) : base(context)
        {
        }
    }
}
