using Catalog.Domain.Enums;

using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Contract
{
    public class BannerDto
    {
        public BannerActionType ActionType { get; set; }
        public Guid BannerLocationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ActionId { get; set; } //catId,ProdId,pSellerId
        public string ImageUrl { get; set; }
        public int Order { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<BannerFiltersDto> BannerFilters { get; set; }
    }
}
