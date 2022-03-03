using Catalog.ApiContract.Response.Command.CategoryAttributeCommands;
using Catalog.Domain.CategoryAggregate;
using Framework.Core.Model;

namespace Catalog.ApplicationService.Assembler
{
    public class CategoryAttributeAssembler : ICategoryAttributeAssembler
    {

        public ResponseBase<CreateCategoryAttributeResult> MapToCreateCategoryAttributeCommandResult(CategoryAttribute catAttribute)
        {
            var categoryAttribute = new Catalog.ApiContract.Response.Command.CategoryAttributeCommands.CreateCategoryAttributeResult
            {
                Id = catAttribute.Id,
                IsListed = catAttribute.IsListed,
                AttributeId = catAttribute.AttributeId,
                CategoryId = catAttribute.CategoryId,
                IsRequired = catAttribute.IsRequired,
                IsVariantable = catAttribute.IsVariantable,
            };
            return new ResponseBase<CreateCategoryAttributeResult>()
            {
                Data = categoryAttribute,
                Success = true
            };
        }
    }

}

