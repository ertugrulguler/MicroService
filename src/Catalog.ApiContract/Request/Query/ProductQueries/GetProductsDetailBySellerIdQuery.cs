using Catalog.ApiContract.Response.Query.ProductQueries;
using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Query.ProductQueries
{
    public class GetProductsDetailBySellerIdQuery : IRequest<ResponseBase<List<SellerProducts>>>
    {
        public Guid SellerId { get; set; }
        public string Code { get; set; }
        public int Size { get; set; }
        public int Page { get; set; }
    }
}
