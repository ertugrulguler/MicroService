using Catalog.ApiContract.Response.Query.ProductQueries;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Query.ProductQueries
{
    public class GetProductSeoUrlQuery : IRequest<ResponseBase<GetProductSeoUrlResponse>>
    {
        public Guid Id { get; set; }
    }
}
