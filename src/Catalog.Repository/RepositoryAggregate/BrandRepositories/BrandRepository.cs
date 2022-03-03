using Catalog.Domain.BrandAggregate;
using Framework.Core.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Repository.RepositoryAggregate.BrandRepositories
{
    public class BrandRepository : GenericRepository<Domain.BrandAggregate.Brand>, IBrandRepository
    {
        public BrandRepository(CatalogDbContext context) : base(context)
        {

        }
        public async Task<List<Brand>> GetBrandsSearchOptimization(DateTime createdDate)
        {
            return await _entities.Where(p => p.ModifiedDate > createdDate && p.IsActive).OrderByDescending(brand => brand.ModifiedDate)
                .ToListAsync();
        }

        public async Task<Dictionary<string, Guid>> GetContainsBrands(List<string> list, Boolean isSeo = false)
        {
            var brandNameDic = new Dictionary<string, Guid>();
            var brandNameList = new List<string>();
            Brand brand;
            brandNameList = list.Distinct().Where(r => !String.IsNullOrEmpty(r.Trim())).Select(y => y.Trim()).ToList();
            foreach (var item in brandNameList)
            {
                string joinedItem = string.Join(" ", item.Split());
                if (isSeo) brand = await _entities.Where(s => s.SeoName == joinedItem && s.IsActive).FirstOrDefaultAsync();
                else brand = await _entities.Where(s => s.Name == joinedItem && s.IsActive).FirstOrDefaultAsync();
                if (brand != null)
                {
                    brandNameDic.Add(brand.Name, brand.Id);
                }
            }
            return brandNameDic;
        }
        public async Task<List<Domain.ValueObject.BrandDto>> GetBrandsForBO(string name, PagerInput pagerInput)
        {
            return await _entities.AsQueryable().AsNoTracking()
              .Where(p => p.Name.StartsWith(name))
              .Skip((pagerInput.PageIndex - 1) * pagerInput.PageSize)
              .Take(pagerInput.PageSize).Select(j => new Domain.ValueObject.BrandDto
              {
                  Id = j.Id,
                  LogoUrl = j.LogoUrl,
                  Name = j.Name,
                  Status = j.IsActive,
                  Website = j.WebSite
              })
              .ToListAsync();
        }
    }
}


