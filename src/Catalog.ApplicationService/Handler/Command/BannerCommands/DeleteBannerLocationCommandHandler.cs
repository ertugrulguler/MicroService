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
    public class DeleteBannerLocationCommandHandler : IRequestHandler<DeleteBannerLocationCommand,
        ResponseBase<object>>
    {
        private readonly IBannerLocationRepository _bannerLocationRepository;
        private readonly IDbContextHandler _dbContextHandler;
        private readonly IBannerService _bannerService;

        public DeleteBannerLocationCommandHandler(IBannerLocationRepository bannerLocationRepository,
            IDbContextHandler dbContextHandler, IBannerService bannerService)
        {
            _dbContextHandler = dbContextHandler;
            _bannerService = bannerService;
            _bannerLocationRepository = bannerLocationRepository;
        }

        public async Task<ResponseBase<object>> Handle(DeleteBannerLocationCommand request,
            CancellationToken cancellationToken)
        {

            var entity = await _bannerLocationRepository.FindByAsync(g => g.Order == request.Order &&
                                                                          g.BannerType == request.BannerType &&
                                                                          g.Location == request.BannerLocationType);
            if (entity == null)
            {
                throw new BusinessRuleException(ApplicationMessage.InvalidBannerLocation,
                    ApplicationMessage.InvalidBannerLocation.Message(),
                    ApplicationMessage.InvalidBannerLocation.UserMessage());
            }
            _bannerLocationRepository.Delete(entity);
            await _dbContextHandler.SaveChangesAsync();

            return new ResponseBase<object>
            {
                Success = true
            };
        }
    }
}
