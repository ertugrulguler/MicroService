using Catalog.Domain.BannerAggregate;

namespace Catalog.Repository.RepositoryAggregate.BannerRepositories
{
    public class BannerFilterTypeRepository : GenericRepository<BannerFilterType>, IBannerFilterTypeRepository
    {
        public BannerFilterTypeRepository(CatalogDbContext context) : base(context)
        {

        }
    }
}
