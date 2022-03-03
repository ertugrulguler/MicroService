using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.ApplicationService.Assembler;
using Catalog.ApplicationService.Communicator.BackOffice;
using Catalog.ApplicationService.Communicator.BackOffice.Model;
using Catalog.ApplicationService.Communicator.Contract;
using Catalog.ApplicationService.Communicator.Contract.Model;
using Catalog.ApplicationService.Communicator.Merchant;
using Catalog.ApplicationService.Communicator.Merchant.Model;
using Catalog.ApplicationService.Communicator.Parameter;
using Catalog.ApplicationService.Handler.Services;
using Catalog.Domain;
using Catalog.Domain.AttributeAggregate;
using Catalog.Domain.BrandAggregate;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.CouponAggregate;
using Catalog.Domain.Enums;
using Catalog.Domain.Helpers;
using Catalog.Domain.ProductAggregate;
using Catalog.Domain.ProductAggregate.ServiceModels;

using Framework.Core.Authorization;
using Framework.Core.Helpers.Abstract;
using Framework.Core.Model;
using Framework.Core.Model.Enums;

using MediatR;

using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Catalog.ApplicationService.Handler.Query.ProductQueries
{
    public class GetProductDetailQueryHandler : IRequestHandler<GetProductDetailQuery, ResponseBase<GetProductDetailResponse>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductAssembler _productAssembler;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IAttributeRepository _attributeRepository;
        private readonly IAttributeValueRepository _attributeValueRepository;
        private readonly IProductSellerRepository _productSellerRepository;
        private readonly ICategoryAttributeRepository _categoryAttributeRepository;
        private readonly IMerhantCommunicator _merchantCommunicator;
        private readonly IParameterCommunicator _parameterCommunicator;
        private readonly IProductVariantService _productVariantService;
        private readonly ICategoryInstallmentRepository _categoryInstallmentRepository;
        private readonly IBackOfficeCommunicator _backOfficeCommunicator;
        private readonly IIdentityContext _identityContext;
        private readonly IFavoriteProductRepository _favoriteProductRepository;
        private readonly IConfiguration _configuration;
        private readonly IContractCommunicator _contractCommunicator;
        private readonly IMerhantCommunicator _merhantCommunicator;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IProductService _productService;
        private readonly ICustomerHelper _customerHelper;
        private readonly IDiscountService _discountService;
        public GetProductDetailQueryHandler(IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IProductAssembler productAssembler,
            IBrandRepository brandRepository,
            IAttributeRepository attributeRepository,
            IAttributeValueRepository attributeValueRepository,
            IProductSellerRepository productSellerRepository,
            ICategoryAttributeRepository categoryAttributeRepository,
            IMerhantCommunicator merchantCommunicator,
            IParameterCommunicator parameterCommunicator,
            IProductVariantService productVariantService,
            ICategoryInstallmentRepository categoryInstallmentRepository,
            IBackOfficeCommunicator backOfficeCommunicator,
            IIdentityContext identityContext,
            IFavoriteProductRepository favoriteProductRepository, IProductImageRepository productImageRepository,
            IConfiguration configuration,
            IContractCommunicator contractCommunicator, IMerhantCommunicator merhantCommunicator, IProductService productService, ICustomerHelper customerHelper, IDiscountService discountService)
        {
            _productRepository = productRepository;
            _productAssembler = productAssembler;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _attributeRepository = attributeRepository;
            _attributeValueRepository = attributeValueRepository;
            _productSellerRepository = productSellerRepository;
            _categoryAttributeRepository = categoryAttributeRepository;
            _merchantCommunicator = merchantCommunicator;
            _parameterCommunicator = parameterCommunicator;
            _productVariantService = productVariantService;
            _categoryInstallmentRepository = categoryInstallmentRepository;
            _backOfficeCommunicator = backOfficeCommunicator;
            _identityContext = identityContext;
            _favoriteProductRepository = favoriteProductRepository;
            _configuration = configuration;
            _contractCommunicator = contractCommunicator;
            _merhantCommunicator = merhantCommunicator;
            _productService = productService;
            _customerHelper = customerHelper;
            _productImageRepository = productImageRepository;
            _discountService = discountService;
        }

        public async Task<ResponseBase<GetProductDetailResponse>> Handle(GetProductDetailQuery request, CancellationToken cancellationToken)
        {
            var channel = _customerHelper.GetChannel();
            var breadCrumbs = new List<Breadcrumb>();

            CouponServiceResponse coupon = null;

            if (request.Url != null)
            {
                Uri myUri = new Uri(request.Url);
                var magaza = HttpUtility.ParseQueryString(myUri.Query).Get("magaza");

                var productCode = myUri.Segments[1].Substring(myUri.Segments[1].IndexOf("-p-") + 3).ToLower();
                var productToId = await _productRepository.GetProductWithAllRelations(productCode);
                request.ProductId = productToId.Id;

                if (magaza != null)
                {
                    var sellerId = await _merhantCommunicator.GetSellerBySeoName(magaza);
                    if (sellerId.Data == null)
                    {
                        var tempProduct = await _productRepository.GetProductWithAllRelations(productCode);
                        request.SellerId = tempProduct.ProductSellers.OrderBy(a => a.SalePrice).ToList().FirstOrDefault().SellerId;

                    }
                    else
                        request.SellerId = sellerId.Data.SellerId;
                }
                else
                {
                    var tempProduct = await _productRepository.GetProductWithAllRelations(productCode);
                    request.SellerId = tempProduct.ProductSellers.OrderBy(a => a.SalePrice).ToList().FirstOrDefault().SellerId;
                }

                var productBrand = await _brandRepository.GetByIdAsync(productToId.BrandId);

                //1
                var catList = new List<Category>();

                var leafCategory = await _categoryRepository.FindByAsync(c => c.Id == productToId.ProductCategories.FirstOrDefault().CategoryId);
                catList.Add(leafCategory);
                var parentId = leafCategory.ParentId;
                while (parentId != null)
                {
                    var parentCategories = await _categoryRepository.FindByAsync(c => c.Id == parentId);
                    catList.Add(parentCategories);
                    if (parentCategories.ParentId == null)
                    {
                        break;
                    }

                    parentId = parentCategories.ParentId;

                }

                foreach (var category in catList)
                {
                    if (catList.First() != category)
                    {
                        breadCrumbs.Add(new Breadcrumb
                        {
                            CategoryId = category.Id,
                            Name = category.DisplayName,
                            Url = category.SeoName + "-k-" + category.Code
                        });
                    }
                    else
                    {
                        breadCrumbs.Add(new Breadcrumb
                        {
                            CategoryId = category.Id,
                            Name = productBrand.Name + " " + category.DisplayName,
                            Url = productBrand.SeoName + "/" + category.SeoName + "-k-" + category.Code
                        });
                    }

                }

            }

            var breadCrumbsReversed = Enumerable.Reverse(breadCrumbs).ToList();
            var productSeller = await _productSellerRepository.FindByAsync(x => x.SellerId == request.SellerId && x.ProductId == request.ProductId);

            if (productSeller == null)
                throw new BusinessRuleException(ApplicationMessage.InvalidId,
                 ApplicationMessage.InvalidId.Message(),
                 ApplicationMessage.InvalidId.UserMessage());

            var product = await _productRepository.GetProductWithAllRelations(request.ProductId);

            if (product == null)
                throw new BusinessRuleException(ApplicationMessage.ProductNotFound,
                 ApplicationMessage.ProductNotFound.Message(),
                 ApplicationMessage.ProductNotFound.UserMessage());

            var mainProductSellerName = await GetSellerName(request.SellerId);
            var mainProductSellerSeoName = _merhantCommunicator.GetSellerDetailByIds(new GetSellerDetailByIdsRequest { SellerId = new List<Guid> { request.SellerId } }).Result.Data.FirstOrDefault().SellerSeoName;
            var attributes = await _attributeRepository.FilterByAsync(z => product.ProductAttributes.Select(xx => xx.AttributeId).Contains(z.Id));
            var attributeValues = await _attributeValueRepository.FilterByAsync(x => product.ProductAttributes.Select(pa => pa.AttributeValueId).Contains(x.Id));
            var categories = await _categoryRepository.FilterByAsync(z => product.ProductCategories.Select(xx => xx.CategoryId).Contains(z.Id));
            var brand = await _brandRepository.FindByAsync(z => z.Id == product.BrandId);
            var categoryAttributes = await _categoryAttributeRepository.FilterByAsync(z => product.ProductCategories.Select(xx => xx.CategoryId).Contains(z.CategoryId)
            && attributes.Select(aa => aa.Id).Contains(z.AttributeId) && z.IsVariantable);
            var productCategory = categories.FirstOrDefault(x => x.Type == CategoryTypeEnum.MainCategory);

            #region Images

            product.ArrangeProductImagesBySellerId(productSeller.SellerId);

            #endregion

            #region Variant

            var groupedProductGroups = new List<List<ProductVariantGroup>>();
            var catId = productCategory.Id;
            var variantGroups = await _productVariantService.GetProductWithVariants(product.Id, catId);

            if (variantGroups.Count > 0)
                groupedProductGroups = await ArrangeVariants(variantGroups, channel, request.SellerId);

            #endregion

            #region Discount

            var discount = await _discountService.GetDiscountResult(productSeller.SellerId, productSeller.ProductId,
                productSeller.SalePrice, productSeller.ListPrice,channel);

            if (discount!=null && discount.IsDiscounted)
            {
                productSeller.SetSalePrice(discount.DiscountedAmount);
            }
            #endregion

            #region Installments

            var categoryInstallment = await _categoryInstallmentRepository.FindByAsync(x => x.CategoryId == productCategory.Id);
            var sellerInstallmentCount = await _backOfficeCommunicator.CategoryCompanyInstallment(new CategoryCompanyInstallmentRequest
            {
                CategoryId = productCategory.Id,
                SellerId = productSeller.SellerId
            });
            var installments = await ArrangeInstallments(productSeller, sellerInstallmentCount.Data, categoryInstallment);

            #endregion

            #region IsFavorite

            bool isFavorite = false;
            try
            {
                var customerId = _identityContext.GetUserInfo().Id;
                var query = await _favoriteProductRepository.FindByAsync(y => y.CustomerId == customerId && y.ProductId == request.ProductId && y.IsActive);
                if (query != null)
                    isFavorite = true;
            }
            catch { };

            #endregion

            #region Delivery

            var delivery = await GetDeliveryOptions(product.ProductDeliveries, productSeller);

            #endregion

            #region Other Sellers

            var otherSellers = await ArrangeOtherSellers(product.ProductSellers.Where(x => x.SellerId != productSeller.SellerId).ToList(), product.ProductDeliveries);

            #endregion

            #region UrlRequests

            var allBrandsUrl = new AllBrandsUrl
            {
                Filter = new GetProductListAndFilterQuery
                {
                    FilterModel = new List<FilterModel> { new FilterModel { FilterField = "BrandId", Id = $"{product.BrandId}", Type = "Product" } },
                    PagerInput = new PagerInput(1, 100)
                }
            };

            var allCategoryUrl = new AllCategoryUrl
            {
                Filter = new GetProductListAndFilterQuery
                {
                    FilterModel = new List<FilterModel> { new FilterModel { FilterField = ProductFilterEnum.CategoryId.ToString(), Id = $"{catId}", Type = ProductFilterEnum.ProductCategory.ToString() } },
                    PagerInput = new PagerInput(1, 100)
                }
            };

            #endregion

           

            return _productAssembler.MapToGetProductDetailBySellerIdQueryResult(product, productSeller, mainProductSellerName, brand, categories, categoryAttributes, attributes, attributeValues, groupedProductGroups, otherSellers, delivery, installments, isFavorite, allCategoryUrl, allBrandsUrl, productCategory, breadCrumbsReversed, request.SellerId, request.ProductId, mainProductSellerSeoName);

        }

        private async Task<string> GetSellerName(Guid sellerId)
        {
            var seller = await _merchantCommunicator.GetSellerById(new GetSellerRequest { SellerId = sellerId });
            try
            {
                return !string.IsNullOrEmpty(seller.Data.CompanyName) ? seller.Data.CompanyName : seller.Data.FirmName;
            }
            catch
            {
                return "Seller info not found";
            }

        }

        private async Task<List<List<ProductVariantGroup>>> ArrangeVariants(List<List<VariantGroup>> variants, ChannelCode channel, Guid sellerId)
        {
            var groupedVariants = new List<List<ProductVariantGroup>>();

            var attributeIdsList = variants.SelectMany(x => x.Select(ss => ss.AttributeId));
            var attributeValueIdsList = variants.SelectMany(x => x.Select(ss => ss.AttributeValueId));
            var productIdsList = variants.SelectMany(x => x.Select(p => p.ProductId));

            var attributes = await _attributeRepository.FilterByAsync(a => attributeIdsList.Contains(a.Id));
            var attributeValues = await _attributeValueRepository.FilterByAsync(av => attributeValueIdsList.Contains(av.Id));
            var variantSellers = await _productSellerRepository.FilterByAsync(ps => productIdsList.Contains(ps.ProductId));

            for (int i = 0; i < variants.Count; i++)
            {
                var groupVariant = new List<ProductVariantGroup>();
                foreach (var item in variants[i])
                {
                    ProductSeller seller;
                    IEnumerable<ProductSeller> sellers;
                    if (channel == ChannelCode.IsCep)
                        sellers = variantSellers.Where(x => x.ProductId == item.ProductId && x.SellerId == sellerId);
                    else
                        sellers = variantSellers.Where(x => x.ProductId == item.ProductId);

                    if (sellers.Count() > 1 && sellers.Any(x => x.StockCount > 0))
                        seller = sellers.Where(x => x.StockCount > 0).OrderBy(o => o.SalePrice).FirstOrDefault();
                    else
                        seller = sellers.FirstOrDefault();


                    groupVariant.Add(new ProductVariantGroup
                    {
                        ProductId = item.ProductId,
                        ProductSellerId = seller.Id,
                        SellerId = seller.SellerId,
                        AttributeName = attributes.FirstOrDefault(a => a.Id == item.AttributeId).DisplayName,
                        AttributeValue = attributeValues.FirstOrDefault(av => av.Id == item.AttributeValueId).Value,
                        OrderByAttributeValue = attributeValues.FirstOrDefault(av => av.Id == item.AttributeValueId).Order,
                        IsOpen = seller.StockCount > 0,
                        IsSelected = item.IsSelected,
                        SellerName = await GetSellerName(seller.SellerId),
                        SeoUrl = _productService.GetProductSeoUrl(item.ProductId).Result
                    });
                }
                var orderedGroupVariant = groupVariant.OrderBy(gv => gv.OrderByAttributeValue).ToList();
                groupedVariants.Add(orderedGroupVariant);
            }

            return groupedVariants;
        }

        private async Task<DeliveryOptions> GetDeliveryOptions(IReadOnlyCollection<ProductDelivery> deliveries, ProductSeller productSeller, bool otherSeller = false)
        {
            //TODO: Refactor
            var deliveryResponse = await _merchantCommunicator.GetDeliveriesWithId(new GetSellerDeliveryRequest { DeliveryIds = deliveries.Select(x => x.DeliveryId).ToList() });

            if (deliveryResponse.Data == null)
                return null;

            var badges = await _parameterCommunicator.GetBadges();

            if (!badges.Success || badges.Data == null || deliveryResponse.Data == null)
                throw new BusinessRuleException(ApplicationMessage.EmptyList,
                      ApplicationMessage.EmptyList.Message(),
                      ApplicationMessage.EmptyList.UserMessage());

            var deliveryOptions = new DeliveryOptions { Badges = new List<Badges>() };
            var now = DateTime.Now;
            var fastDelivery = DateTime.Today.AddHours(16);
            if (productSeller.StockCount == 4)
            {
                if (!deliveryOptions.Badges.Exists(x => x.Type == Enum.GetName(typeof(IconType), IconType.LastFourProducts)))
                {
                    deliveryOptions.Badges.Add(new Badges
                    {
                        BadgeUrl = badges.Data.FirstOrDefault(x => x.BatchCode == (int)IconType.LastFourProducts).ImageCdnUrl,
                        Type = Enum.GetName(typeof(IconType), IconType.LastFourProducts)
                    });
                }
            }
            //TODO: Tablo yapısı değişmeli -- foreach switch if gitmek zorunda kalıyoruz. Çok fazla if case var.
            foreach (var delivery in deliveries)
            {
                var sellerDelivery = deliveryResponse.Data.SellerDeliveries.FirstOrDefault(x => x.Id == delivery.DeliveryId);

                if (sellerDelivery == null)
                    continue;

                switch (delivery.DeliveryType)
                {
                    case DeliveryType.CargoDelivery:

                        #region Ücretsiz Teslimat || Kargo Fiyatı

                        if (string.IsNullOrWhiteSpace(sellerDelivery.CampaignText))
                        {
                            deliveryOptions.CargoPriceText = new StyledText
                            {
                                Text = $"{sellerDelivery.Price}",
                                TextStyleInfo = new StyleInfo { TextColor = StyleConstants.Black, FontSize = StyleConstants.Font14 }
                            };
                        }
                        else if (productSeller.SalePrice >= sellerDelivery.CampaignAmount) //Kargo Bedava
                        {
                            if (sellerDelivery.CampaignPrice == 0M)
                            {
                                if (!deliveryOptions.Badges.Exists(x => x.Type == Enum.GetName(typeof(IconType), IconType.FreeDelivery)))
                                {
                                    deliveryOptions.Badges.Add(new Badges
                                    {
                                        BadgeUrl = badges.Data.FirstOrDefault(x => x.BatchCode == (int)IconType.FreeDelivery).ImageCdnUrl,
                                        Type = Enum.GetName(typeof(IconType), IconType.FreeDelivery)
                                    });
                                }
                            }
                            else
                            {
                                deliveryOptions.CargoPriceText = new StyledText
                                {
                                    Text = $"{sellerDelivery.CampaignPrice}",
                                    TextStyleInfo = new StyleInfo { TextColor = StyleConstants.Black, FontSize = StyleConstants.Font14 }
                                };

                            }
                        }
                        else
                        {
                            deliveryOptions.CargoPriceText = new StyledText
                            {
                                Text = $"{sellerDelivery.Price}",
                                TextStyleInfo = new StyleInfo { TextColor = StyleConstants.Black, FontSize = StyleConstants.Font14 }
                            };
                        }
                        #endregion

                        #region Kargo Zamanı

                        if (sellerDelivery.DeliveryDuration > 0)
                        {
                            var cargoDay = now.AddDays(sellerDelivery.DeliveryDuration);

                            deliveryOptions.CargoTimeText = new StyledText
                            {
                                Text = $"{cargoDay.Day} {cargoDay.ToString("MMMM", CultureInfo.GetCultureInfo("tr-TR"))} {cargoDay.ToString("dddd", CultureInfo.GetCultureInfo("tr-TR"))} günü kargoya verilir",
                                TextStyleInfo = new StyleInfo { BackgroundColor = StyleConstants.White, TextColor = StyleConstants.Blue, FontSize = StyleConstants.Font14 },
                                Styles = new List<SubStyleInfo> {
                                    new SubStyleInfo{
                                        SubText = $"{cargoDay.Day} {cargoDay.ToString("MMMM", CultureInfo.GetCultureInfo("tr-TR"))} {cargoDay.ToString("dddd", CultureInfo.GetCultureInfo("tr-TR"))}",
                                        Bold = true,
                                        TextColor = StyleConstants.Blue,
                                        FontSize = StyleConstants.Font16
                                    }
                                }
                            };

                            if (otherSeller)
                                deliveryOptions.CargoTimeText.TextStyleInfo.TextColor = StyleConstants.Black;
                        }
                        else
                        {
                            TimeSpan span = fastDelivery.Subtract(now);
                            if (span <= TimeSpan.Zero)
                                continue;

                            var fastDeliveryCargo = string.Empty;
                            if (span.Minutes > 0)
                                fastDeliveryCargo = $"{span.Minutes} dakika";
                            if (span.Hours > 0)
                                fastDeliveryCargo = $"{span.Hours} saat {fastDeliveryCargo}";


                            deliveryOptions.FastCargoTimeText = new StyledText
                            {
                                Text = $"{fastDeliveryCargo} içinde sipariş verirseniz bugün kargoya verilir",
                                TextStyleInfo = new StyleInfo { BackgroundColor = StyleConstants.Blue, TextColor = StyleConstants.White, FontSize = StyleConstants.Font14 },
                                Styles = new List<SubStyleInfo>{
                                new SubStyleInfo {SubText = fastDeliveryCargo, Bold = true, FontSize = StyleConstants.Font16},
                                new SubStyleInfo {SubText = "bugün", Bold = true, FontSize = StyleConstants.Font16}
                            }
                            };

                            if (otherSeller)
                            {
                                deliveryOptions.FastCargoTimeText.TextStyleInfo.BackgroundColor = StyleConstants.White;
                                deliveryOptions.FastCargoTimeText.TextStyleInfo.TextColor = StyleConstants.Black;
                                deliveryOptions.FastCargoTimeText.Styles.ForEach(x => x.TextColor = StyleConstants.Blue);
                            }
                        }

                        #endregion

                        break;
                    case DeliveryType.FastDelivery: //Hızlı Teslimat
                        if (!deliveryOptions.Badges.Exists(x => x.Type == Enum.GetName(typeof(IconType), IconType.FastDelivery)))
                        {
                            deliveryOptions.Badges.Add(new Badges
                            {
                                BadgeUrl = badges.Data.FirstOrDefault(x => x.BatchCode == (int)IconType.FastDelivery).ImageCdnUrl,
                                Type = Enum.GetName(typeof(IconType), IconType.FastDelivery)
                            });
                        }
                        if (sellerDelivery.DeliveryDuration == 0) // Kurye ile Teslimat Badge'i
                        {
                            if (!deliveryOptions.Badges.Exists(x => x.Type == Enum.GetName(typeof(IconType), IconType.DeliveryMan)))
                            {
                                deliveryOptions.Badges.Add(new Badges
                                {
                                    BadgeUrl = badges.Data.FirstOrDefault(x => x.BatchCode == (int)IconType.DeliveryMan).ImageCdnUrl,
                                    Type = Enum.GetName(typeof(IconType), IconType.DeliveryMan)
                                });
                            }
                        }
                        break;
                    default:
                        break;

                }

            }

            return deliveryOptions;
        }

        private async Task<List<OtherSellers>> ArrangeOtherSellers(List<ProductSeller> otherSellers, IReadOnlyCollection<ProductDelivery> productDeliveries)
        {
            var otherSellersInformation = new List<OtherSellers>();

            foreach (var otherSeller in otherSellers)
            {
                if (otherSeller.StockCount > 0)
                {
                    var urls = await _productImageRepository.FilterByAsync(h => h.SellerId == otherSeller.SellerId && h.ProductId == otherSeller.ProductId);
                    decimal discountRate = otherSeller.ListPrice == 0 ? 0 : Decimal.Round(((otherSeller.ListPrice - otherSeller.SalePrice) / otherSeller.ListPrice) * 100);
                    otherSellersInformation.Add(new OtherSellers
                    {
                        SellerId = otherSeller.SellerId,
                        SellerName = await GetSellerName(otherSeller.SellerId),
                        SellerSeoName = _merhantCommunicator.GetSellerDetailByIds(new GetSellerDetailByIdsRequest { SellerId = new List<Guid> { otherSeller.SellerId } }).Result.Data.FirstOrDefault().SellerSeoName,
                        SalePrice = new Price(otherSeller.SalePrice),
                        ImageUrl = urls?.FirstOrDefault()?.Url,
                        ListPrice = otherSeller.ListPrice > otherSeller.SalePrice ? new Price(otherSeller.ListPrice) : null,
                        DiscountRate = new DiscountRateInfo
                        {
                            DiscountRate = discountRate.ToString(),
                            DiscountRateText = discountRate == 0
                                ? null
                                : new StyledText
                                {
                                    Text = $"Sepette %{discountRate} indirim fiyatı",
                                    TextStyleInfo = new StyleInfo { TextColor = StyleConstants.Blue, FontSize = StyleConstants.Font12 }
                                }
                        },
                        DeliveryOptions = await GetDeliveryOptions(productDeliveries, otherSeller, true)
                    });
                }
            }

            return otherSellersInformation;
        }

        private async Task<string> ArrangeInstallments(ProductSeller productSeller, CategoryCompanyInstallmentResponse sellerInstallment, CategoryInstallment categoryInstallment)
        {
            int installment = 1;

            if (sellerInstallment == null)
                sellerInstallment = new CategoryCompanyInstallmentResponse { MaxInstallmentCount = 1 };

            if (categoryInstallment == null || sellerInstallment.MaxInstallmentCount <= categoryInstallment.MaxInstallmentCount)
                installment = sellerInstallment.MaxInstallmentCount == 0 ? 1 : sellerInstallment.MaxInstallmentCount;
            else
                installment = categoryInstallment.MaxInstallmentCount == 0 ? 1 : categoryInstallment.MaxInstallmentCount;

            var installmentPrice = productSeller.SalePrice / installment;
            string content = "";

            var createInstallmentRequest = new TemplatePreviewRequest();

            if (installment == 1)
            {
                content += $"<tr align='center'><td>-</td><td>{new Price(installmentPrice).ValueString}</td><td>{new Price(installmentPrice).ValueString}</td></tr>";
                createInstallmentRequest.TemplateCode = _configuration["PaymentOneInstallmentCode"];
                createInstallmentRequest.Parameters = new Dictionary<string, string> { { "data", content } };
            }
            else
                for (int i = 1; i <= installment; i++)
                {
                    content += $"<tr align='center'><td>{i}</td><td>{new Price(productSeller.SalePrice / i).ValueString}</td><td>{new Price(productSeller.SalePrice).ValueString}</td></tr>";
                    createInstallmentRequest.TemplateCode = _configuration["PaymentMultipleInstallmentCode"];
                    createInstallmentRequest.Parameters = new Dictionary<string, string> { { "data", content } };
                }
            var result = await _contractCommunicator.TemplatePreview(createInstallmentRequest);
            return result.Data != null ? result.Data.ToString() : "";

        }

    }
    public class AttributeAndAttributeValueIds
    {
        public Guid AttributeId { get; set; }
        public Guid AttributeValueId { get; set; }
    }
}
