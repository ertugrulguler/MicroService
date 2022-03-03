using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Response.Command.CategoryCommands;
using Catalog.ApiContract.Response.Query.CategoryQueries;
using Catalog.Domain.AttributeAggregate;
using Catalog.Domain.CategoryAggregate;
using Framework.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using GetCategoryAndImageBySellerIdResult =
    Catalog.ApiContract.Response.Query.CategoryQueries.GetCategoryAndImageBySellerIdResult;

namespace Catalog.ApplicationService.Assembler
{
    public class CategoryAssembler : ICategoryAssembler
    {
        public ResponseBase<CreateCategory> MapToCrateCategoryCommandResult(Category category)
        {
            var createCategory = new CreateCategory()
            {
                ParentId = category.ParentId,
                Name = category.Name,
                DisplayName = category.DisplayName,
                Code = category.Code,
                DisplayOrder = category.DisplayOrder,
                Description = category.Description,
                CategoryImage = category.CategoryImage
            };
            return new ResponseBase<CreateCategory>()
            {
                Data = createCategory
            };
        }

        public ResponseBase<CategoryDto> MapToUpdateCategoryCommandResult(Category category)
        {
            return new()
            {
                Data = new CategoryDto
                {
                    ParentId = category.ParentId,
                    Name = category.Name,
                    DisplayName = category.DisplayName,
                    Code = category.Code,
                    DisplayOrder = category.DisplayOrder,
                    Description = category.Description,
                    Attributes = category.CategoryAttributes.Select(c => new AttributeDto()
                    {
                        Name = c.Attribute.Name,
                        //Value = c.Attribute.Value,
                        //IsRequired = c.Attribute.IsRequired
                    }).ToList()
                }
            };
        }

        public ResponseBase<CategoryAttributeDto> MapToCreateCategoryAttributeCommandResult(
            CategoryAttribute categoryAttribute)
        {
            return new()
            {
                Data = new CategoryAttributeDto
                {
                    AttributeId = categoryAttribute.AttributeId,
                    CategoryId = categoryAttribute.CategoryId,
                    IsRequired = categoryAttribute.IsRequired
                }
            };
        }

        public ResponseBase<GetCategoryWithAttributes> MapToGetCategoryWithAttributesCodeResult(Category category, List<AttributeMap> attributeMaps,
            List<Domain.AttributeAggregate.Attribute> attributes, List<AttributeValue> attributeValues, List<CategoryAttributeValueMap> categoryAttributeValueMaps, bool onlyRequiredFields)
        {
            //TODO: Refactor
            var attributeQueryResult = new List<AttributeQueryResult>();

            foreach (var item in category.CategoryAttributes)
            {
                var attributeMap = attributeMaps.Where(x => x.AttributeId == item.AttributeId);
                var attributeValueMap = attributeMap.Where(x => categoryAttributeValueMaps.Where(cav => cav.CategoryAttributeId == item.Id).Select(x => x.AttributeValueId).Contains(x.AttributeValueId));

                attributeQueryResult.Add(new AttributeQueryResult
                {
                    Id = item.AttributeId,
                    Name = attributes.FirstOrDefault(x => x.Id == item.AttributeId)?.Name,
                    DisplayName = attributes.FirstOrDefault(x => x.Id == item.AttributeId)?.DisplayName,
                    IsVariantable = item.IsVariantable,
                    IsRequired = item.IsRequired,
                    Description = attributes.FirstOrDefault(x => x.Id == item.AttributeId)?.Description,
                    AttributeValues = attributeValues.Where(x => attributeValueMap.Select(k => k.AttributeValueId).Contains(x.Id))
                    .Select(v => new AttributeValueQueryResult
                    {
                        Value = v.Value,
                        Id = v.Id
                    }).ToList()
                });
            }
            var getCategoryWithAttributes = new GetCategoryWithAttributes
            {
                Id = category.Id,
                Name = category.Name,
                DisplayName = category.DisplayName,
                Attributes = attributeQueryResult
            };

            return new ResponseBase<GetCategoryWithAttributes>() { Data = getCategoryWithAttributes, Success = true };
        }

