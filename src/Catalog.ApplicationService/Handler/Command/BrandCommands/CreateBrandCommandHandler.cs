using Catalog.ApiContract.Request.Command.BrandCommands;
using Catalog.ApiContract.Response.Command.BrandCommands;
using Catalog.ApplicationService.Assembler;
using Catalog.Domain;
using Catalog.Domain.BrandAggregate;

using Framework.Core.Model;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.BrandCommands
{
    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, ResponseBase<CreateBrand>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IDbContextHandler _dbContextHandler;
        private readonly IBrandAssembler _brandAssembler;
        private readonly IGeneralAssembler _genarelAssembler;
        public CreateBrandCommandHandler(IBrandRepository brandRepository, IDbContextHandler dbContextHandler,
            IBrandAssembler brandAssembler, IGeneralAssembler genarelAssembler)
        {
            _brandRepository = brandRepository;
            _dbContextHandler = dbContextHandler;
            _brandAssembler = brandAssembler;
            _genarelAssembler = genarelAssembler;
        }

        public async Task<ResponseBase<CreateBrand>> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            var existing = await _brandRepository.FindByAsync(x => x.Name == request.Name);

            if (existing == null)
            {
                var seoName = _genarelAssembler.GetSeoName(request.Name, Domain.Enums.SeoNameType.Brand);
                var Brand = new Brand(request.Name, request.LogoUrl, request.Website, seoName);
                await _brandRepository.SaveAsync(Brand);
                await _dbContextHandler.SaveChangesAsync();
                return _brandAssembler.MapToCreateBrandCommandResult(Brand);
            }

            throw new BusinessRuleException(ApplicationMessage.BrandAlreadyExist,
            ApplicationMessage.BrandAlreadyExist.Message(),
            ApplicationMessage.BrandAlreadyExist.UserMessage());

        }
    }
}
