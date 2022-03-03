using Catalog.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Catalog.ApplicationService.Communicator.Merchant.Model
{
    public class GetSellerDeliveryResponse
    {
        public List<SellerDeliveryResponse> SellerDeliveries { get; set; }
    }
    public class SellerDeliveryResponse
    {
        public Guid Id { get; set; }
        public Guid SellerId { get; set; }
        public CargoType CargoType { get; set; }
        public Guid CargoCompanyId { get; set; }
        public decimal Price { get; set; }
        public decimal CampaignPrice { get; set; }
        public decimal CampaignAmount { get; set; }
        public string CampaignText { get; set; }
        public int DeliveryDuration { get; set; }
        public int LastHourForDeliveryDuration { get; set; }
    }
}
