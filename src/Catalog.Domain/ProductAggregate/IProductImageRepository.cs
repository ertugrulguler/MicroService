
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Domain.ProductAggregate
{
    public interface IProductImageRepository : IGenericRepository<ProductImage>
    {
        Task<List<ProductImage>> GetProductImagesByProductIds(List<Guid> productIds);
    }
}
