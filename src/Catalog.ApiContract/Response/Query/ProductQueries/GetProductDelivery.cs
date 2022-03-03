using Catalog.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Query.ProductQueries
{
    public class GetProductDelivery
    {
        public List<ProductDeliveries> ProductDeliveries { get; set; }
    }

    public class ProductDeliveries
    {
        public Guid ProductId { get; set; }
        public Guid SellerId { get; set; }

        public List<DeliveryOption> DeliveryOptions { get; set; }
        public bool UseOverdraftInstallment { get; set; }
        public int? OverdraftInstallmentCount { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class DeliveryOption
    {
        public DeliveryType DeliveryType { get; set; }
        public Guid DeliveryId { get; set; }
        public List<DeliveryCity> CityId { get; set; }

    }
    public class DeliveryCity
    {
        public Guid? CityId { get; set; }
    }
}
