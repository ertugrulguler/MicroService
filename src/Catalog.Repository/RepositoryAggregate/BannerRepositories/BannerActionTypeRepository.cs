using Catalog.Domain.BannerAggregate;

namespace Catalog.Repository.RepositoryAggregate.BannerRepositories
{
    public class BannerActionTypeRepository : GenericRepository<BannerActionType>, IBannerActionTypeRepository
    {
        public BannerActionTypeRepository(CatalogDbContext context) : base(context)
        {

        }
    }
}
