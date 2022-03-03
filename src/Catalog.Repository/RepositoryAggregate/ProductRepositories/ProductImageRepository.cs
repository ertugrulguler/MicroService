using Catalog.Domain.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Repository.RepositoryAggregate.ProductRepositories
{
    public class ProductImageRepository : GenericRepository<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(CatalogDbContext context) : base(context)
        {
        }

        public async Task<List<ProductImage>> GetProductImagesByProductIds(List<Guid> productIds)
        {
            return await _entities.AsQueryable().Where(pi => productIds.Contains(pi.ProductId.Value)).ToListAsync();
        }

    }
}