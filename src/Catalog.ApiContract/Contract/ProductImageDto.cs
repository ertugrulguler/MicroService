using System;

namespace Catalog.ApiContract.Contract
{
    public class ProductImageDto
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public Guid? SellerId { get; set; }
        public int SortOrder { get; set; }
        public bool IsDefault { get; set; }
    }
}
