using Framework.Core.Model;
using MediatR;

namespace Catalog.ApiContract.Request.Command.CategoryCommands
{
    public class CalculateLeafPathsCommand : IRequest<ResponseBase<object>>
    {
    }
}
