using Catalog.ApiContract.Response.Query.ProductQueries;
using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Query.ProductQueries
{
    public class GetProductDeliveryListQuery : IRequest<ResponseBase<GetProductDelivery>>
    {
        public List<GetProductDeliveryInfo> GetProductDeliveryInfos { get; set; }
    }

    public class GetProductDeliveryInfo
    {
        public Guid SellerId { get; set; }
        public Guid ProductId { get; set; }
    }
}
