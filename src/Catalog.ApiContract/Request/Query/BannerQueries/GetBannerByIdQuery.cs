using Catalog.ApiContract.Response.Query.BannerQueries;
using Framework.Core.Model;
using MediatR;
using System;

namespace Catalog.ApiContract.Request.Query.BannerQueries
{
    public class GetBannerByIdQuery : IRequest<ResponseBase<GetBannerById>>
    {
        public Guid Id { get; set; }

    }
}
