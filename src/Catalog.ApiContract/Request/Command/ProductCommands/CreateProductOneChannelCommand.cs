using Catalog.Domain.Enums;
using Framework.Core.Model;
using MediatR;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Command.ProductCommands
{
    public class CreateProductOneChannelCommand : IRequest<ResponseBase<object>>
    {
        public string ProductCode { get; set; }
        public ProductChannelCode ProductChannelCode { get; set; }
    }
}