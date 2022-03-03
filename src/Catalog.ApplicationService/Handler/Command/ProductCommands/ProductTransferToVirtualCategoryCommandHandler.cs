using Catalog.ApiContract.Request.Command.ProductCommands;
using Catalog.Domain;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;
using Catalog.Domain.ProductAggregate;

using Framework.Core.Model;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.ProductCommands
{
    public class ProductTransferToVirtualCategoryCommandHandler : IRequestHandler<ProductTransferToVirtualCategoryCommand, ResponseBase<object>>
    {

        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDbContextHandler _dbContextHandler;

        public ProductTransferToVirtualCategoryCommandHandler(
            IDbContextHandler dbContextHandler,
            IProductCategoryRepository productCategoryRepository,
            ICategoryRepository categoryRepository
            )
        {
            _dbContextHandler = dbContextHandler;
            _productCategoryRepository = productCategoryRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<ResponseBase<object>> Handle(ProductTransferToVirtualCategoryCommand request, CancellationToken cancellationToken)
        {
            var isVirtualCategory = await _categoryRepository.FindByAsync(c => c.Id == request.CategoryId);
            if (isVirtualCategory.Type == CategoryTypeEnum.MainCategory)
            {
                throw new BusinessRuleException(ApplicationMessage.CategoryIsNotVirtual,
                    ApplicationMessage.CategoryIsNotVirtual.Message(),
                    ApplicationMessage.CategoryIsNotVirtual.UserMessage());
            }
            foreach (var productId in request.ProductIdList)
            {
                var existingProductCategories = await _productCategoryRepository.FilterByAsync(p =>
                    p.CategoryId == request.CategoryId && p.ProductId == productId);

                if (existingProductCategories.Count == 0)
                {
                    var transferredProduct = new ProductCategory(productId, request.CategoryId);

                    await _productCategoryRepository.SaveAsync(transferredProduct);
                }

            }
            await _dbContextHandler.SaveChangesAsync();
            return new ResponseBase<object> { Success = true };
        }
    }
}
