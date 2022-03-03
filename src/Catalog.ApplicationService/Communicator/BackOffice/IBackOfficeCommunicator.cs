using Catalog.ApplicationService.Communicator.BackOffice.Model;
using Framework.Core.Model;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Communicator.BackOffice
{
    public interface IBackOfficeCommunicator
    {
        Task<UpdatePriceAndInventoryApproveResponse> UpdatePriceAndInventoryApprove(UpdatePriceAndInventoryApproveRequest request);
        Task<ResponseBase<CategoryCompanyInstallmentResponse>> CategoryCompanyInstallment(CategoryCompanyInstallmentRequest request);
        Task<ResponseBase<CategoryCompanyMileResponse>> CategoryCompanyMile(CategoryCompanyMileRequest request);
        Task<ResponseBase<object>> DeleteCache(string key);
    }
}
