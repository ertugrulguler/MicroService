using Catalog.ApiContract.Response.Query.ProductQueries;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Query.ProductQueries
{
    public class GetSellerProductCountDetailsQuery : IRequest<ResponseBase<GetSellerProductCountsDetail>>
    {
        public Guid SellerId { get; set; }
    }
}
