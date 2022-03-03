using System;
using System.Collections.Generic;

namespace Catalog.ApplicationService.Communicator.Merchant.Model
{
    public class GetSellerDetailByIdsRequest
    {
        public List<Guid> SellerId { get; set; }
    }
}
