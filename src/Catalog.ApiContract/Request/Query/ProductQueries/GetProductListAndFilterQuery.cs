using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.Domain.Enums;
using Catalog.Domain.ProductAggregate.ServiceModels;
using Framework.Core.Model;
using MediatR;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Query.ProductQueries
{
    public class GetProductListAndFilterQuery : IRequest<ResponseBase<ProductListAndFilterQueryResult>>
    {
        public string Query { get; set; }
        public List<FilterModel> FilterModel { get; set; }
        public OrderBy OrderBy { get; set; }
        public PagerInput PagerInput { get; set; }
        public bool IsVisibleAllFilters { get; set; } = true;
        public bool IsSellerVisible { get; set; }
        public bool IsSellerSearch { get; set; } = false;
    }
}
