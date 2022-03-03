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
    public class UpdateBannerLocationCommandHandler : IRequestHandler<UpdateBannerLocationCommand, ResponseBase<object>>
    {
        private readonly IBannerLocationRepository _bannerLocationRepository;
        private readonly IDbContextHandler _dbContextHandler;
        private readonly IBannerService _bannerService;

        public UpdateBannerLocationCommandHandler(IDbContextHandler dbContextHandler, IBannerService bannerService, IBannerLocationRepository bannerLocationRepository)
        {
            _dbContextHandler = dbContextHandler;
            _bannerService = bannerService;
            _bannerLocationRepository = bannerLocationRepository;
        }

        public async Task<ResponseBase<object>> Handle(UpdateBannerLocationCommand request, CancellationToken cancellationToken)
        {
            var bannerLocation = await _bannerLocationRepository.FindByAsync(b => b.Id == request.Id);
            if (bannerLocation == null)
                throw new BusinessRuleException(ApplicationMessage.InvalidId,
                ApplicationMessage.InvalidId.Message(),
                ApplicationMessage.InvalidId.UserMessage());

            bannerLocation.SetBannerLocation(request.Title, request.Order, request.IsActive);
            _bannerLocationRepository.Update(bannerLocation);
            await _dbContextHandler.SaveChangesAsync();

            return new ResponseBase<object>
            {
                Success = true
            };
        }
    }
}
