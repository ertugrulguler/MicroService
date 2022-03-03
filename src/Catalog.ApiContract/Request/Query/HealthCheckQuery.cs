using Framework.Core.Model;
using MediatR;

namespace Catalog.ApiContract.Request.Query
{
    public class HealthCheckQuery : IRequest<ResponseBase<object>>
    {
    }
}