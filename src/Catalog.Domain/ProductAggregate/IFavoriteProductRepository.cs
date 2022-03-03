
using Catalog.Domain.ProductAggregate.ServiceModels;
using Framework.Core.Model;
using System;
using System.Threading.Tasks;

namespace Catalog.Domain.ProductAggregate
{
    public interface IFavoriteProductRepository : IGenericRepository<FavoriteProduct>
    {
        Task<FavoriteRepoProductList> GetFavoriteProductsByCustomerId(Guid CustomerId, PagerInput pagerInput);
    }
}
