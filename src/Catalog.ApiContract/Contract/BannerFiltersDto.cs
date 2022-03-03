using Catalog.Domain.Enums;
using System;

namespace Catalog.ApiContract.Contract
{
    public class BannerFiltersDto
    {
        public BannerFilterType BannerFilterType { get; set; }
        public Guid ActionId { get; set; }
    }
}
