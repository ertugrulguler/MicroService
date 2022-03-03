using Catalog.ApiContract.Contract;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Command.ProductCommands
{
    public class DeleteProductCommand : IRequest<ResponseBase<ProductDto>>
    {
        public Guid Id { get; set; }

    }
}
