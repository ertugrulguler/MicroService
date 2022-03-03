using Catalog.Domain.CategoryAggregate;

namespace Catalog.Repository.RepositoryAggregate.CategoryRepositories
{
    public class CategoryAttributeValueMapRepository : GenericRepository<CategoryAttributeValueMap>, ICategoryAttributeValueMapRepository
    {
        public CategoryAttributeValueMapRepository(CatalogDbContext context) : base(context)
        {
        }
    }
}
