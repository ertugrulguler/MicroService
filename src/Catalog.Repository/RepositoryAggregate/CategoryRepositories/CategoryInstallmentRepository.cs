using Catalog.Domain.CategoryAggregate;

namespace Catalog.Repository.RepositoryAggregate.CategoryRepositories
{
    public class CategoryInstallmentRepository : GenericRepository<CategoryInstallment>, ICategoryInstallmentRepository
    {
        public CategoryInstallmentRepository(CatalogDbContext context) : base(context)
        {
        }

    }
}