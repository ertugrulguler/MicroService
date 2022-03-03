using Catalog.Domain.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Repository.RepositoryAggregate.ProductRepositories
{
    public class ProductSellerRepository : GenericRepository<ProductSeller>, IProductSellerRepository
    {
        public ProductSellerRepository(CatalogDbContext context) : base(context)
        {
        }
        public async Task<List<Guid>> GetProductIdsByProductSellerIds(List<Guid> productSellerIds)
        {
            return await _entities.Where(ps => productSellerIds.Contains(ps.Id)).Select(ps => ps.ProductId).ToListAsync();
        }

    }
}