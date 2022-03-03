using Catalog.ApiContract.Request.Command.CategoryCommands;
using Catalog.Domain;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.ProductAggregate;
using Framework.Core.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.CategoryCommands
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ResponseBase<object>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDbContextHandler _dbContextHandler; //unit of work   
        private readonly IProductCategoryRepository _productCategoryRepository;
        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, IDbContextHandler dbContextHandler,
            IProductCategoryRepository productCategoryRepository)
        {
            _categoryRepository = categoryRepository;
            _dbContextHandler = dbContextHandler;
            _productCategoryRepository = productCategoryRepository;
        }


        public async Task<ResponseBase<object>> Handle(DeleteCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var existing = await _categoryRepository.FindByAsync(r => r.Id == request.Id);
            if (existing != null)
            {
                var productCategory = await _productCategoryRepository.FilterByAsync(g => g.CategoryId == request.Id);
                if (productCategory.Count > 0)
                {
                    throw new BusinessRuleException(ApplicationMessage.CategoryNotDelete,
                 ApplicationMessage.CategoryNotDelete.Message(),
                 ApplicationMessage.CategoryNotDelete.UserMessage());
                }

                _categoryRepository.Delete(existing);

                await _dbContextHandler.SaveChangesAsync();

                return new ResponseBase<object> { Success = true };
            }

            return new ResponseBase<object> { Success = false };
        }
    }
}