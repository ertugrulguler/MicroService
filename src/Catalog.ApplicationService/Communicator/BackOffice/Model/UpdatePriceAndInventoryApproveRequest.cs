using Catalog.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Catalog.ApplicationService.Communicator.BackOffice.Model
{
    public class UpdatePriceAndInventoryApproveRequest
    {
        public Guid SellerId { get; set; }
        public List<CreateProduct> Products { get; set; }
    }

    public class CreateProduct
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public Guid BrandId { get; set; }
        public int Desi { get; set; }
        public string Code { get; set; }
        public string GroupCode { get; set; }
        public int StockCount { get; set; }
        public string StockCode { get; set; }
        public int VatRate { get; set; }
        public decimal ListPrice { get; set; }
        public decimal SalePrice { get; set; }
        public int InstallmentCount { get; set; }
        public Guid CategoryId { get; set; }
        public List<CreateProductAttribute> Attributes { get; set; }
        public List<CreateProductImage> Images { get; set; }
        public List<CreateProductDelivery> Deliveries { get; set; }
    }

    public class CreateProductAttribute
    {
        public Guid AttributeId { get; set; }

        public Guid AttributeValueId { get; set; }
    }

    public class CreateProductImage
    {
        public string ImageUrl { get; set; }
    }
    public class CreateProductDelivery
    {
        public Guid DeliveryId { get; set; }
        public DeliveryType DeliveryType { get; set; }
        public List<Guid> CityList { get; set; }
    }
}