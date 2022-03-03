using System;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Services
{
    public interface IAttributeValueService
    {
        Task<Guid?> GetAttributeValueId(string value);
    }
}