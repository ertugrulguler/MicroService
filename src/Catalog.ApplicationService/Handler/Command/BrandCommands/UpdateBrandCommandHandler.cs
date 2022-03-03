using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Request.Command.BrandCommands;
using Catalog.ApplicationService.Assembler;
using Catalog.Domain;
using Catalog.Domain.BrandAggregate;

using Framework.Core.Model;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.BrandCommands
{
    public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, ResponseBase<BrandDto>>
    {

        private readonly IBrandRepository _brandRepository;
        private readonly IDbContextHandler _dbContextHandler;
        private readonly IBrandAssembler _brandAssembler;
        private readonly IGeneralAssembler _generalAssembler;

        public UpdateBrandCommandHandler(IBrandRepository brandRepository, IGeneralAssembler generalAssembler, IDbContextHandler dbContextHandler, IBrandAssembler brandAssembler)
        {
            _brandRepository = brandRepository;
            _dbContextHandler = dbContextHandler;
            _brandAssembler = brandAssembler;
            _generalAssembler = generalAssembler;
        }

        public async Task<ResponseBase<BrandDto>> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var existing = await _brandRepository.FindByAsync(c => c.Id == request.Id);
            if (existing != null)
            {
                var seoName = _generalAssembler.GetSeoName(request.Name, Domain.Enums.SeoNameType.Brand);
                existing.SetBrand(request.Name, request.LogoUrl, request.Website, seoName);
                existing.IsActive = request.Status;
                _brandRepository.Update(existing);
                await _dbContextHandler.SaveChangesAsync();
                return _brandAssembler.MapToUpdateBrandCommandResult(existing);
            }
            throw new BusinessRuleException(ApplicationMessage.InvalidId,
            ApplicationMessage.InvalidId.Message(),
            ApplicationMessage.InvalidId.UserMessage());
        }
    }
}