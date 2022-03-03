using System;
using System.Collections.Generic;

namespace Catalog.ApplicationService.Communicator.Merchant.Model
{

    public class GetSellerDeliveryRequest
    {
        public List<Guid> DeliveryIds { get; set; }

    }
}
