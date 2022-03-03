using System;
using System.Collections.Generic;

namespace Catalog.ApplicationService.Communicator.Merchant.Model
{
    public class GetSellerFilterInfosRequest
    {
        public List<Guid> SellerIdList { get; set; }
    }
}
