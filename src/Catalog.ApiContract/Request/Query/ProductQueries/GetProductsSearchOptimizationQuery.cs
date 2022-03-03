using Catalog.ApiContract.Response.Query.ProductQueries;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Query.ProductQueries
{
    public class GetProductsSearchOptimizationQuery : IRequest<ResponseBase<GetProductsSearchOptimizationQueryResult>>
    {
        public DateTime CreatedDate { get; set; }
    }
}
