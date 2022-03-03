using Catalog.Domain.Enums;
using System;

namespace Catalog.ApiContract.Contract
{
    public class ProductDeliveryDto
    {
        public Guid SellerId { get; set; }
        public Guid DeliveryId { get; set; }
        public Guid? CityId { get; set; }
        public DeliveryType DeliveryType { get; set; }
    }
}
