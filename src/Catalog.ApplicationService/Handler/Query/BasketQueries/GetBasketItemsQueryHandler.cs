using Catalog.ApiContract.Request.Query.BasketQueries;
using Catalog.ApiContract.Response.Query.BasketQueries;
using Catalog.ApplicationService.Communicator.BackOffice;
using Catalog.ApplicationService.Communicator.BackOffice.Model;
using Catalog.ApplicationService.Communicator.Merchant;
using Catalog.ApplicationService.Handler.Services;
using Catalog.Domain;
using Catalog.Domain.AttributeAggregate;
using Catalog.Domain.BrandAggregate;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;
using Catalog.Domain.ProductAggregate;

using Framework.Core.Helpers.Abstract;
using Framework.Core.Model;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.BasketQueries
{
    public class GetBasketItemsQueryHandler : IRequestHandler<GetBasketItemsQuery, ResponseBase<List<BasketDetail>>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryAttributeRepository _categoryAttributeRepository;
        private readonly IAttributeValueRepository _attributeValueRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductSellerRepository _productSellerRepository;
        private readonly IBackOfficeCommunicator _backOfficeCommunicator;
        private readonly IMerhantCommunicator _merhantCommunicator;
        private readonly IBrandRepository _brandRepository;
        private readonly IProductService _productService;
        private readonly ICustomerHelper _customerHelper;
        private readonly IDiscountService _discountService;

        public GetBasketItemsQueryHandler(IProductRepository productRepository,
            ICategoryAttributeRepository categoryAttributeRepository,
            IAttributeValueRepository attributeValueRepository,
            ICategoryRepository categoryRepository,
            IProductSellerRepository productSellerRepository,
            IBackOfficeCommunicator backOfficeCommunicator,
            IMerhantCommunicator merhantCommunicator,
            IBrandRepository brandRepository, IProductService productService, ICustomerHelper customerHelper, IDiscountService discountService)
        {
            _productRepository = productRepository;
            _categoryAttributeRepository = categoryAttributeRepository;
            _attributeValueRepository = attributeValueRepository;
            _categoryRepository = categoryRepository;
            _productSellerRepository = productSellerRepository;
            _backOfficeCommunicator = backOfficeCommunicator;
            _merhantCommunicator = merhantCommunicator;
            _brandRepository = brandRepository;
            _productService = productService;
            _customerHelper = customerHelper;
            _discountService = discountService;
        }

        public async Task<ResponseBase<List<BasketDetail>>> Handle(GetBasketItemsQuery request, CancellationToken cancellationToken)
        {
            var channel = _customerHelper.GetChannel();

            var basketItems = new List<BasketDetail>();
            var bannedSellers = (await _merhantCommunicator.GetBannedSellers()).Data;

            foreach (var requestBasketItem in request.BasketProducts.Where(requestBasketItem => !bannedSellers.Contains(requestBasketItem.SellerId)))
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
                    var useMile = await _backOfficeCommunicator.CategoryCompanyMile(new CategoryCompanyMileRequest { CategoryId = productCategory.Id, SellerId = productSeller.SellerId });
                    var brand = await _brandRepository.GetByIdAsync(product.BrandId);

                    #region Image
                    var productImage = "No image";
                    var productDefaultImage = product.ProductImages.FirstOrDefault(x => x.IsDefault);
                    var productFirstImage = product.ProductImages.FirstOrDefault();

                    if (productDefaultImage != null)
                        productImage = productDefaultImage.Url;
                    else if (productFirstImage != null)
                        productImage = productFirstImage.Url;
                    #endregion

                    var coupon = await _discountService.GetDiscountResult(productSeller.SellerId,
                        productSeller.ProductId, productSeller.SalePrice, productSeller.ListPrice, channel);

                    decimal discountAmount = 0;
                    if (coupon != null && coupon.IsDiscounted)
                    {
                        discountAmount = requestBasketItem.Quantity * (productSeller.SalePrice - coupon.DiscountedAmount);
                        productSeller.SetSalePrice(coupon.DiscountedAmount);
                    }

                    var basketDetail = new BasketDetail
                    {
                        ProductId = product.Id,
                        SellerId = productSeller.SellerId,
                        CategoryId = productCategory.Id,
                        CategoryName = productCategory.DisplayName,
                        IsReturnable = productCategory.IsReturnable,
                        BrandName = brand.Name,
                        Barcode = product.Code,
                        ProductName = product.DisplayName,
                        ProductImageUrl = productImage,
                        ListPrice = new Price(productSeller.ListPrice),
                        SalePrice = new Price(productSeller.SalePrice),
                        ExternalDiscountAmount = discountAmount,
                        Desi = product.Desi,
                        VatRate = product.VatRate,
                        VatPrice = new Price(productSeller.SalePrice * product.VatRate / 100),
                        StockCode = productSeller.StockCode,
                        UseMile = useMile.Data != null,
                        MinRequiredAmountForMile = useMile.Data == null ? new Price(0) : new Price(useMile.Data.MinMileAmount),
                        HasError = false,
                        ErrorMessage = string.Empty,
                        ProductUrl = await _productService.GetProductSeoUrl(product.Id)
                    };

                    if (productSeller.StockCount < requestBasketItem.Quantity)
                    {
                        basketDetail.StockCount = 0;
                        basketItems.Add(basketDetail);
                        continue;
                    }

                    var attributeValues = new List<AttributeValue>();
                    var variantableAttributes = await _categoryAttributeRepository.FilterByAsync(x => product.ProductAttributes.Select(x => x.AttributeId).Contains(x.AttributeId) && x.IsVariantable
                    && product.ProductCategories.Select(c => c.CategoryId).Contains(x.CategoryId));

                    if (variantableAttributes != null && variantableAttributes.Count > 0)
                    {
                        var variantableAttributeValues = product.ProductAttributes.Where(x => variantableAttributes.Select(va => va.AttributeId).Contains(x.AttributeId));
                        attributeValues.AddRange(await _attributeValueRepository.FilterByAsync(x => variantableAttributeValues.Select(va => va.AttributeValueId).Contains(x.Id)));
                    }

                    basketDetail.VariantOptionDisplay = string.Join("/", attributeValues.Select(x => x.Value));
                    basketDetail.StockCount = productSeller.StockCount;
                    basketItems.Add(basketDetail);
                }
                catch (Exception ex)
                {
                    basketItems.Add(new BasketDetail
                    {
                        ProductId = requestBasketItem.ProductId,
                        SellerId = requestBasketItem.SellerId,
                        HasError = true,
                        ErrorMessage = ex.Message
                    });
                }
            }

            return new ResponseBase<List<BasketDetail>> { Data = basketItems, Success = true };
        }


    }
}
