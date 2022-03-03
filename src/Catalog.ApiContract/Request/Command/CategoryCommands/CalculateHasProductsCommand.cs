using Framework.Core.Model;
using MediatR;

namespace Catalog.ApiContract.Request.Command.CategoryCommands
{
    public class CalculateHasProductsCommand : IRequest<ResponseBase<object>>
    {
    }
}
