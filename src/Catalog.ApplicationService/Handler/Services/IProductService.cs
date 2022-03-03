using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;
using Catalog.Domain.ProductAggregate.ServiceModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Services
{
    public interface IProductService
    {
        Task<ProductListWithCount> GetProductList(GetProductList request, List<Guid> bannedSellers, List<Category> categorySubList);
        ExpressionsModel GetProductListExpressions(GetProductList request, ProductFilterEnum productFilterEnum, List<Category> categorySubList);
        Task<List<FavoriteProductsList>> GetFavoriteProductsForCustomerId(Guid CustomerId);
        Task<ProductListWithCount> GetProductListBySeller(GetProductList request, List<Guid> bannedSeller);
        List<OrderByListofObject> GetOrderList();
        Task<string> GetProductSeoUrl(Guid id);
    }
}

