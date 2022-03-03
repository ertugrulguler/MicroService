using Catalog.ApiContract.Response.Query.ProductQueries;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Query.ProductQueries
{
    public class GetCategoryTreeSearchOptimizationQuery : IRequest<ResponseBase<GetCategoryTreeSearchOptimizationQueryResult>>
    {
        public DateTime CreatedDate { get; set; }
    }
}
