using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Services
{
    public interface IAttributeService
    {
        bool ReadFromExcelWithAttributeAllRelation(IFormFile fi);
        Task<bool> UpdateAttributeValueOrder();
    }
}
