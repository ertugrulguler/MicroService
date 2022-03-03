using Catalog.ApiContract.Contract;
using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Query.ProductQueries
{

    public class ProductDetail
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public Guid BrandId { get; set; }
        public BrandDto Brand { get; set; }
        public string Code { get; set; }
        public int PriorityRank { get; set; }
        public int? ProductMainId { get; set; }

        public List<AttributeDto> Attributes { get; set; }
        public List<CategoryDto> Categories { get; set; }
        public List<SimilarProductDto> SimilarProducts { get; set; }
        public List<ProductSellerDto> ProductSellers { get; set; }
    }
}
