using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.Domain.ProductAggregate;

using Framework.Core.Model;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.ProductQueries
{
    public class ProductBarcodeControlHandler : IRequestHandler<ProductBarcodeControlQuery, ResponseBase<bool>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductSellerRepository _productSellerRepository;
        public ProductBarcodeControlHandler(IProductRepository productRepository, IProductSellerRepository productSellerRepository)
        {
            _productRepository = productRepository;
            _productSellerRepository = productSellerRepository;
        }

        public async Task<ResponseBase<bool>> Handle(ProductBarcodeControlQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.FindByAsync(x => x.Code == request.Code);
            if (product == null)
                return new ResponseBase<bool>() { Data = false };

            var productSeller = await _productSellerRepository.FindByAsync(y =>
                y.SellerId == request.SellerId && y.ProductId == product.Id);

            if (productSeller == null)
                return new ResponseBase<bool>() { Data = true };

            return new ResponseBase<bool>() { Data = false };
        }
    }
}
