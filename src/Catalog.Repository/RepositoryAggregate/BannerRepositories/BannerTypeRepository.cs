using Catalog.Domain.BannerAggregate;

namespace Catalog.Repository.RepositoryAggregate.BannerRepositories
{
    public class BannerTypeRepository : GenericRepository<BannerType>, IBannerTypeRepository
    {
        public BannerTypeRepository(CatalogDbContext context) : base(context)
        {

        }
    }
}
