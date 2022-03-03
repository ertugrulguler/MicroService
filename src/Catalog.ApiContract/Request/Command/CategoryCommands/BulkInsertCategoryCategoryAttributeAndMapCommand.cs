using Framework.Core.Model;

using MediatR;

using Microsoft.AspNetCore.Http;

namespace Catalog.ApiContract.Request.Command.CategoryCommands
{
    public class BulkInsertCategoryCategoryAttributeAndMapCommand : IRequest<ResponseBase<object>>
    {
        public IFormFile File { get; set; }

    }
}
