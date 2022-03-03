using Catalog.ApiContract.Response.Query.ProductQueries;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Query.ProductQueries
{
    public class GetProductQuery : IRequest<ResponseBase<ProductDetail>>
    {
        public Guid Id { get; set; }
    }
}
