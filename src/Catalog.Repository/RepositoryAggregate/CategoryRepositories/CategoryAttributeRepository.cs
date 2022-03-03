using Catalog.Domain.CategoryAggregate;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Repository.RepositoryAggregate.CategoryRepositories
{
    public class CategoryAttributeRepository : GenericRepository<CategoryAttribute>, ICategoryAttributeRepository
    {
        public CategoryAttributeRepository(CatalogDbContext context) : base(context)
        {

        }
        public async Task<List<Guid>> GetAttributeByCategoryList(List<Guid> categoryIdList)
        {

            var list = await _entities.AsQueryable()
              .Where(p => categoryIdList.Contains(p.CategoryId) && p.IsActive)
              .GroupBy(y => y.AttributeId).Select(g => new { key = g.Key, value = g.Count() }).Where(t => t.value == categoryIdList.Count).Select(u => u.key)
              .ToListAsync();

            return list;
        }
    }
}