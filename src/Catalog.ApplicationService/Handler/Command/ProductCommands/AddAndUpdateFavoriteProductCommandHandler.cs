using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Request.Command.ProductCommands;
using Catalog.ApplicationService.Assembler;
using Catalog.Domain;
using Catalog.Domain.ProductAggregate;
using Framework.Core.Authorization;
using Framework.Core.Model;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.ProductCommands
{
    public class AddAndUpdateFavoriteProductCommandHandler : IRequestHandler<AddAndUpdateFavoriteProductCommand, ResponseBase<FavoriteProductDto>>
    {
        private readonly IFavoriteProductRepository _favoriteProductRepository;
        private readonly IDbContextHandler _dbContextHandler;
        private readonly IProductAssembler _productAssembler;
        private readonly IIdentityContext _identityContext;

        public AddAndUpdateFavoriteProductCommandHandler(IFavoriteProductRepository favoriteProductRepository, IDbContextHandler dbContextHandler, IProductAssembler productAssembler, IIdentityContext identityContext)
        {
            _dbContextHandler = dbContextHandler;
            _favoriteProductRepository = favoriteProductRepository;
            _productAssembler = productAssembler;
            _identityContext = identityContext;
        }

        public async Task<ResponseBase<FavoriteProductDto>> Handle(AddAndUpdateFavoriteProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id != null)
                {
                    var existingFavorite = await _favoriteProductRepository.FindByAsync(y => y.Id == request.Id);
                    if (existingFavorite != null)
                    {
                        existingFavorite.SetFavoriteProduct(request.IsActive);
                        _favoriteProductRepository.Update(existingFavorite);
                        await _dbContextHandler.SaveChangesAsync();
                        return _productAssembler.MapToCreateAndUpdateFavoriteProductCommandResult(existingFavorite);
                    }
                    throw new BusinessRuleException(ApplicationMessage.InvalidId,
                                                    ApplicationMessage.InvalidId.Message(),
                                                    ApplicationMessage.InvalidId.UserMessage());
                }
                var customerId = _identityContext.GetUserInfo().Id;
                var query = await _favoriteProductRepository.FindByAsync(y => y.CustomerId == customerId && y.ProductId == request.ProductId);
                if (query == null)
                {
                    var favorite = new FavoriteProduct(request.ProductId, customerId, request.IsActive);
                    await _favoriteProductRepository.SaveAsync(favorite);
                    await _dbContextHandler.SaveChangesAsync();
                    return _productAssembler.MapToCreateAndUpdateFavoriteProductCommandResult(favorite);
                }
                else
                {
                    query.SetFavoriteProduct(request.IsActive);
                    _favoriteProductRepository.Update(query);
                    await _dbContextHandler.SaveChangesAsync();
                    return _productAssembler.MapToCreateAndUpdateFavoriteProductCommandResult(query);
                }
            }
            catch (Exception e)
            {
                return new ResponseBase<FavoriteProductDto> { Data = null, Success = false, Message = ApplicationMessage.FavoriteListNotAdded.Message() };
            }
        }
    }
}
