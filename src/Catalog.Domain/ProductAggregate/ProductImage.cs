using Catalog.Domain.Entities;
using System;

namespace Catalog.Domain.ProductAggregate
{
    public class ProductImage : Entity
    {

        public string Name { get; protected set; }
        public string Url { get; protected set; }
        public string Description { get; protected set; }
        public Guid? ProductId { get; protected set; }
        public Guid? SellerId { get; protected set; }
        public int SortOrder { get; protected set; }
        public bool IsDefault { get; protected set; }

        protected ProductImage()
        {

        }


        public ProductImage(string name, string url, string description, Guid? productId, Guid? sellerId, int sortOrder, bool isDefault) : this()
        {
            Name = name;
            Url = url;
            Description = description;
            ProductId = productId;
            SellerId = sellerId;
            SortOrder = sortOrder;
            IsDefault = isDefault;
        }

        public void SetProductImage(string name, string description, int sortOrder, bool isDefault)
        {
            Name = name;
            Description = description;
            SortOrder = sortOrder;
            IsDefault = isDefault;
        }
    }
}
