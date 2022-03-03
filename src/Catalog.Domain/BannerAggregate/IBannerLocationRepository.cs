
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Domain.BannerAggregate
{
    public interface IBannerLocationRepository : IGenericRepository<BannerLocation>
    {
        Task<List<BannerLocation>> GetBannerDetailList();
    }
}
