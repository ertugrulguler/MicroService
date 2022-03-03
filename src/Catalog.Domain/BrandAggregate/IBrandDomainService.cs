using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Domain.BrandAggregate
{
    public interface IBrandDomainService
    {
        Task<Dictionary<string, Guid>> GetBrandName(List<string> list, Boolean isSeo);
    }
}
