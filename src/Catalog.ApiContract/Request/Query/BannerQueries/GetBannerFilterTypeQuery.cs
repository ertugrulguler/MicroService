using Catalog.ApiContract.Response.Query.BannerQueries;
using Framework.Core.Model;
using MediatR;

namespace Catalog.ApiContract.Request.Query.BannerQueries
{
    public class GetBannerFilterTypeQuery : IRequest<ResponseBase<GetBannerFilterTypeList>>
    {
    }
}
