using Catalog.ApiContract.Response.Query.BannerQueries;
using Catalog.Domain.Enums;
using Framework.Core.Model;
using MediatR;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Query.BannerQueries
{
    public class GetBannerChannelQuery : CachedQuery, IRequest<ResponseBase<GetBannerChannelQueryResponse>>
    {
        public BannerLocationType BannerLocationType { get; set; }
        public ProductChannelCode ProductChannelCode { get; set; }
    }
}