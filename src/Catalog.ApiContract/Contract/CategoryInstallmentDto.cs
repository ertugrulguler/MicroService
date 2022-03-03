using System;

namespace Catalog.ApiContract.Contract
{
    public class CategoryInstallmentDto
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int MaxInstallmentCount { get; set; }
        public decimal? MinPrice { get; set; }
        public int NewMaxInstallmentCount { get; set; }

    }
}
