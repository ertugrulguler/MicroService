using Catalog.ApiContract.Request.Query.BannerQueries;
using Catalog.ApiContract.Response.Query.BannerQueries;
using Catalog.ApplicationService.Assembler;
using Catalog.Domain.BannerAggregate;
using Framework.Core.Model;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.BannerQueries
{
    public class GetBannersActionTypeHandler : IRequestHandler<GetBannersActionTypeQuery, ResponseBase<GetBannerActionTypeList>>
    {
        private readonly IBannerActionTypeRepository _bannerActionTypeRepository;
        private readonly IBannerAssembler _bannerAssembler;


        public GetBannersActionTypeHandler(IBannerActionTypeRepository bannerActionTypeRepository, IBannerAssembler bannerAssembler)
        {
            _bannerActionTypeRepository = bannerActionTypeRepository;
            _bannerAssembler = bannerAssembler;
        }
        public async Task<ResponseBase<GetBannerActionTypeList>> Handle(GetBannersActionTypeQuery request, CancellationToken cancellationToken)
        {
            var getBannerTypeResponseList = new ResponseBase<List<GetBannerActionTypeList>>
            {
                Data = new List<GetBannerActionTypeList>()
            };
            var bannerList = await _bannerActionTypeRepository.AllAsync();
            var response = _bannerAssembler.MapToBannerActionTypeListQueryResult(bannerList);

            return new ResponseBase<GetBannerActionTypeList>
            {
                Data = new GetBannerActionTypeList
                {
                    BannerActionType = response.Data.BannerActionType
                },
                Success = true
            };
        }
    }
}