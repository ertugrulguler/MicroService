using Catalog.Domain.BannerAggregate;

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Repository.RepositoryAggregate.BannerRepositories
{
    public class BannerLocationRepository : GenericRepository<BannerLocation>, IBannerLocationRepository
    {
        public BannerLocationRepository(CatalogDbContext context) : base(context)
        {

        }
        public async Task<List<BannerLocation>> GetBannerDetailList()
        {
            return await _entities.AsQueryable()
                .Include(c => c.Banners.Where(t => t.IsActive))
                .Where(p => p.IsActive)
                .ToListAsync();
        }
    }
}
