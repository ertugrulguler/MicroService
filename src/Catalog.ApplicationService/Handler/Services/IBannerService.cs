using Catalog.Domain.BannerAggregate;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Services
{
    public interface IBannerService
    {
        Task<bool> CreateOrUpdateBannerLocation(BannerLocation entity, bool isUpdated);

    }
}