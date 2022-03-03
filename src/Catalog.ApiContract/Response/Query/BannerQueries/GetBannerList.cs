using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.Domain.Enums;
using System;
using System.Collections.Generic;


namespace Catalog.ApiContract.Response.Query.BannerQueries
{
    public class GetBannerList
    {
        public Guid? BannerLocationId { get; set; }
        public BannerType? Type { get; set; }
        public string Title { get; set; }
        public List<Items> Items { get; set; }
        public List<BannerProductList> ProductList { get; set; }
    }

    public class Items
    {
        public Guid BannerId { get; set; }
        public string Name { get; set; }
        public string SeoUrl { get; set; }
        public BannerActionType ActionType { get; set; }
        public string ImageUrl { get; set; }
        public Guid? ActionId { get; set; } //filter ise actionId catId 
        public GetProductListAndFilterQuery Filter { get; set; }
        public ProductDetails ProductDetail { get; set; }
        public int? MMActionId { get; set; }
    }

    public class ProductDetails
    {
        public Guid? ProductSellerId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? SellerId { get; set; }
    }
}

