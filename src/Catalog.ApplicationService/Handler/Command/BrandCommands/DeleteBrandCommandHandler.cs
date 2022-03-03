using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Request.Command.BrandCommands;
using Catalog.ApplicationService.Assembler;
using Catalog.Domain;
using Catalog.Domain.BrandAggregate;
using Catalog.Domain.ProductAggregate;

using Framework.Core.Model;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.BrandCommands
{
    public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand, ResponseBase<BrandDto>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IDbContextHandler _dbContextHandler;
        private readonly IBrandAssembler _brandAssembler;
        private readonly IProductRepository _productRepository;

        public DeleteBrandCommandHandler(IBrandRepository brandRepository, IDbContextHandler dbContextHandler, IProductRepository productRepository,
            IBrandAssembler brandAssembler)
        {
            _brandRepository = brandRepository;
            _dbContextHandler = dbContextHandler;
            _brandAssembler = brandAssembler;
            _productRepository = productRepository;
        }

        public async Task<ResponseBase<BrandDto>> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            var existing = await _brandRepository.FindByAsync(w => w.Id == request.Id);

            if (existing == null)
            {
                throw new BusinessRuleException(ApplicationMessage.InvalidId,
                 ApplicationMessage.InvalidId.Message(),
                 ApplicationMessage.InvalidId.UserMessage());
            }

            var product = await _productRepository.FindByAsync(x => x.BrandId == request.Id);

            if (product != null)
            {
                throw new BusinessRuleException(ApplicationMessage.AlreadyProductBrandId,
                ApplicationMessage.AlreadyProductBrandId.Message(),
                ApplicationMessage.AlreadyProductBrandId.UserMessage());
            }

            _brandRepository.Delete(existing);

            await _dbContextHandler.SaveChangesAsync();
            return _brandAssembler.MapToDeleteBrandCommandResult(existing);


        }

    }

}
