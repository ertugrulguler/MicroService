using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Request.Command.ProductCommands;
using Catalog.ApplicationService.Assembler;
using Catalog.Domain;
using Catalog.Domain.ProductAggregate;

using Framework.Core.Model;

using MediatR;

using System.Threading;
using System.Threading.Tasks;


namespace Catalog.ApplicationService.Handler.Command.ProductCommands
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ResponseBase<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IDbContextHandler _dbContextHandler;
        private readonly IProductAssembler _productAssembler;


        public DeleteProductCommandHandler(IProductRepository productRepository, IDbContextHandler dbContextHandler, IProductAssembler productAssembler)
        {
            _productRepository = productRepository;
            _dbContextHandler = dbContextHandler;
            _productAssembler = productAssembler;

        }
        public async Task<ResponseBase<ProductDto>> Handle(DeleteProductCommand request,
            CancellationToken cancellationToken)
        {
            var existing = await _productRepository.FindByAsync(w => w.Id == request.Id);
            if (existing == null)
            {
                throw new BusinessRuleException(ApplicationMessage.InvalidProductId,
                         ApplicationMessage.InvalidProductId.Message(),
                         ApplicationMessage.InvalidProductId.UserMessage());
            }
            _productRepository.Delete(existing);

            await _dbContextHandler.SaveChangesAsync();
            return _productAssembler.MapToDeleteProductCommandResult(existing);

        }

    }

}
