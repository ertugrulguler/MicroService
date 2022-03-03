using System;

namespace Catalog.ApiContract.Contract
{
    public class ProductGroupDto
    {
        public Guid ProductId { get; set; }
        public string GroupCode { get; set; }
    }
}
