using Catalog.Domain.BannerAggregate;

namespace Catalog.Repository.RepositoryAggregate.BannerRepositories
{
    public class BannerRepository : GenericRepository<Banner>, IBannerRepository
    {
        public BannerRepository(CatalogDbContext context) : base(context)
        {

        }
    }
}
