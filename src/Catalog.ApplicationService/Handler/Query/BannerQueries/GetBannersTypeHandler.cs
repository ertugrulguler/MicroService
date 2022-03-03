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
    public class GetBannersTypeHandler : IRequestHandler<GetBannersTypeQuery, ResponseBase<BannerTypeList>>
    {
        private readonly IBannerTypeRepository _bannerTypeRepository;
        private readonly IBannerAssembler _bannerAssembler;


        public GetBannersTypeHandler(IBannerTypeRepository bannerTypeRepository, IBannerAssembler bannerAssembler)
        {
            _bannerTypeRepository = bannerTypeRepository;
            _bannerAssembler = bannerAssembler;
        }
        public async Task<ResponseBase<BannerTypeList>> Handle(GetBannersTypeQuery request, CancellationToken cancellationToken)
        {
            var getBannerTypeResponseList = new ResponseBase<List<BannerTypeList>>
            {
                Data = new List<BannerTypeList>()
            };
            var bannerList = await _bannerTypeRepository.AllAsync();
            var response = _bannerAssembler.MapToBannerTypeListQueryResult(bannerList);

            return new ResponseBase<BannerTypeList>
            {
                Data = new BannerTypeList
                {
                    Banners = response.Data.Banners
                },
                Success = true
            };
        }
    }
}