using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.Domain;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.ProductAggregate;
using Framework.Core.Model;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.ProductQueries
{
    public class GetProductReturnableQueryHandler : IRequestHandler<GetProductReturnableQuery, ResponseBase<bool>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public GetProductReturnableQueryHandler(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<ResponseBase<bool>> Handle(GetProductReturnableQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductWithCategory(request.Id);
            if (product == null)
            {
                throw new BusinessRuleException(ApplicationMessage.ProductNotFound, ApplicationMessage.ProductNotFound.Message(), ApplicationMessage.ProductNotFound.UserMessage());
            }

            var category = await _categoryRepository.FindByAsync(x => product.ProductCategories.Select(c => c.CategoryId).Contains(x.Id) && x.Type == Domain.Enums.CategoryTypeEnum.MainCategory);

            return new ResponseBase<bool>
            {
                Success = true,
                MessageCode = ApplicationMessage.Success,
                Data = (category?.IsReturnable).GetValueOrDefault()
            };
        }
    }
}
