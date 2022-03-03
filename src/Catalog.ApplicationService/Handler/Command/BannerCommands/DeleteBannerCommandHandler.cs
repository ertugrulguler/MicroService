using Catalog.ApiContract.Request.Command.BannerCommands;
using Catalog.ApplicationService.Assembler;
using Catalog.Domain;
using Catalog.Domain.BannerAggregate;

using Framework.Core.Model;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.BannerCommands
{
    public class DeleteBannerCommandHandler : IRequestHandler<DeleteBannerCommand, ResponseBase<object>>
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IBannerFiltersRepository _bannerFiltersRepository;
        private readonly IDbContextHandler _dbContextHandler;
        private readonly IBannerAssembler _bannerAssembler;

        public DeleteBannerCommandHandler(IBannerRepository bannerRepository, IDbContextHandler dbContextHandler, IBannerAssembler bannerAssembler, IBannerFiltersRepository bannerFiltersRepository)
        {
            _bannerRepository = bannerRepository;
            _dbContextHandler = dbContextHandler;
            _bannerAssembler = bannerAssembler;
            _bannerFiltersRepository = bannerFiltersRepository;
        }
        public async Task<ResponseBase<object>> Handle(DeleteBannerCommand request, CancellationToken cancellationToken)
        {
            var banner = await _bannerRepository.FindByAsync(w => w.Id == request.Id);

            if (banner == null)
                throw new BusinessRuleException(ApplicationMessage.InvalidId,
                ApplicationMessage.InvalidId.Message(),
                ApplicationMessage.InvalidId.UserMessage());

            var bannerFilters = await _bannerFiltersRepository.FilterByAsync(bf => bf.BannerId == request.Id);
            if (bannerFilters.Count > 0)
            {
                foreach (var bannerFilter in bannerFilters)
                {
                    _bannerFiltersRepository.Delete(bannerFilter);
                }
            }

            _bannerRepository.Delete(banner);
            await _dbContextHandler.SaveChangesAsync();

            return new ResponseBase<object>()
            {
                Success = true
            };
        }
    }
}
