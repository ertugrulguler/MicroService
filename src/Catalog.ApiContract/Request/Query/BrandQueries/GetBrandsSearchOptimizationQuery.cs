using Catalog.ApiContract.Response.Query.BrandQueries;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Query.BrandQueries
{
    public class GetBrandsSearchOptimizationQuery : IRequest<ResponseBase<GetBrandsSearchOptimizationQueryResult>>
    {
        public DateTime CreatedDate { get; set; }
    }
}
