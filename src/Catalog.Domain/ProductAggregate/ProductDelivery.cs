using Catalog.Domain.Entities;
using Catalog.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Catalog.Domain.ProductAggregate
{
    public class ProductDelivery : Entity
    {
        public Guid ProductId { get; protected set; }
        public Guid SellerId { get; protected set; }
        public Guid DeliveryId { get; protected set; }
        public Guid? CityId { get; protected set; }
        public DeliveryType DeliveryType { get; set; }

        private readonly List<Guid?> _cityList;
        public ICollection<Guid?> CityList => _cityList;

        protected ProductDelivery()
        {
            _cityList = new List<Guid?>();
        }

        public ProductDelivery(Guid productId, Guid sellerId, Guid deliveryId, Guid? cityId, DeliveryType deliveryType)
        {
            ProductId = productId;
            SellerId = sellerId;
            DeliveryId = deliveryId;
            CityId = cityId;
            DeliveryType = deliveryType;
        }

        public void SetProductDelivery(Guid productId, Guid sellerId, Guid deliveryId, Guid? cityId, DeliveryType deliveryType)
        {
            ProductId = productId;
            SellerId = sellerId;
            DeliveryId = deliveryId;
            CityId = cityId;
            DeliveryType = deliveryType;

        }
    }
}
