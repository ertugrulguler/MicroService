using System;
using System.Collections.Generic;

namespace Catalog.ApplicationService.Communicator.Merchant.Model
{
    public class GetSellerFilterInfosResponse
    {
        public List<SellerInfoList> SellerInfoLists { get; set; }
    }
    public class SellerInfoList
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string SeoName { get; set; }
    }
}
