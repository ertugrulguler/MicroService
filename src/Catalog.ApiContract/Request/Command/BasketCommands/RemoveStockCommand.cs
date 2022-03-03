using Catalog.Domain.ValueObject;
using Framework.Core.Model;
using MediatR;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Command.BasketCommands
{
    public class RemoveStockCommand : IRequest<ResponseBase<object>>
    {
        public List<RequestBasketInfo> BasketProducts { get; set; }
    }
}
