using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.Domain;
using Catalog.Domain.AttributeAggregate;
using Catalog.Domain.BrandAggregate;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.ProductAggregate;
using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.ProductQueries
{
    public class
        GetProductRelationControlQueryHandler : IRequestHandler<GetProductRelationControlQuery,
            ResponseBase<List<string>>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryDomainService _categoryDomainService;
        private readonly ICategoryAttributeRepository _categoryAttributeRepository;
        private readonly IAttributeRepository _attributeRepository;
        private readonly IAttributeValueRepository _attributeValueRepository;
        private readonly IAttributeMapRepository _attributeMapRepository;
        private readonly IProductRepository _productRepository;

        public GetProductRelationControlQueryHandler(IBrandRepository brandRepository,
            ICategoryRepository categoryRepository, ICategoryDomainService categoryDomainService,
            ICategoryAttributeRepository categoryAttributeRepository, IAttributeRepository attributeRepository,
            IAttributeValueRepository attributeValueRepository,
            IAttributeMapRepository attributeMapRepository,
            IProductRepository productRepository)
        {
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
            _categoryDomainService = categoryDomainService;
            _categoryAttributeRepository = categoryAttributeRepository;
            _attributeRepository = attributeRepository;
            _attributeValueRepository = attributeValueRepository;
            _attributeMapRepository = attributeMapRepository;
            _productRepository = productRepository;
        }

        public async Task<ResponseBase<List<string>>> Handle(GetProductRelationControlQuery request,
            CancellationToken cancellationToken)
        {
            var errorList = new List<string>();
            var attributeValues = new List<AttributeValue>();
            var attributeMapValues = new List<AttributeMap>();
            var category = await _categoryRepository.FindByAsync(g => g.IsActive && g.Code != null && g.Id == request.CategoryId &&
                          g.Type == Domain.Enums.CategoryTypeEnum.MainCategory);
            if (category == null)
            {
                errorList.Add(request.CategoryId + " " + ApplicationMessage.CategoryNotActive.UserMessage());
            }
            if (!string.IsNullOrEmpty(request.BarcodeNo))
            {
                var productInfo = await _productRepository.GetProductWithCategoryByCode(request.BarcodeNo);
                if (productInfo != null)
                {
                    var productCategories = productInfo.ProductCategories.Select(x => x.CategoryId).ToList();
                    var productCategoryList = await _categoryRepository.FilterByAsync(x => productCategories.Contains(x.Id) && x.Type == Domain.Enums.CategoryTypeEnum.MainCategory);
                    var productCategoryCount = productCategoryList.Where(x => x.Id != request.CategoryId).Count();
                    if (productCategoryCount > 0)
                    {
                        var categoryName = await GetLeafPathCategoryAsync(productCategoryList.FirstOrDefault());
                        errorList.Add(request.BarcodeNo + ApplicationMessage.ProductAlreadyCreatedDifferentCategory.UserMessage() + categoryName);
                    }
                }

            }
            var brandIsExist = await _brandRepository.Exist(b => b.Id == request.BrandId);
            if (!brandIsExist)
            {
                errorList.Add(request.BrandId + " " + ApplicationMessage.BrandNotFound.UserMessage());
            }

            errorList.AddRange(await _categoryDomainService.CheckCategoryIsExistOrLeaf(request.CategoryId));

            var categoryRequiredAttributes =
                await _categoryAttributeRepository.FilterByAsync(y => y.CategoryId == request.CategoryId);

            var attributeList = request.CategoryAttributeValue.Select(y => y.AttributeId).ToList();
            var categoryRequiredAttributesIds = categoryRequiredAttributes.Where(t => t.IsRequired).ToList();
            var categoryNotRequiredAttributesIds = categoryRequiredAttributes.Where(t => !t.IsRequired).ToList();

            foreach (var item in categoryRequiredAttributesIds)
            {
                if (!attributeList.Contains(item.AttributeId))
                {
                    errorList.Add(item.CategoryId.ToString() + " Id'li category 'e ait zorunlu " +
                                  item.AttributeId.ToString() + " " +
                                  ApplicationMessage.AttributeNotFound.UserMessage());
                }
            }

            foreach (var attributeId in attributeList)
            {
                if (!categoryNotRequiredAttributesIds.Select(a => a.AttributeId).Contains(attributeId) &&
                    !categoryRequiredAttributesIds.Select(a => a.AttributeId).Contains(attributeId))
                {
                    errorList.Add(attributeId.ToString() + " " + ApplicationMessage.AttributeNotFound.UserMessage());
                }
            }

            var duplicateList = attributeList.GroupBy(x => x).Where(g => g.Count() > 1).Select(y => y.Key).ToList();
            if (duplicateList.Any())
                duplicateList.ForEach(t =>
                    errorList.Add(t.ToString() + " " + ApplicationMessage.AttributeIdAlreadyExist.UserMessage()));

            var cat = await _categoryRepository.FindByAsync(g => g.Id == request.CategoryId);
            if (string.IsNullOrEmpty(cat.Code))
            {
                attributeValues =
        await _attributeValueRepository.FilterByAsync(x => attributeList.Contains(x.AttributeId.Value));
            }
            else if (!string.IsNullOrEmpty(cat.Code))
            {
                attributeMapValues =
        await _attributeMapRepository.FilterByAsync(x => attributeList.Contains(x.AttributeId));
            }
            foreach (var item in request.CategoryAttributeValue)
            {
                //TODO ÖZLEM IF KISMI SİLİNECEK!!
                if (attributeValues.Any() && !attributeValues.Where(y => y.AttributeId == item.AttributeId)
                    .Any(t => t.Id == item.AttributeValueId))
                    errorList.Add(item.AttributeId.ToString() + " - " + item.AttributeValueId.ToString() +
                                  ApplicationMessage.AttributeAndAttributeValueIdNotFound.UserMessage());

                else if (attributeMapValues.Any() && (!attributeMapValues.Where(y => y.AttributeId == item.AttributeId)
                   .Any(t => t.AttributeValueId == item.AttributeValueId))
                    )
                    errorList.Add(item.AttributeId.ToString() + " - " + item.AttributeValueId.ToString() +
                                  ApplicationMessage.AttributeAndAttributeValueIdNotFound.UserMessage());
            }
            return new ResponseBase<List<string>>() { Data = errorList, Success = errorList.Any() ? false : true };
        }
        private async Task<string> GetLeafPathCategoryAsync(Category category)
        {
            if (category != null)
            {
                var categoryName = category.DisplayName;
                if (!string.IsNullOrEmpty(category.LeafPath))
                {
                    categoryName = "->" + category.DisplayName;
                    var splitCategory = category.LeafPath.Split(',');

                    foreach (var leafItem in splitCategory)
                    {
                        var categoryId = new Guid(leafItem);
                        var categoryItem = await _categoryRepository.GetByIdAsync(categoryId);
                        categoryName = "->" + categoryItem.DisplayName + categoryName;
                    }
                }
                return categoryName;
            }
            else
                return string.Empty;
        }
    }
}