using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Domain.CategoryAggregate
{
    public interface ICategoryAttributeRepository : IGenericRepository<CategoryAttribute>
    {
        Task<List<Guid>> GetAttributeByCategoryList(List<Guid> categoryIdList);
    }
}