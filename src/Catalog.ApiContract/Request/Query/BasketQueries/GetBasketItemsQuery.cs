using Catalog.ApiContract.Response.Query.BasketQueries;
using Catalog.Domain.ValueObject;
using Framework.Core.Model;
using MediatR;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Query.BasketQueries
{
    public class GetBasketItemsQuery : IRequest<ResponseBase<List<BasketDetail>>>
    {
        public List<RequestBasketInfo> BasketProducts { get; set; }
    }
}
