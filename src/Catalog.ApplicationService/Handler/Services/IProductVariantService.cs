using Catalog.Domain.ProductAggregate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Services
{
    public interface IProductVariantService
    {
        Task<List<List<VariantGroup>>> GetProductWithVariants(Guid productId, Guid categoryId);
        Task<List<Product>> FilterVariants(List<Product> productList);
        Task<List<Guid>> VariantableProductIds(List<Product> productList);
        Task<List<VariantGroup>> GetProductWithVariantsList(List<Guid> productIdList);
    }
}