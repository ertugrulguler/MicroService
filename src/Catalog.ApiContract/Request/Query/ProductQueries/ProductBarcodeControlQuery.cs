using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Query.ProductQueries
{
    public class ProductBarcodeControlQuery : IRequest<ResponseBase<bool>>
    {
        public string Code { get; set; }
        public Guid SellerId { get; set; }
    }
}
