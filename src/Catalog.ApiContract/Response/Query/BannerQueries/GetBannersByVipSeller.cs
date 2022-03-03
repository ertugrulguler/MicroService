using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.Domain.Enums;
using System;
using System.Collections.Generic;


namespace Catalog.ApiContract.Response.Query.BannerQueries
{
    public class GetBannersByVipSeller
    {
        public BannerType? Type { get; set; }
        public string Title { get; set; }
        public List<VipItems> VipItems { get; set; }
    }

    public class VipItems
    {
        public string Name { get; set; }
        public BannerActionType ActionType { get; set; }
        public string ImageUrl { get; set; }
        public Guid? ActionId { get; set; } //filter ise actionId catId 
        public GetProductListAndFilterQuery Filter { get; set; }
        public VipProductDetails VipProductDetails { get; set; }

    }

    public class VipProductDetails
    {
        public Guid? ProductSellerId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? SellerId { get; set; }
    }
}

