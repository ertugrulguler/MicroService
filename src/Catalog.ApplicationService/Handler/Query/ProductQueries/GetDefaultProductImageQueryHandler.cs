using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.ApplicationService.Communicator.Merchant;
using Catalog.ApplicationService.Communicator.Merchant.Model;
using Catalog.ApplicationService.Handler.Services;
using Catalog.Domain.ProductAggregate;
using Framework.Core.Model;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.ProductQueries
{
    public class GetDefaultProductImageQueryHandler : IRequestHandler<GetDefaultProductImageQuery, ResponseBase<GetDefaultProductImage>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IMerhantCommunicator _merchantCommunicator;
        private readonly IProductService _productService;
        public GetDefaultProductImageQueryHandler(IProductRepository productRepository, IProductImageRepository productImageRepository, IMerhantCommunicator merchantCommunicator, IProductService productService)
        {
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
            _merchantCommunicator = merchantCommunicator;
            _productService = productService;
        }
        public async Task<ResponseBase<GetDefaultProductImage>> Handle(GetDefaultProductImageQuery request,
            CancellationToken cancellationToken)
        {
            GetDefaultProductImage result;
            var sellerName = string.Empty;
            var productName = string.Empty;

            var seller = await _merchantCommunicator.GetSellerById(new GetSellerRequest { SellerId = request.SellerId });
            if (seller.Data != null)
            {
                sellerName = !string.IsNullOrEmpty(seller.Data.CompanyName) ? seller.Data.CompanyName : seller.Data.FirmName;
            }

            var product = await _productRepository.FindByAsync(x => x.Id == request.ProductId);

            if (product != null)
                productName = product.Name;

            var defaultImage = await _productImageRepository.FilterByAsync(x => x.ProductId == request.ProductId && x.SellerId == request.SellerId && x.IsDefault && x.IsActive);
            if (defaultImage.Count > 0)
                result = new GetDefaultProductImage { ImageUrl = defaultImage.FirstOrDefault()?.Url, SellerName = sellerName, ProductName = productName, ProductSeoUrl = _productService.GetProductSeoUrl(request.ProductId).Result };
            else
            {
                var firstImage = await _productImageRepository.FilterByAsync(x => x.ProductId == request.ProductId && x.SellerId == request.SellerId && x.IsActive);
                result = new GetDefaultProductImage { ImageUrl = firstImage.FirstOrDefault()?.Url, SellerName = sellerName, ProductName = productName, ProductSeoUrl = _productService.GetProductSeoUrl(request.ProductId).Result };
            }



            return new ResponseBase<GetDefaultProductImage>
            {
                Data = result,
                Success = true
            };
        }
    }
}