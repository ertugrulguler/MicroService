using Catalog.ApiContract.Response.Query.ProductQueries;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Query.ProductQueries
{
    public class GetProductDetailQuery : IRequest<ResponseBase<GetProductDetailResponse>>
    {
        public Guid SellerId { get; set; }
        public Guid ProductId { get; set; }
        public string Url { get; set; }

    }
}
