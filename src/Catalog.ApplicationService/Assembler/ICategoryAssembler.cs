using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Response.Command.CategoryCommands;
using Catalog.ApiContract.Response.Query.CategoryQueries;
using Catalog.Domain.AttributeAggregate;
using Catalog.Domain.CategoryAggregate;
using Framework.Core.Model;
using System;
using System.Collections.Generic;

namespace Catalog.ApplicationService.Assembler
{
    public interface ICategoryAssembler
    {
        ResponseBase<CreateCategory> MapToCrateCategoryCommandResult(Category category);
        ResponseBase<CategoryDto> MapToUpdateCategoryCommandResult(Category category);
        ResponseBase<CategoryAttributeDto> MapToCreateCategoryAttributeCommandResult(CategoryAttribute categoryAttribute);
        ResponseBase<GetCategoryWithAttributes> MapToGetCategoryWithAttributesCodeResult(Category category, List<AttributeMap> attributeMaps,
            List<Domain.AttributeAggregate.Attribute> attributes, List<AttributeValue> attributeValues, List<CategoryAttributeValueMap> categoryAttributeValueMaps, bool onlyRequiredFields);
        ResponseBase<GetCategoryWithAttributes> MapToGetCategoryWithAttributesQueryResult(Category category, bool onlyRequiredFields);
        ResponseBase<List<CategoryTree>> MapToGetCategoryTreeQueryResult(List<Category> categoryList, Dictionary<Guid, List<string>> categoryWithParents);
        ResponseBase<List<CategoryIdAndName>> MapToGetAllCategoriesQueryResult(List<Category> categoryList, Dictionary<Guid, string> leafCategoryList);
        CategoryListForMobileDto MapToGetCategoriesForMobileQueryResult(Category category);

        ResponseBase<List<GetCategoryAndImageBySellerIdResult>> MapToGetCategoryAndImageBySellerIdResult(
            List<GetCategoryAndImageBySellerIdResponse> list);
        CategoryListForSellerDto MapToGetCategoriesForSellerQueryResult(Category category);

        GetCategoriesResult MapToGetCategoriesQueryResult(Category category);

    }
}