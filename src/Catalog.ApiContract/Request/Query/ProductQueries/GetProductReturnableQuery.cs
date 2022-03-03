using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Query.ProductQueries
{
    public class GetProductReturnableQuery : IRequest<ResponseBase<bool>>
    {
        public Guid Id { get; set; }
    }
}
