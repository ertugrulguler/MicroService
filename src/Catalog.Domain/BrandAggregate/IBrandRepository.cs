using Framework.Core.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Domain.BrandAggregate
{
    public interface IBrandRepository : IGenericRepository<Brand>
    {
        Task<List<Brand>> GetBrandsSearchOptimization(DateTime createdDate);
        Task<Dictionary<string, Guid>> GetContainsBrands(List<string> list, Boolean isSeo = false);
        Task<List<Domain.ValueObject.BrandDto>> GetBrandsForBO(string name, PagerInput pagerInput);
    }

}