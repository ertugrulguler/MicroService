using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Domain.ProductAggregate
{
    public interface IProductSellerRepository : IGenericRepository<ProductSeller>
    {
        Task<List<Guid>> GetProductIdsByProductSellerIds(List<Guid> productSellerIds);
    }
}
