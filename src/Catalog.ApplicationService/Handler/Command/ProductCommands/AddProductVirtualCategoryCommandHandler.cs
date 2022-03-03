using Catalog.ApiContract.Request.Command.ProductCommands;
using Catalog.ApiContract.Response.Command.ProductCommands;
using Catalog.Domain;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;
using Catalog.Domain.ProductAggregate;

using Framework.Core.Model;

using MediatR;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.ProductCommands
{
    public class AddProductVirtualCategoryCommandHandler : IRequestHandler<AddProductVirtualCategoryCommand, ResponseBase<AddProductVirtualCategoryResult>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDbContextHandler _dbContextHandler;


        public AddProductVirtualCategoryCommandHandler(IProductRepository productRepository, IDbContextHandler dbContextHandler,
                                                       IProductCategoryRepository productCategoryRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _dbContextHandler = dbContextHandler;
            _productCategoryRepository = productCategoryRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task<ResponseBase<AddProductVirtualCategoryResult>> Handle(AddProductVirtualCategoryCommand request, CancellationToken cancellationToken)
        {
            var errorList = new List<string>();

            foreach (var item in request.Code)
            {
                var category = await _categoryRepository.FindByAsync(c => c.Id == request.VirtualCategoryId && c.Type == CategoryTypeEnum.VipSellerVirtual);
                if (category == null)
                    throw new BusinessRuleException(ApplicationMessage.NotVirtualCategory,
                    ApplicationMessage.NotVirtualCategory.Message(),
                    ApplicationMessage.NotVirtualCategory.UserMessage());

                var productId = await _productRepository.FindByAsync(x => x.Code == item);
                if (productId == null)
                {
                    errorList.Add(item + " barkoduna ait ürün bulunamadı.");
                }

                else
                {
                    if (request.VirtualCategoryActionType == VirtualCategoryActionType.Create)
                    {
                        var control = await _productCategoryRepository.FilterByAsync(pc => pc.CategoryId == request.VirtualCategoryId && pc.ProductId == productId.Id);
                        if (control.Count > 0) continue;

                        var newProductCategory = new ProductCategory(productId.Id, request.VirtualCategoryId);
                        await _productCategoryRepository.SaveAsync(newProductCategory);
                    }

                    if (request.VirtualCategoryActionType == VirtualCategoryActionType.Delete)
                    {
                        var productCategoryId = await _productCategoryRepository.FindByAsync(p => p.ProductId == productId.Id && p.CategoryId == request.VirtualCategoryId);
                        if (productCategoryId != null)
                        {
                            _productCategoryRepository.Delete(productCategoryId);
                        }
                    }
                }
            }

            await _dbContextHandler.SaveChangesAsync();

            return new ResponseBase<AddProductVirtualCategoryResult>
            {
                Data = new AddProductVirtualCategoryResult
                {
                    Error = errorList
                },

                Success = true
            };

        }

    }
}
