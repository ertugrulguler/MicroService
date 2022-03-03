using Catalog.Domain.ProductAggregate;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Query.ProductQueries
{
    public class GetProductSellerDetailToBannerQuery : IRequest<ResponseBase<ProductSeller>>
    {
        public string Code { get; set; }
        public Guid SellerId { get; set; }
    }
}
