using Catalog.ApiContract.Response.Command.CategoryAttributeCommands;
using Catalog.Domain.CategoryAggregate;
using Framework.Core.Model;

namespace Catalog.ApplicationService.Assembler
{
    public interface ICategoryAttributeAssembler
    {
        ResponseBase<CreateCategoryAttributeResult> MapToCreateCategoryAttributeCommandResult(CategoryAttribute catAttribute);
    }
}
