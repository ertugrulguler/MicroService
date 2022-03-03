using System;
using System.Xml;
using System.Xml.Serialization;

namespace Catalog.ApiContract.Contract
{
    public class ProductXmlDto
    {
        [XmlElement(ElementName = "merchantItemId")]
        public Guid ID { get; set; }
        [XmlElement(ElementName = "merchantItemCategoryId")]
        public Guid? CategoryID { get; set; }
        [XmlElement(ElementName = "merchantItemCategoryName")]
        public XmlCDataSection? CategoryName { get; set; }
        [XmlElement(ElementName = "brand")]
        public XmlCDataSection? Brand { get; set; }
        //[XmlElement(ElementName = "model")] //No Data
        //public XmlCDataSection? Model { get; set; } 
        [XmlElement(ElementName = "itemTitle")]
        public XmlCDataSection? Name { get; set; }
        [XmlElement(ElementName = "merchantItemField")]
        public XmlCDataSection? Field { get; set; }
        [XmlElement(ElementName = "itemUrl")]
        public XmlCDataSection? Url { get; set; }
        //[XmlElement(ElementName = "priceEft")] //No Data
        //public decimal PriceEFT { get; set; }
        [XmlElement(ElementName = "pricePlusTax")]
        public decimal? PricePlusTax { get; set; }
        [XmlElement(ElementName = "itemUrlMobile")]
        public XmlCDataSection? UrlMobile { get; set; }
        [XmlArray("itemImageUrls")]
        [XmlArrayItem("itemImageUrl")]
        public XmlCDataSection?[] ImageUrls { get; set; }
        [XmlElement(ElementName = "shippingFee")] //No Data
        public decimal? ShippingFee { get; set; }
        [XmlElement(ElementName = "stockStatus")]
        public int? Stock { get; set; }
        //[XmlElement(ElementName = "stockDetail")] //No Data
        //public XmlCDataSection? StockDetail { get; set; }
        [XmlElement(ElementName = "shippingDay")]
        public int? ShippingDay { get; set; }
        [XmlElement(ElementName = "shippingDetail")]
        public XmlCDataSection? ShippingDetail { get; set; }
        //[XmlElement(ElementName = "typeOfWarranty")] //No Data
        //public int TypeWarranty { get; set; }
        //[XmlElement(ElementName = "warrantyPeriod")] //No Data
        //public int WarrantyPeriod { get; set; }
        [XmlArray("eans")]
        [XmlArrayItem("ean")]
        public XmlCDataSection?[] Eans { get; set; }
        [XmlArray("specs")]
        [XmlArrayItem("spec")]
        public Spec?[] Specs { get; set; }
        //[XmlArray("installments")]
        //[XmlArrayItem("installment")]
        //public InstallmentDto[] Installments { get; set; }
    }

    //public class Installment
    //{
    //    [XmlElement(ElementName = "card")] //No Data
    //    public XmlCDataSection? Card { get; set; }
    //    [XmlElement(ElementName = "month")] //No Data
    //    public int? Month { get; set; }
    //    [XmlElement(ElementName = "installmentPrice")] //No Data
    //    public XmlCDataSection? InstallmentPrice { get; set; }
    //}

    public class Spec
    {
        //[XmlElement(ElementName = "description")]
        //public XmlCDataSection? Description { get; set; }
        [XmlElement(ElementName = "values")]
        public XmlCDataSection? Values { get; set; }
    }


    [XmlRoot("MerchantItems")]
    public class MerchantItems
    {
        [XmlElement("MerchantItem")]
        public ProductXmlDto[] ProductXmlDtos { get; set; }
    }

}
