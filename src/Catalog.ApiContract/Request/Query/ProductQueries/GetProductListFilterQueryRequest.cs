using System;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.Domain.Enums;
using Framework.Core.Model;
using MediatR;

namespace Catalog.ApiContract.Request.Query.ProductQueries
{
    public class GetProductListFilterQueryRequest : IRequest<ResponseBase<GetProductListFilterQueryResponse>>
    {
        public ProductFilterType ProductFilterType { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? BrandId { get; set; }
    }
}