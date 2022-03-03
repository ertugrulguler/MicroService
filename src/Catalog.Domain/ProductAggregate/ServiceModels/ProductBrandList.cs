using System;
using System.Collections.Generic;

namespace Catalog.Domain.ProductAggregate.ServiceModels
{
    public class ProductBrandList
    {
        public List<BrandList> BrandList { get; set; }
        public DateTime LastDateTime { get; set; }
        public int Count { get; set; }
    }

    public class BrandList
    {
        public Guid BrandId { get; set; }
        public string BrandName { get; set; }

    }
}
