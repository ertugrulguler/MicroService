using System;

namespace Catalog.ApplicationService.Communicator.Merchant.Model
{
    public class GetSellerResponse
    {
        public Guid UserId { get; set; }
        public string FirmName { get; set; }
        public string CompanyName { get; set; }
    }
}
