using Catalog.Domain.ProductAggregate.ServiceModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Domain.CategoryAggregate
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<List<Category>> AllCategoryListWithRelations();
        Task<Category> GetCategoryWithAllRelations(Guid id);
        Task<List<Category>> GetCategoryAndImageBySellerId(List<Guid> categoryIds);
        Task<Category> GetCategoryWithAttributeValues(Guid id);
        Task<List<Category>> GetCategorySearchOptimization(DateTime createdDate);
        Task<Category> GetCategoryWithAttributeMap(Guid id);
        Task<List<Guid>> NotExistingCategories(List<Guid> categoryIdList);
        Task<List<RelatedCategories>> GetProductCategoryFilter(List<Guid> categoryId,
            List<Guid> sellerIdList, List<List<Guid>> attributeIds, List<string> salePriceList, List<Guid> brandIdList,
            List<string> codeList,
            List<Guid> searchList, List<Guid> bannedSellers, int productChannel,
            List<Guid> sellerList);
        Task<Dictionary<string, Guid>> GetContainsCategories(string name);
        Task<List<string>> GetCategoryNamesByCategoryId(List<Guid> categoryIds);
    }
}