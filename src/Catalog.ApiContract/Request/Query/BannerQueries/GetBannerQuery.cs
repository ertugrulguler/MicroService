using Catalog.ApiContract.Response.Query.BannerQueries;
using Catalog.Domain.Enums;
using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Query.BannerQueries
{
    public class GetBannerQuery : CachedQuery, IRequest<ResponseBase<List<GetBannerList>>>
    {
        public BannerLocationType BannerLocationType { get; set; }
        public Guid? ActionId { get; set; }
        public ProductChannelCode ProductChannelCode { get; set; }
        public override string CacheKey => nameof(GetBannerQuery) + ":" + BannerLocationType + ":" + ProductChannelCode +
            (ActionId == null ? "" : ":" + "ActionId#" + ActionId.ToString());
    }

}
