using Catalog.Domain.ProductAggregate.ServiceModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Domain.CategoryAggregate
{
    public interface ICategoryDomainService
    {
        Task<List<GetCategoryAndImageBySellerIdResponse>> GetCategoryAndImageBySellerId(Guid sellerId);
        Task<List<string>> CheckCategoryIsExistOrLeaf(Guid categoryIdList);
        Task<List<RelatedCategories>> GetRelatedCategory(Guid CategoryId);
        Task<List<RelatedCategories>> GetRelatedCategoryforProductList(List<Guid> productList);
        Task<Dictionary<Guid, int>> GetCategoryAttributeList(Guid? CategoryId);
        Task<List<Category>> GetSubSuggestedCategories(Guid categoryId);
        Task<List<Category>> GetLeafCategories(Guid? categoryId, List<Category> leafCategories);
        Task<List<Category>> GetLeafCategoriesV2(Guid? categoryId);
        Task<List<Category>> GetLeafCategoriesForCalculateHasProduct(Guid? categoryId);
        Task<Category> GetMainCategory(Guid categoryId);
        Task<bool> CheckSuitableForSuggestedCategory(Guid? parentId);
        Task<List<Category>> GetLeafCategoriesBySeller(Guid sellerId, List<Category> leafCategories);
        Task<List<Category>> GetCategoryTree();
        Task<List<Category>> GetLeafCategoriesV3(List<Guid> categoryIdIdList);
        Task<List<RelatedCategories>> GetRelatedCategoryV2(List<Guid> categoryIdIdList);
        Task<Dictionary<string, Guid>> GetCategoryName(string name);
    }
}