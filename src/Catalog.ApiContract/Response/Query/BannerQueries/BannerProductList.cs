using Framework.Core.Model;
using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Query.BannerQueries
{
    public class BannerProductList
    {
        public Guid Id { get; set; }
        public Guid BannerId { get; set; }
        public Guid ProductSellerId { get; set; }
        public string Name { get; set; }
        public string SeoUrl { get; set; }
        public string DisplayName { get; set; }
        public List<string> ImageUrl { get; set; }
        public Price ListPrice { get; set; }
        public Price SalePrice { get; set; }
        public Guid SellerId { get; set; }
        public bool IsFavorite { get; set; }
    }


}
