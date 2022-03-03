using System;
using Catalog.ApiContract.Request.Command.ProductCommands;
using Catalog.Domain;
using Catalog.Domain.ProductAggregate;

using Framework.Core.Model;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.ProductCommands
{
    public class CreateProductOneChannelCommandHandler : IRequestHandler<CreateProductOneChannelCommand, ResponseBase<object>>
    {
        private readonly IProductChannelRepository _productChannelRepository;
        private readonly IProductRepository _productRepository;
        private readonly IDbContextHandler _dbContextHandler;


        public CreateProductOneChannelCommandHandler(IDbContextHandler dbContextHandler, IProductChannelRepository productChannelRepository, IProductRepository productRepository)
        {
            _dbContextHandler = dbContextHandler;
            _productChannelRepository = productChannelRepository;
            _productRepository = productRepository;
        }

        public async Task<ResponseBase<object>> Handle(CreateProductOneChannelCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.FindByAsync(p => p.Code == request.ProductCode);
            if (product == null)
                throw new BusinessRuleException(ApplicationMessage.ProductNotFound,
                ApplicationMessage.ProductNotFound.Message(),
                ApplicationMessage.ProductNotFound.UserMessage());



            var productChannel = await _productChannelRepository.FindByAsync(pc => pc.ProductId == product.Id && pc.ChannelCode == request.ProductChannelCode.GetHashCode());
            if (productChannel != null)
            {
                return new ResponseBase<object>()
                {
                    Data = product.Id,
                    Success = true,
                    UserMessage = ApplicationMessage.ProductWithChannelAlreadyExist.UserMessage()
                };
                
            }

            var newProductChannel = new ProductChannel(product.Id, (int)request.ProductChannelCode);
            await _productChannelRepository.SaveAsync(newProductChannel);

            await _dbContextHandler.SaveChangesAsync();
            return new ResponseBase<object>()
            {
                Data = $"{product.Id} idli ürüne {newProductChannel.ChannelCode} channel eklendi.",
                Success = true
            };
        }
    }
}