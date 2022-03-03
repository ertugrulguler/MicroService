using Catalog.Domain.CategoryAggregate;

namespace Catalog.Repository.RepositoryAggregate.CategoryRepositories
{
    public class CategoryImageRepository : GenericRepository<CategoryImage>, ICategoryImageRepository
    {
        public CategoryImageRepository(CatalogDbContext context) : base(context)
        {
        }
    }
}
