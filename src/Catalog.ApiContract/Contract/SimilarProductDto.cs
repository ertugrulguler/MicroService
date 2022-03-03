using System;

namespace Catalog.ApiContract.Contract
{
    public class SimilarProductDto
    {
        public Guid? FirstProductId { get; set; }
        public Guid? SecondProductId { get; set; }
    }
}
