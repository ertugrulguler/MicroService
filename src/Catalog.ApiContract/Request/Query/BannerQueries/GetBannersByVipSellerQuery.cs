using Catalog.ApiContract.Response.Query.BannerQueries;
using Catalog.Domain.Enums;
using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Query.BannerQueries
{
    public class GetBannersByVipSellerQuery : IRequest<ResponseBase<List<GetBannersByVipSeller>>>
    {
        public Guid SellerId { get; set; }
        public BannerLocationType BannerLocationType { get; set; }
        public Framework.Core.Model.Enums.ChannelCode ChannelCode { get; set; }
    }
}
