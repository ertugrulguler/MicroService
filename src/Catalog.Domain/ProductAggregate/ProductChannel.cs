using Catalog.Domain.Entities;
using System;

namespace Catalog.Domain.ProductAggregate
{
    public class ProductChannel : Entity
    {
        public Guid ProductId { get; protected set; }
        public int ChannelCode { get; protected set; }


        public ProductChannel(Guid productId, int channelCode)
        {
            ProductId = productId;
            ChannelCode = channelCode;
        }

    }
}
