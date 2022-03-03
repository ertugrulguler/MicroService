using System;

namespace Catalog.ApplicationService.Communicator.Merchant.Model
{
    public class GetSellerDetailByIdsResponse
    {
        public Guid SellerId { get; set; }
        public string FirmName { get; set; }
        public string SellerSeoName { get; set; }
        public string LogoUrl { get; set; }
    }
}
