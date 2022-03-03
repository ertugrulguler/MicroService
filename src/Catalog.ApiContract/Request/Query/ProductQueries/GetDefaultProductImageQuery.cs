using Catalog.ApiContract.Response.Query.ProductQueries;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Query.ProductQueries
{
    public class GetDefaultProductImageQuery : IRequest<ResponseBase<GetDefaultProductImage>>
    {
        public Guid ProductId { get; set; }
        public Guid SellerId { get; set; }
    }
}