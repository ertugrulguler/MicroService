using Catalog.ApiContract.Response.Query.BannerQueries;
using Framework.Core.Model;
using MediatR;

namespace Catalog.ApiContract.Request.Query.BannerQueries
{
    public class GetBannerForBOQuery : IRequest<ResponseBase<GetBannerListForBO>>
    {
        public int Size { get; set; }
        public int Page { get; set; }
    }
}
