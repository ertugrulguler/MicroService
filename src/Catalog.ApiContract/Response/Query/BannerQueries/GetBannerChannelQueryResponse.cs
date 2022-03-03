using Catalog.ApiContract.Response.Query.ProductQueries;
using Framework.Core.Model;
using Framework.Core.Model.Enums;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Query.BannerQueries
{
    public class GetBannerChannelQueryResponse
    {
        public List<ChannelItem> BannerItems { get; set; }
        public string AllOpportunityForIscepUrl { get; set; }
    }
    public class ChannelItem
    {
        public ChannelCode ChannelCode { get; set; }
        public string Title { get; set; }
        public string BannerImageUrl { get; set; }
        public string Description { get; set; }
        public string StartedDate { get; set; }
        public string EndDate { get; set; }
        public string Deeplink { get; set; }
        public ProductDetailsChannel ProductDetail { get; set; }
    }

    public class ProductDetailsChannel
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string SellerName { get; set; }
        public string Description { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public ProductPrice Prices { get; set; }
    }


    public class ProductPrice
    {
        public int VatRate { get; set; }
        public Price ListPrice { get; set; }
        public Price SalePrice { get; set; }
        public DiscountRateInfoByChannel DiscountRate { get; set; }
    }

    public class DiscountRateInfoByChannel
    {
        public string DiscountRate { get; set; }
        public string DiscountPrice { get; set; }
        public StyledText DiscountRateText { get; set; }
    }

    public class ProductImage
    {
        public string ImageUrl { get; set; }
    }
}