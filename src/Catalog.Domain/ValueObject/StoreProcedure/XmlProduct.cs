using Microsoft.EntityFrameworkCore;

using System;

namespace Catalog.Domain.ValueObject.StoreProcedure
{
    [Keyless]
    public class XmlProduct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string SeoName { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Guid SellerId { get; set; }
        public int StockCount { get; set; }
        public decimal Price { get; set; }
        public string BrandName { get; set; }
        public int DeliveryDuration { get; set; }
        public int LastHourForDeliveryDuration { get; set; }
        public string CampaignText { get; set; }
        public decimal ShippingPrice { get; set; }

        //public string Model { get; set; } 
        //public string Url { get; set; }
        //public decimal PriceEFT { get; set; }
        //public string UrlMobile { get; set; }
        //public string StockDetail { get; set; }
        //public int TypeWarranty { get; set; }
        //public int WarrantyPeriod { get; set; }
        //public InstallmentDto[] Installments { get; set; }
    }

    //public class Installment
    //{
    //    public string Card { get; set; }
    //    public int Month { get; set; }
    //    public string InstallmentPrice { get; set; }
    //}

    [Keyless]
    public class XmlAttribute
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        //public string DisplayName { get; set; }
        public string Value { get; set; }
    }

}
