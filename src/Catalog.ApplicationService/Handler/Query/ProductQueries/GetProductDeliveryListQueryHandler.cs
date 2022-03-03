using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.ApplicationService.Assembler;
using Catalog.ApplicationService.Communicator.BackOffice;
using Catalog.ApplicationService.Communicator.BackOffice.Model;
using Catalog.Domain;
using Catalog.Domain.ProductAggregate;
using Framework.Core.Model;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.ProductQueries
{
    public class GetProductDeliveryListQueryHandler : IRequestHandler<GetProductDeliveryListQuery, ResponseBase<GetProductDelivery>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductAssembler _productAssembler;
        private readonly IBackOfficeCommunicator _backOfficeCommunicator;


        public GetProductDeliveryListQueryHandler(IProductRepository productRepository, IProductAssembler productAssembler, IBackOfficeCommunicator backOfficeCommunicator)
        {
            _productRepository = productRepository;
            _productAssembler = productAssembler;
            _backOfficeCommunicator = backOfficeCommunicator;
        }

        public async Task<ResponseBase<GetProductDelivery>> Handle(GetProductDeliveryListQuery request, CancellationToken cancellationToken)
        {
            var response = new GetProductDelivery();
            response.ProductDeliveries = new List<ProductDeliveries>();

            var products = new List<Product>();
            var categoryInstallmentList = new List<CategoryCompanyInstallmentResponse>();

            foreach (var item in request.GetProductDeliveryInfos)
            {
                var productItem = await _productRepository.GetProductByProductSellerId(item.ProductId, item.SellerId);

                if (productItem == null || productItem.ProductDeliveries.Count == 0)
                {
                    var deliveries = new ProductDeliveries
                    {
                        HasError = true,
                        ErrorMessage = ApplicationMessage.EmptySellerProducts.Message(),
                        SellerId = item.SellerId,
                        ProductId = item.ProductId
                    };
                    response.ProductDeliveries.Add(deliveries);
                    continue;
                }

                var sellerInstallmentCount = await _backOfficeCommunicator.CategoryCompanyInstallment(new CategoryCompanyInstallmentRequest
                {
                    CategoryId = productItem.ProductCategories.FirstOrDefault().CategoryId,
                    SellerId = item.SellerId
                });
                if (sellerInstallmentCount != null && sellerInstallmentCount.Data != null)
                    categoryInstallmentList.Add(sellerInstallmentCount.Data);
                products.Add(productItem);
            }

            var productResponse = _productAssembler.MapToGetProductDeliveryListQueryResult(products, categoryInstallmentList);
            response.ProductDeliveries.AddRange(productResponse.Data.ProductDeliveries);
            return new ResponseBase<GetProductDelivery> { Data = response, Success = true };
        }
    }
}
