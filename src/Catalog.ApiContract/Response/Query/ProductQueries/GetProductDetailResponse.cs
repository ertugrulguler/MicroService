using Catalog.ApiContract.Request.Query.ProductQueries;

using Framework.Core.Model;

using System;
using System.Collections.Generic;


namespace Catalog.ApiContract.Response.Query.ProductQueries
{
    public class GetProductDetailResponse
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string SellerName { get; set; }
        public string SellerSeoName { get; set; }
        public string Description { get; set; }
        public bool IsDescriptionHtml { get; set; }
        public Guid BrandId { get; set; }
        public string BrandName { get; set; }
        public string BrandSeoName { get; set; }
        public string CategoryName { get; set; }
        public string Code { get; set; }
        public int StockCount { get; set; }
        public string StockCode { get; set; }
        public bool IsFavorite { get; set; }
        public List<SellerProductAttribute> Attributes { get; set; }
        public List<SellerProductCategory> Categories { get; set; }
        public List<SellerProductImage> Images { get; set; }
        public SellerProductPrice Prices { get; set; }
        public string Installments { get; set; }
        public List<ProductVariantGroup> FirstVariantGroup { get; set; }
        public List<ProductVariantGroup> SecondVariantGroup { get; set; }
        public DeliveryOptions DeliveryOptions { get; set; }
        public List<OtherSellers> OtherSellers { get; set; }
        public StyledText AllCategoryText { get; set; }
        public AllCategoryUrl AllCategoryUrl { get; set; }
        public StyledText AllBrandText { get; set; }
        public AllBrandsUrl AllBrandsUrl { get; set; }
        public string ShareUrl { get; set; }
        public List<Breadcrumb> Breadcrumb { get; set; }
        public Guid SellerId { get; set; }
        public Guid ProductId { get; set; }
    }

    public class SellerProductAttribute
    {
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
        public bool IsVariantable { get; set; }
        public bool IsRequired { get; set; }
    }

    public class SellerProductCategory
    {
        public string CategoryName { get; set; }
    }

    public class SellerProductPrice
    {
        public int VatRate { get; set; }
        public Price ListPrice { get; set; }
        public Price SalePrice { get; set; }

        public DiscountRateInfo DiscountRate { get; set; }
    }

    public class SellerProductImage
    {
        public string ImageUrl { get; set; }
    }

    public class ProductVariantGroup
    {
        public Guid ProductSellerId { get; set; }
        public Guid ProductId { get; set; }
        public Guid SellerId { get; set; }
        public string SellerName { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
        public bool IsOpen { get; set; }
        public bool IsSelected { get; set; }
        public int OrderByAttributeValue { get; set; }
        public string SeoUrl { get; set; }
    }

    public class OtherSellers
    {
        public Guid SellerId { get; set; }
        public string SellerName { get; set; }
        public string SellerSeoName { get; set; }
        public Price SalePrice { get; set; }
        public Price ListPrice { get; set; }
        public DiscountRateInfo DiscountRate { get; set; }
        public DeliveryOptions DeliveryOptions { get; set; }
        public string ImageUrl { get; set; }
    }

    public class DeliveryOptions
    {
        public StyledText CargoTimeText { get; set; }
        public StyledText FastCargoTimeText { get; set; }
        public StyledText CargoPriceText { get; set; }
        public List<Badges> Badges { get; set; }
    }

    public class Badges
    {
        public string Type { get; set; }
        public string BadgeUrl { get; set; }
    }

    public class InstallmentTable
    {
        public int Installment { get; set; }
        public string InstallmentPrice { get; set; }
    }

    public class StyledText
    {
        public string Text { get; set; }
        public StyleInfo TextStyleInfo { get; set; }
        public List<SubStyleInfo> Styles { get; set; }
    }

    public class StyleInfo
    {
        public int FontSize { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public string TextColor { get; set; }
        public string BackgroundColor { get; set; }
    }

    public class SubStyleInfo
    {
        public string SubText { get; set; }
        public int FontSize { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public string TextColor { get; set; }
    }

    public class AllCategoryUrl
    {
        public GetProductListAndFilterQuery Filter { get; set; }
    }

    public class AllBrandsUrl
    {
        public GetProductListAndFilterQuery Filter { get; set; }
    }

    public class Breadcrumb
    {
        public Guid CategoryId { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
    }
}
