using Catalog.ApiContract.Request.Command.BannerCommands;
using Catalog.Domain;
using Catalog.Domain.BannerAggregate;

using Framework.Core.Model;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.BannerCommands
{
    public class DeleteBannerLocationByIdCommandHandler : IRequestHandler<DeleteBannerLocationByIdCommand, ResponseBase<object>>
    {
        private readonly IBannerLocationRepository _bannerLocationRepository;
        private readonly IDbContextHandler _dbContextHandler;

        public DeleteBannerLocationByIdCommandHandler(IDbContextHandler dbContextHandler, IBannerLocationRepository bannerLocationRepository)
        {
            _dbContextHandler = dbContextHandler;
            _bannerLocationRepository = bannerLocationRepository;
        }
        public async Task<ResponseBase<object>> Handle(DeleteBannerLocationByIdCommand request, CancellationToken cancellationToken)
        {
            var existing = await _bannerLocationRepository.FindByAsync(w => w.Id == request.Id);

            if (existing == null)
            {
                throw new BusinessRuleException(ApplicationMessage.InvalidId,
                 ApplicationMessage.InvalidId.Message(),
                 ApplicationMessage.InvalidId.UserMessage());
            }

            _bannerLocationRepository.Delete(existing);

            await _dbContextHandler.SaveChangesAsync();
            return new ResponseBase<object>
            {
                Success = true
            };
        }
    }
}
