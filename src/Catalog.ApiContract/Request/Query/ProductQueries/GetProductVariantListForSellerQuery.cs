using Catalog.ApiContract.Response.Query.ProductQueries;
using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Query.ProductQueries
{
    public class GetProductVariantListForSellerQuery : IRequest<ResponseBase<List<ProductVariantGroup>>>
    {
        public List<Guid> ProductIdList { get; set; }

    }
}
