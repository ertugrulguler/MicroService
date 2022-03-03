using Catalog.Domain.BannerAggregate;

namespace Catalog.Repository.RepositoryAggregate.BannerRepositories
{
    public class BannerFiltersRepository : GenericRepository<BannerFilters>, IBannerFiltersRepository
    {
        public BannerFiltersRepository(CatalogDbContext context) : base(context)
        {

        }
    }
}
