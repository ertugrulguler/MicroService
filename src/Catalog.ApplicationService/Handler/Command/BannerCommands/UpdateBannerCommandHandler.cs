using Catalog.ApiContract.Request.Command.BannerCommands;
using Catalog.ApplicationService.Handler.Services;
using Catalog.Domain;
using Catalog.Domain.BannerAggregate;

using Framework.Core.Model;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.BannerCommands
{
    public class UpdateBannerCommandHandler : IRequestHandler<UpdateBannerCommand, ResponseBase<object>>
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IDbContextHandler _dbContextHandler;
        private readonly IBannerService _bannerService;

        public UpdateBannerCommandHandler(IDbContextHandler dbContextHandler, IBannerService bannerService, IBannerRepository bannerRepository)
        {
            _dbContextHandler = dbContextHandler;
            _bannerService = bannerService;
            _bannerRepository = bannerRepository;
        }

        public async Task<ResponseBase<object>> Handle(UpdateBannerCommand request, CancellationToken cancellationToken)
        {
            var banner = await _bannerRepository.FindByAsync(b => b.Id == request.Id);
            if (banner == null)
                throw new BusinessRuleException(ApplicationMessage.InvalidId,
                ApplicationMessage.InvalidId.Message(),
                ApplicationMessage.InvalidId.UserMessage());

            if (request.MMActionId != 0) banner.UpdateBannerAndMMActionId(request.ImageUrl, request.Order, request.MMActionId, request.StartDate, request.EndDate);
            else banner.UpdateBanner(request.ImageUrl, request.Order, request.StartDate, request.EndDate);
            _bannerRepository.Update(banner);
            await _dbContextHandler.SaveChangesAsync();

            return new ResponseBase<object>
            {
                Success = true
            };
        }
    }
}
