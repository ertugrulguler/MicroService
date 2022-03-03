using Catalog.ApplicationService.Communicator.Merchant.Model;
using Framework.Core.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Communicator.Merchant
{
    public interface IMerhantCommunicator
    {
        Task<ResponseBase<GetSellerFilterInfosResponse>> GetProductSellersWithFilters(GetSellerFilterInfosRequest request);
        Task<ResponseBase<GetSellerResponse>> GetSellerById(GetSellerRequest request);
        Task<ResponseBase<GetSellerDeliveryResponse>> GetDeliveriesWithId(GetSellerDeliveryRequest request);
        Task<ResponseBase<List<Guid>>> GetBannedSellers();
        Task<ResponseBase<GetSellerBySeoNameResponse>> GetSellerBySeoName(string seoName);
        Task<ResponseBase<List<GetSellerDetailByIdsResponse>>> GetSellerDetailByIds(GetSellerDetailByIdsRequest request);



    }
}
