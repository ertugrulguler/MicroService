using Catalog.ApiContract.Request.Command.BannerCommands;
using Catalog.ApiContract.Response.Command.BannerCommands;
using Catalog.ApplicationService.Handler.Services;
using Catalog.Domain;
using Catalog.Domain.BannerAggregate;

using Framework.Core.Model;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.BannerCommands
{
    public class CreateBannerLocationCommandHandler : IRequestHandler<CreateBannerLocationCommand,
        ResponseBase<CreateBannerLocationResult>>
    {
        private readonly IBannerLocationRepository _bannerLocationRepository;
        private readonly IDbContextHandler _dbContextHandler;
        private readonly IBannerService _bannerService;

        public CreateBannerLocationCommandHandler(IBannerLocationRepository bannerLocationRepository,
            IDbContextHandler dbContextHandler, IBannerService bannerService)
        {
            _dbContextHandler = dbContextHandler;
            _bannerService = bannerService;
            _bannerLocationRepository = bannerLocationRepository;
        }

        public async Task<ResponseBase<CreateBannerLocationResult>> Handle(CreateBannerLocationCommand request, CancellationToken cancellationToken)
        {
            CreateBannerLocationResult createBannerLocationResult;

            var entity = await _bannerLocationRepository.FindByAsync(g => g.Order == request.Order &&
                                                                          g.BannerType == request.BannerType &&
                                                                          g.Location == request.BannerLocationType
                                                                          && g.ProductChannelCode == request.ProductChannelCode);
            if (entity != null)
            {
                entity.Title = request.Title;
                entity.Description = request.Description;
                await _bannerService.CreateOrUpdateBannerLocation(entity, true);
            }
            else
            {
                entity = new BannerLocation(request.BannerType, request.Title, request.Order,
                    request.BannerLocationType, request.Description, request.ProductChannelCode, request.ActionId);
                await _bannerLocationRepository.SaveAsync(entity);
                await _dbContextHandler.SaveChangesAsync();

            }

            return new ResponseBase<CreateBannerLocationResult>
            {
                Data = createBannerLocationResult = new CreateBannerLocationResult
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    ActionId = entity.ActionId,
                    ProductChannelCode = entity.ProductChannelCode,
                    Location = entity.Location,
                    Description = entity.Description,
                    BannerType = entity.BannerType,
                    Order = entity.Order
                },
                Success = true,
            };


        }
    }
}
