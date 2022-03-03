using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.Domain.Enums;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Query.ProductQueries
{
    public class GetProductListForSellerQuery : IRequest<ResponseBase<GetProductListForSellerQueryResult>>
    {
        public Guid SellerId { get; set; }
        public string Code { get; set; }
        public string GroupCode { get; set; }
        public string BrandName { get; set; }
        public string ProductName { get; set; }
        public int? Size { get; set; }
        public int? Page { get; set; }
        public OrderByDate OrderByDate { get; set; }
        public ProductFilterForSeller State { get; set; }

    }
}
