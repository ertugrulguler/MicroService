using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.Domain;
using Catalog.Domain.ProductAggregate;

using Framework.Core.Model;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.ProductQueries
{
    public class GetProductIdByCodeQueryHandler : IRequestHandler<GetProductIdByCodeQuery, ResponseBase<object>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductIdByCodeQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ResponseBase<object>> Handle(GetProductIdByCodeQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.FindByAsync(x => x.Code == request.Code);

            if (product == null)
                throw new BusinessRuleException(ApplicationMessage.ProductNotFound,
                                        ApplicationMessage.ProductNotFound.Message(),
                                        ApplicationMessage.ProductNotFound.UserMessage());

            return new ResponseBase<object> { Data = product.Id };
        }
    }
}
