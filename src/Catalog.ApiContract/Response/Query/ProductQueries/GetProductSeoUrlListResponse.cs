using System;

namespace Catalog.ApiContract.Response.Query.ProductQueries
{
    public class GetProductSeoUrlListResponse
    {
        public Guid ProductId { get; set; }
        public string ProductSeoUrl { get; set; }
    }
}
