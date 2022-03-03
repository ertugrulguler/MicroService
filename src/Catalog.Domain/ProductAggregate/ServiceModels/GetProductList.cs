using Catalog.Domain.Enums;
using Framework.Core.Model;
using System;
using System.Collections.Generic;

namespace Catalog.Domain.ProductAggregate.ServiceModels
{
    public class GetProductList
    {
        public List<FilterModel> FilterModel { get; set; }
        public List<Guid> CategoryIds { get; set; }
        public OrderBy OrderBy { get; set; }
        public ProductChannelCode ProductChannelCode { get; set; }
        public PagerInput PagerInput { get; set; }
        public Guid SellerId { get; set; }
        public bool IsSellerVisible { get; set; }
        public string SeoUrl { get; set; }
    }
}
