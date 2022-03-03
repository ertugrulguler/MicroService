using Catalog.Domain.AttributeAggregate;

namespace Catalog.Repository.RepositoryAggregate.AttributeRepositories
{
    public class AttributeMapRepository : GenericRepository<Domain.AttributeAggregate.AttributeMap>, IAttributeMapRepository
    {
        public AttributeMapRepository(CatalogDbContext context) : base(context)
        {
        }

    }
}