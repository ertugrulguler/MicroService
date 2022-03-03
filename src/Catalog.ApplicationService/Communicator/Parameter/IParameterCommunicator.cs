using Catalog.ApiContract.Contract;
using Catalog.ApplicationService.Communicator.Parameter.Model;
using Framework.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Communicator.Parameter
{
    public interface IParameterCommunicator
    {
        Task<ResponseBase<List<IconResponse>>> GetBadges();
        Task<ResponseBase<List<ProductChannelDto>>> GetProductChannelList();
        Task<ResponseBase<List<CityListResponse>>> GetCities();
    }
}