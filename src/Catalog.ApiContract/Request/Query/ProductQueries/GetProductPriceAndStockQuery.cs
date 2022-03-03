using Catalog.ApiContract.Response.Query.ProductQueries;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Query.ProductQueries
{
    public class GetProductPriceAndStockQuery : IRequest<ResponseBase<GetProductPriceAndStockQueryResult>>
    {
        public string Code { get; set; }
        public Guid SellerId { get; set; }
    }
}