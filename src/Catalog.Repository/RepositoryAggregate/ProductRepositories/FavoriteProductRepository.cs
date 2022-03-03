
using Catalog.Domain.ProductAggregate;
using Catalog.Domain.ProductAggregate.ServiceModels;
using Framework.Core.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Repository.RepositoryAggregate.ProductRepositories
{
    public class FavoriteProductRepository : GenericRepository<FavoriteProduct>, IFavoriteProductRepository
    {
        public FavoriteProductRepository(CatalogDbContext context) : base(context)
        {

        }
        public async Task<FavoriteRepoProductList> GetFavoriteProductsByCustomerId(Guid CustomerId, PagerInput pagerInput)
        {
            var favoriteProducts = new List<FavoriteProduct>();

            var favoriteProductsCount = await _entities.AsQueryable().AsNoTracking()
                   .Where(p => p.CustomerId == CustomerId && p.IsActive).OrderByDescending(p => p.ModifiedDate).ToListAsync();

            if (favoriteProductsCount.Count > 0)
            {
                favoriteProducts = favoriteProductsCount.Skip((pagerInput.PageIndex - 1) * pagerInput.PageSize).Take(pagerInput.PageSize).ToList();
            }
            return new FavoriteRepoProductList { List = favoriteProducts, TotalCount = favoriteProductsCount.Count };
        }
    }
}
