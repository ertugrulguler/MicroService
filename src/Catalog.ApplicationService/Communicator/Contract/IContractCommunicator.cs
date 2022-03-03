using Catalog.ApplicationService.Communicator.Contract.Model;
using Framework.Core.Model;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Communicator.Contract
{
    public interface IContractCommunicator
    {
        Task<ResponseBase<object>> TemplatePreview(TemplatePreviewRequest request);
    }
}
