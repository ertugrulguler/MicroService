using Catalog.Domain.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Repository.RepositoryAggregate.ProductRepositories
{
    public class ProductChannelRepository : GenericRepository<ProductChannel>, IProductChannelRepository
    {
        public ProductChannelRepository(CatalogDbContext context) : base(context)
        {
        }

        public async Task<ProductChannel> GetProductChannelAll(Guid productId)
        {
            return await _entities.Where(pc => pc.ProductId == productId).FirstOrDefaultAsync();
        }

    }
}