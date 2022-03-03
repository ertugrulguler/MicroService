using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.Domain.Enums;
using Catalog.Domain.ProductAggregate.ServiceModels;
using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Query.ProductQueries
{
    public class GetProductListAndFilterBySellerQuery : IRequest<ResponseBase<GetProductListAndFilterBySellerQueryResult>>
    {
        public List<FilterModel> FilterModel { get; set; }
        public Guid SellerId { get; set; }
        public OrderBy OrderBy { get; set; }
        public PagerInput PagerInput { get; set; }
        public bool IsVisibleAllFilters { get; set; } = true;
    }
}
