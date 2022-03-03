using Catalog.ApiContract.Response.Command.AttributeCommands;
using Framework.Core.Model;
using MediatR;

namespace Catalog.ApiContract.Request.Command.AttributeCommands
{
    public class CreateAttributeCommand : IRequest<ResponseBase<CreateAttributeResult>>
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }
}
