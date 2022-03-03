using Catalog.ApiContract.Request.Query.BasketQueries;
using Catalog.ApiContract.Response.Query.BasketQueries;
using Catalog.ApplicationService.Communicator.BackOffice;
using Catalog.ApplicationService.Communicator.BackOffice.Model;
using Catalog.Domain;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;
using Catalog.Domain.ProductAggregate;

using Framework.Core.Model;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.BasketQueries
{
    public class GetBasketItemsInstallmentCountQueryHandler : IRequestHandler<GetBasketItemsInstallmentCountQuery, ResponseBase<List<BasketDetailInstallment>>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductSellerRepository _productSellerRepository;
        private readonly ICategoryInstallmentRepository _categoryInstallmentRepository;
        private readonly IBackOfficeCommunicator _backOfficeCommunicator;

        public GetBasketItemsInstallmentCountQueryHandler(IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IProductSellerRepository productSellerRepository,
            ICategoryInstallmentRepository categoryInstallmentRepository,
            IBackOfficeCommunicator backOfficeCommunicator)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _productSellerRepository = productSellerRepository;
            _categoryInstallmentRepository = categoryInstallmentRepository;
            _backOfficeCommunicator = backOfficeCommunicator;
        }

        public async Task<ResponseBase<List<BasketDetailInstallment>>> Handle(GetBasketItemsInstallmentCountQuery request, CancellationToken cancellationToken)
        {
            var basketItemsInstallment = new List<BasketDetailInstallment>();

            foreach (var requestBasketItem in request.BasketProducts)
            {
                try
                {
                    var product = await _productRepository.GetProductByProductSellerId(requestBasketItem.ProductId, requestBasketItem.SellerId);

                    if (product == null || product.ProductSellers.FirstOrDefault() == null)
                    {
                        throw new BusinessRuleException(ApplicationMessage.EmptySellerProducts,
                             ApplicationMessage.EmptySellerProducts.Message(),
                             ApplicationMessage.EmptySellerProducts.UserMessage());
                    }

                    var productSeller = await _productSellerRepository.FindByAsync(x => x.ProductId == requestBasketItem.ProductId && x.SellerId == requestBasketItem.SellerId);
                    var productCategory = await _categoryRepository.FindByAsync(x => product.ProductCategories.Select(c => c.CategoryId).Contains(x.Id) && x.Type == CategoryTypeEnum.MainCategory);
                    var categoryInstallment = await _categoryInstallmentRepository.FindByAsync(x => x.CategoryId == productCategory.Id);
                    var sellerInstallmentCount = await _backOfficeCommunicator.CategoryCompanyInstallment(new CategoryCompanyInstallmentRequest
                    {
                        CategoryId = productCategory.Id,
                        SellerId = productSeller.SellerId
                    });
                    var installmentType = InstallmentType.SellerType;
                    var installmentCount = ArrangeInstallment(new Price(productSeller.SalePrice), sellerInstallmentCount.Data, categoryInstallment, request.CardUserType, request.PaymentType, out installmentType);

                    var basketDetailInstallment = new BasketDetailInstallment
                    {
                        ProductId = product.Id,
                        SellerId = requestBasketItem.SellerId,
                        InstallmentCount = installmentCount,
                        InstallmentType = Enum.GetName(typeof(InstallmentType), installmentType)
                    };

                    basketItemsInstallment.Add(basketDetailInstallment);
                }
                catch (Exception ex)
                {
                    basketItemsInstallment.Add(new BasketDetailInstallment
                    {
                        ProductId = requestBasketItem.ProductId,
                        SellerId = requestBasketItem.SellerId,
                        HasError = true,
                        ErrorMessage = ex.Message
                    });
                }
            }

            return new ResponseBase<List<BasketDetailInstallment>> { Data = basketItemsInstallment, Success = true };
        }

        //TODO: sale price işlemleri yapılacaksa diye gönderildi. For future development.
        private int ArrangeInstallment(Price salePrice, CategoryCompanyInstallmentResponse sellerInstallment, CategoryInstallment categoryInstallment, CardUserType cardUserType, PaymentType paymentType, out InstallmentType installmentType)
        {
            installmentType = InstallmentType.CategoryType;

            if (sellerInstallment == null)
                sellerInstallment = new CategoryCompanyInstallmentResponse { MaxInstallmentCount = 1 };

            var sellerMaxInstallmentCount = sellerInstallment.MaxInstallmentCount;

            if (cardUserType == CardUserType.Commercial && paymentType == PaymentType.DebitCreditCard)
            {
                if (sellerInstallment.UseCommercialCardInstallment)
                    sellerMaxInstallmentCount = sellerInstallment.CommercialCardInstallmentCount.HasValue ? sellerInstallment.CommercialCardInstallmentCount.Value : 1;
            }

            if (categoryInstallment == null || sellerMaxInstallmentCount <= categoryInstallment.MaxInstallmentCount)
            {
                installmentType = InstallmentType.SellerType;
                return sellerMaxInstallmentCount == 0 ? 1 : sellerMaxInstallmentCount;
            }

            return categoryInstallment.MaxInstallmentCount == 0 ? 1 : categoryInstallment.MaxInstallmentCount;
        }
    }
}
