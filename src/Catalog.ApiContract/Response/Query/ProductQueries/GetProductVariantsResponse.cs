using Framework.Core.Model;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Query.ProductQueries
{
    public class GetProductVariantsResponse
    {
        public string DisplayName { get; set; }
        public Price ListPrice { get; set; }
        public Price SalePrice { get; set; }
        public DiscountRateInfo DiscountRate { get; set; }
        public SellerProductImage Image { get; set; }
        public List<ProductVariantGroup> FirstVariantGroup { get; set; }
        public List<ProductVariantGroup> SecondVariantGroup { get; set; }
    }
}
