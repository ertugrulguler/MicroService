using System;
using System.Threading.Tasks;

namespace Catalog.Domain.ProductAggregate
{
    public interface IProductChannelRepository : IGenericRepository<ProductChannel>
    {
        Task<ProductChannel> GetProductChannelAll(Guid productId);


    }
}
