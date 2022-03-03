using Catalog.Domain.Enums;
using Catalog.Domain.ProductAggregate;
using Catalog.Domain.ProductAggregate.ServiceModels;
using Framework.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Domain.CategoryAggregate
{
    public class CategoryDomainService : ICategoryDomainService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryAttributeRepository _categoryAttributeRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        public CategoryDomainService(ICategoryRepository categoryRepository,
            ICategoryAttributeRepository categoryAttributeRepository, IProductRepository productRepository, IProductCategoryRepository productCategoryRepository)
        {
            _categoryRepository = categoryRepository;
            _categoryAttributeRepository = categoryAttributeRepository;
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
        }
        public async Task<List<GetCategoryAndImageBySellerIdResponse>> GetCategoryAndImageBySellerId(Guid sellerId)
        {
            var productList = await _productRepository.GetProductBySellerId(sellerId);
            var getCategoryList = await _categoryRepository.GetCategoryAndImageBySellerId(productList
                .SelectMany(s => s.ProductCategories.Select(y => y.CategoryId)).ToList());
            var result = new List<GetCategoryAndImageBySellerIdResponse>();
            getCategoryList.ForEach(c => result.Add(new GetCategoryAndImageBySellerIdResponse
            {
                Description = c.Description,
                Code = c.Code,
                DisplayName = c.DisplayName,
                ImageUrl = getCategoryList?.FirstOrDefault(d => d.CategoryImage.CategoryId == c.Id)
                    ?.CategoryImage.Url,
                Name = c.Name
            }));
            return result;
        }
        public async Task<List<string>> CheckCategoryIsExistOrLeaf(Guid categoryId)
        {
            var errorList = new List<string>();
            var categoryIsExist = await _categoryRepository.Exist(y => y.Id == categoryId);
            if (!categoryIsExist)
            {
                errorList.Add(categoryId + " " + ApplicationMessage.CategoryNotFound.UserMessage());
            }
            else
            {
                var categoryIsNotLeaf = await _categoryRepository.Exist(c => c.ParentId == categoryId);
                if (categoryIsNotLeaf)
                {
                    errorList.Add(categoryId + " " + ApplicationMessage.CategoryIsNotLeaf.UserMessage());
                }
            }
            return errorList;
        }

        public async Task<Dictionary<Guid, int>> GetCategoryAttributeList(Guid? categoryId)
        {
            var result = new Dictionary<Guid, int>();
            var categoryAttribute = await _categoryAttributeRepository.FilterByAsync(u => u.CategoryId == categoryId);

            if (categoryAttribute.Count > 0)
            {
                result = categoryAttribute.GroupBy(u => u.AttributeId, (k, g) => new
                {
                    Key = k,
                    Value = g.Select(y => y.Order).FirstOrDefault()
                }).OrderBy(u => u.Value).ToDictionary(y => y.Key, y => y.Value);
            }

            return result;
        }


        public async Task<List<RelatedCategories>> GetRelatedCategory(Guid categoryId)
        {
            var categoryList = new List<RelatedCategories>();
            var list = await _categoryRepository.FilterByAsync(y => y.ParentId == categoryId);
            if (list.Any())
                list.ForEach(u => categoryList.Add(new RelatedCategories { Id = u.Id, Name = u.Name, HasProduct = u.HasProduct, SeoName = u.SeoName, Code = u.Code }));
            else
            {
                var category = await _categoryRepository.FindByAsync(y => y.Id == categoryId);
                if (category != null)
                    categoryList.Add(new RelatedCategories { Id = category.Id, Name = category.Name, HasProduct = category.HasProduct, Code = category.Code, SeoName = category.SeoName });
            }

            return categoryList;
        }

        public async Task<List<RelatedCategories>> GetRelatedCategoryV2(List<Guid> categoryIdIdList)
        {
            var categoryList = new List<RelatedCategories>();

            foreach (var item in categoryIdIdList)
            {
                var list = await _categoryRepository.FilterByAsync(y => y.ParentId == item);
                if (list.Any())
                    list.ForEach(u => categoryList.Add(new RelatedCategories { Id = u.Id, Name = u.Name, HasProduct = u.HasProduct, Code = u.Code, SeoName = u.SeoName }));
                else
                {
                    var category = await _categoryRepository.FindByAsync(y => y.Id == item);
                    if (category != null)
                        categoryList.Add(new RelatedCategories { Id = category.Id, Name = category.Name, HasProduct = category.HasProduct, Code = category.Code, SeoName = category.SeoName });
                }
            }

            return categoryList;
        }

        public async Task<Dictionary<string, Guid>> GetCategoryName(string name)
        {
            var categoryList = await _categoryRepository.GetContainsCategories(name);
            return categoryList;
        }

        public async Task<List<RelatedCategories>> GetRelatedCategoryforProductList(List<Guid> productList)
        {
            var categoryList = new List<RelatedCategories>();
            if (!productList.Any())
                return new List<RelatedCategories>();

            var category = await _productCategoryRepository.FilterByAsync(y => productList.Contains(y.ProductId));
            if (category == null)
                return new List<RelatedCategories>();

            var list = await _categoryRepository.FilterByAsync(u => category.Select(y => y.CategoryId).Contains(u.Id));
            list.ForEach(u => categoryList.Add(new RelatedCategories { Id = u.Id, Name = u.Name, HasProduct = u.HasProduct, SeoName = u.SeoName, Code = u.Code }));
            return categoryList;
        }

        public async Task<List<Category>> GetSubSuggestedCategories(Guid categoryId)
        {
            return await _categoryRepository.FilterByAsync(c => c.SuggestedMainId == categoryId);
        }


        public async Task<List<Category>> GetLeafCategories(Guid? categoryId, List<Category> leafCategories)
        {
            if (categoryId == null)
                return leafCategories;
            var categories = await _categoryRepository.FilterByAsync(c => c.ParentId == categoryId);
            if (categories.Count == 0)
            {
                var category = await _categoryRepository.FindByAsync(c => c.Id == categoryId);
                if (category != null)
                {
                    leafCategories.Add(category);
                }

                return leafCategories;
            }

            foreach (var category in categories)
            {
                await GetLeafCategories(category.Id, leafCategories);
            }

            return leafCategories;
        }

        public async Task<List<Category>> GetLeafCategoriesV2(Guid? categoryId)
        {
            if (categoryId == null)
                return new List<Category>();
            var leafCategories = await _categoryRepository.FilterByAsync(c => c.LeafPath.Contains(categoryId.ToString()) && c.HasProduct);
            if (leafCategories.Any())
                return leafCategories;
            else
            {
                return await _categoryRepository.FilterByAsync(c => c.Id == categoryId);
            }
        }

        public async Task<List<Category>> GetLeafCategoriesForCalculateHasProduct(Guid? categoryId)
        {
            if (categoryId == null)
                return new List<Category>();
            var leafCategories = await _categoryRepository.FilterByAsync(c => c.LeafPath.Contains(categoryId.ToString()));
            if (leafCategories.Any())
                return leafCategories;
            else
            {
                return await _categoryRepository.FilterByAsync(c => c.Id == categoryId);
            }
        }

        public async Task<List<Category>> GetLeafCategoriesBySeller(Guid sellerId, List<Category> leafCategories)
        {
            var products = await _productRepository.GetProductBySellerId(sellerId);
            var categoryList = products.SelectMany(x => x.ProductCategories.Select(p => p.CategoryId)).Distinct().ToList();
            if (categoryList.Count == 0)
            {
                foreach (var item in categoryList)
                {
                    var category = await _categoryRepository.FindByAsync(c => c.Id == item);
                    if (category != null)
                    {
                        leafCategories.Add(category);
                    }
                }
                return leafCategories;
            }
            foreach (var category in categoryList)
            {
                await GetLeafCategories(category, leafCategories);
            }

            return leafCategories;
        }

        public async Task<Category> GetMainCategory(Guid categoryId)
        {
            while (true)
            {
                var category = await _categoryRepository.FindByAsync(c => c.Id == categoryId);
                if (category.ParentId == null)
                {
                    return category;
                }
            }
        }

        public async Task<bool> CheckSuitableForSuggestedCategory(Guid? parentId)
        {
            if (parentId == null)
            {
                throw new BusinessRuleException(ApplicationMessage.MainCategoryCannotBeSuggested,
                    ApplicationMessage.MainCategoryCannotBeSuggested.Message(),
                    ApplicationMessage.MainCategoryCannotBeSuggested.UserMessage());
            }
            else
            {
                var parentCategory = await _categoryRepository.FindByAsync(c => c.Id == parentId);
                if (parentCategory.ParentId == null)
                {
                    throw new BusinessRuleException(ApplicationMessage.MainCategoryCannotBeSuggested,
                        ApplicationMessage.MainCategoryCannotBeSuggested.Message(),
                        ApplicationMessage.MainCategoryCannotBeSuggested.UserMessage());
                }

                return true;
            }
        }

        public async Task<List<Category>> GetCategoryTree()
        {
            var allCategoryList = await _categoryRepository.FilterByAsync(x => x.Type == CategoryTypeEnum.MainCategory);
            var mainCategories = allCategoryList.Where(x => x.ParentId == null);

            foreach (var mainCategory in mainCategories)
            {
                mainCategory.SubCategories.AddRange(await SubCategorie(mainCategory.Id, allCategoryList));
            }

            return mainCategories.ToList();
        }


        private async Task<List<Category>> SubCategorie(Guid mainId, List<Category> allCategories)
        {
            var subCategories = allCategories.Where(x => x.ParentId == mainId);

            foreach (var subCategory in subCategories)
            {
                subCategory.SubCategories.AddRange(await SubCategorie(subCategory.Id, allCategories));
            }

            return subCategories.ToList();
        }
        public async Task<List<Category>> GetLeafCategoriesV3(List<Guid> categoryIdIdList)
        {
            var list = new List<Category>();

            if (categoryIdIdList.Count == 0)
                return new List<Category>();
            else
            {
                foreach (var item in categoryIdIdList)
                {
                    var leafCategories = await _categoryRepository.FilterByAsync(c => c.LeafPath.Contains(item.ToString()) && c.HasProduct);
                    if (leafCategories.Any())
                    {
                        leafCategories.ForEach(p => list.Add(p));
                        continue;
                    }
                    else
                    {
                        list.Add(await _categoryRepository.FindByAsync(c => c.Id == item));
                    }
                }
            }
            return list;
        }

    }
}