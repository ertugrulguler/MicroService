using Catalog.ApiContract.Response.Query.ProductQueries;
using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Query.ProductQueries
{
    public class GetProductSeoUrlListQuery : IRequest<ResponseBase<List<GetProductSeoUrlListResponse>>>
    {
        public List<Guid> ProductId { get; set; }
    }
}
