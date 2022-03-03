using Catalog.ApiContract.Response.Query.ProductQueries;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Query.ProductQueries
{
    public class GetProductVariantsQuery : IRequest<ResponseBase<GetProductVariantsResponse>>
    {
        public Guid SellerId { get; set; }
        public Guid ProductId { get; set; }
    }
}