        public ResponseBase<GetCategoryWithAttributes> MapToGetCategoryWithAttributesQueryResult(Category category, bool onlyRequiredFields)
        {
            var attributes = new List<AttributeQueryResult>();
            if (onlyRequiredFields)
            {
                attributes = category.CategoryAttributes.Where(f => f.IsRequired).Select(c => new AttributeQueryResult()
                {
                    Id = c.AttributeId,
                    Name = c.Attribute.Name,
                    DisplayName = c.Attribute.DisplayName,
                    IsVariantable = c.IsVariantable,
                    IsRequired = c.IsRequired,
                    Description = c.Attribute.Description,
                    AttributeValues = c.Attribute.AttributeValues.Select(v => new AttributeValueQueryResult()
                    {
                        Value = v.Value,
                        Id = v.Id
                    }).ToList()
                }).ToList();
            }
            else
            {
                attributes = category.CategoryAttributes.Select(c => new AttributeQueryResult()
                {
                    Id = c.AttributeId,
                    Name = c.Attribute.Name,
                    DisplayName = c.Attribute.DisplayName,
                    IsVariantable = c.IsVariantable,
                    IsRequired = c.IsRequired,
                    Description = c.Attribute.Description,
                    AttributeValues = c.Attribute.AttributeValues.Select(v => new AttributeValueQueryResult()
                    {
                        Value = v.Value,
                        Id = v.Id
                    }).ToList()
                }).ToList().OrderByDescending(g => g.IsRequired).ToList();

            }
            var getCategoryWithAttributes = new GetCategoryWithAttributes
            {
                Id = category.Id,
                Name = category.Name,
                DisplayName = category.DisplayName,
                Attributes = attributes
            };

            return new ResponseBase<GetCategoryWithAttributes>() { Data = getCategoryWithAttributes, Success = true };
        }

        public ResponseBase<List<CategoryTree>> MapToGetCategoryTreeQueryResult(List<Category> categoryList,
            Dictionary<Guid, List<string>> categoryWithParents)
        {
            var resultList = new List<CategoryTree>();
            foreach (var category in categoryList)
            {
                resultList.Add(new CategoryTree
                {
                    Id = category.Id,
                    Name = category.Name,
                    DisplayName = category.DisplayName,
                    Description = category.Description,
                    DisplayOrder = category.DisplayOrder,
                    Leaf = categoryList.Where(c => c.ParentId == category.Id).Count() == 0,
                    ParentCategories = categoryWithParents[category.Id],
                    ParentId = category.ParentId,
                });
            }

            return new ResponseBase<List<CategoryTree>>() { Data = resultList, Success = true };
        }

        public ResponseBase<List<CategoryIdAndName>> MapToGetAllCategoriesQueryResult(List<Category> categoryList, Dictionary<Guid, string> leafCategoryList)
        {
            return new()
            {
                Data = categoryList.Select(x => new CategoryIdAndName
                {
                    Id = x.Id,
                    Name = x.Name,
                    LeafPath = x.LeafPath,
                    LeafPathName = leafCategoryList.Count() > 0 ? leafCategoryList[x.Id] : string.Empty
                }).ToList(),
                Success = true

            };
        }

        public ResponseBase<List<GetCategoryAndImageBySellerIdResult>> MapToGetCategoryAndImageBySellerIdResult(
            List<GetCategoryAndImageBySellerIdResponse> list)
        {
            var getCategoryAndImageBySellerIdResult = new List<GetCategoryAndImageBySellerIdResult>();
            list.ForEach(c => getCategoryAndImageBySellerIdResult.Add(new GetCategoryAndImageBySellerIdResult
            {
                Description = c.Description,
                Code = c.Code,
                DisplayName = c.DisplayName,
                ImageUrl = c.ImageUrl,
                Name = c.Name
            }));

            return new()
            {
                Data = getCategoryAndImageBySellerIdResult,
                Success = true
            };
        }

        public CategoryListForMobileDto MapToGetCategoriesForMobileQueryResult(Category category)
        {
            return new CategoryListForMobileDto
            {
                Id = category.Id,
                ParentId = category.ParentId,
                Name = category.DisplayName,
                ImageUrl = category.CategoryImage == null ? "" : category.CategoryImage.Url,
                Url = category.SeoName + "-k-" + category.Code

            };
        }

        public CategoryListForSellerDto MapToGetCategoriesForSellerQueryResult(Category category)
        {
            return new CategoryListForSellerDto
            {
                Id = category.Id,
                ParentId = category.ParentId,
                Name = category.DisplayName,
                ImageUrl = category.CategoryImage == null ? "" : category.CategoryImage.Url,
            };
        }

        public GetCategoriesResult MapToGetCategoriesQueryResult(Category category)
        {
            return new GetCategoriesResult
            {
                Id = category.Id,
                ParentId = category.ParentId,
                Name = category.DisplayName,
                ImageUrl = category.CategoryImage == null ? "" : category.CategoryImage.Url,
            };
        }
    }
}