using Catalog.ApiContract.Contract;
using Catalog.Domain.CategoryAggregate.ServiceModel;

using Framework.Core.Model;

using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Query.ProductQueries
{
    public class FavoriteProductList
    {
        public List<FavoriteProducts> List { get; set; }
        public int TotalCount { get; set; }
    }

    public class FavoriteProducts
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid SellerId { get; set; }
        public string SeoUrl { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string ImageUrl { get; set; }
        public Price ListPrice { get; set; }
        public Price SalePrice { get; set; }
        public BrandDto BrandDto { get; set; }
        public bool IsVariantable { get; set; }
        public CategoryIdAndNameforProducts CategoryInfo { get; set; }
    }
}
