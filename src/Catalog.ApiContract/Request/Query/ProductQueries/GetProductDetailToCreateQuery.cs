using Catalog.ApiContract.Response.Query.ProductQueries;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Query.ProductQueries
{
    /// <summary>
    /// Catalogda ki bir ürünü, baska bir seller'a create edip, onaya gonder.
    /// </summary>>
    public class GetProductDetailToCreateQuery : IRequest<ResponseBase<GetProductDetailToCreate>>
    {
        public Guid NewSellerId { get; set; }
        public Guid ProductId { get; set; }
    }
}
