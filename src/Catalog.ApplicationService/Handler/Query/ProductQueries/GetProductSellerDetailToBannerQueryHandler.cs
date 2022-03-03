using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.Domain;
using Catalog.Domain.ProductAggregate;

using Framework.Core.Model;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.ProductQueries
{
    public class GetProductSellerDetailToBannerQueryHandler : IRequestHandler<GetProductSellerDetailToBannerQuery, ResponseBase<ProductSeller>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductSellerRepository _productSellerRepository;

        public GetProductSellerDetailToBannerQueryHandler(IProductRepository productRepository, IProductSellerRepository productSellerRepository)
        {
            _productRepository = productRepository;
            _productSellerRepository = productSellerRepository;
        }

        public async Task<ResponseBase<ProductSeller>> Handle(GetProductSellerDetailToBannerQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.FindByAsync(x => x.Code == request.Code);

            if (product == null)
                throw new BusinessRuleException(ApplicationMessage.ProductNotFound,
                                        ApplicationMessage.ProductNotFound.Message(),
                                        ApplicationMessage.ProductNotFound.UserMessage());

            var productSeller = await _productSellerRepository.FindByAsync(x => x.SellerId == request.SellerId && x.ProductId == product.Id);

            if (productSeller == null)
                throw new BusinessRuleException(ApplicationMessage.ProductNotFound,
                                        ApplicationMessage.ProductNotFound.Message(),
                                        ApplicationMessage.ProductNotFound.UserMessage());

            return new ResponseBase<ProductSeller> { Data = productSeller };
        }
    }
}
