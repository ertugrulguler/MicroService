using Catalog.ApiContract.Response.Command.CategoryCommands;
using Framework.Core.Model;
using MediatR;

namespace Catalog.ApiContract.Request.Command.CategoryCommands
{
    public class CreateVirtualCategoryCommand : IRequest<ResponseBase<CreateVirtualCategoryResult>>
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int DisplayOrder { get; set; }
        public string Description { get; set; }
    }
}
