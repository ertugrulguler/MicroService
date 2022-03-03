using Catalog.ApiContract.Contract;
using Catalog.Domain.CategoryAggregate.ServiceModel;

using Framework.Core.Model;
using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Query.ProductQueries
{
    public class ProductList
    {
        public Guid Id { get; set; }
        public Guid ProductSellerId { get; set; }
        public string Name { get; set; }
        public string ProductCode { get; set; }
        public string DisplayName { get; set; }
        public string SeoUrl { get; set; }
        public List<string> ImageUrl { get; set; }
        public Price ListPrice { get; set; }
        public Price SalePrice { get; set; }
        public DiscountRateInfo DiscountRate { get; set; }
        public BrandDto BrandDto { get; set; }
        public Guid SellerId { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsVariantable { get; set; }
        public string IsVariantableStr { get; set; }
        public CategoryIdAndNameforProducts CategoryInfo { get; set; }

    }

    public class DiscountRateInfo
    {
        public string DiscountRate { get; set; }
        public StyledText DiscountRateText { get; set; }
    }
}
