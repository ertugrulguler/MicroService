using System;

namespace Catalog.ApiContract.Response.Query.BasketQueries
{
    public class BasketDetailInstallment
    {
        public Guid ProductId { get; set; }
        public Guid SellerId { get; set; }
        public int InstallmentCount { get; set; }
        public string InstallmentType { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
    }
}
