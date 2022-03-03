using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Domain.BrandAggregate
{
    public class BrandDomainService : IBrandDomainService
    {
        private readonly IBrandRepository _brandRepository;

        public BrandDomainService(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<Dictionary<string, Guid>> GetBrandName(List<string> list, Boolean isSeo)
        {
            var brandList = await _brandRepository.GetContainsBrands(list, isSeo);
            return brandList;
        }
    }

}