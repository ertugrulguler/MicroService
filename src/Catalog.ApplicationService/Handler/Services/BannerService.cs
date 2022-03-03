using Catalog.Domain;
using Catalog.Domain.BannerAggregate;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Services
{
    public class BannerService : IBannerService
    {
        private readonly IBannerLocationRepository _bannerLocationRepository;
        private readonly IDbContextHandler _dbContextHandler;

        public BannerService(IBannerLocationRepository bannerLocationRepository, IDbContextHandler dbContextHandler)
        {
            _bannerLocationRepository = bannerLocationRepository;
            _dbContextHandler = dbContextHandler;
        }
        public async Task<bool> CreateOrUpdateBannerLocation(BannerLocation entity, bool isUpdated)
        {
            if (isUpdated) _bannerLocationRepository.Update(entity);
            else await _bannerLocationRepository.SaveAsync(entity);
            await _dbContextHandler.SaveChangesAsync();
            return true;
        }
    }
}
