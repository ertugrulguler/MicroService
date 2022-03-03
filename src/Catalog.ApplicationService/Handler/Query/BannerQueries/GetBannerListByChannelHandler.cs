using Catalog.ApiContract.Request.Query.BannerQueries;
using Catalog.ApiContract.Response.Query.BannerQueries;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.ApplicationService.Communicator.Merchant;
using Catalog.ApplicationService.Communicator.Merchant.Model;
using Catalog.ApplicationService.Handler.Services;
using Catalog.Domain.BannerAggregate;
using Catalog.Domain.BrandAggregate;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;
using Catalog.Domain.Helpers;
using Catalog.Domain.ProductAggregate;
using Framework.Core.Helpers.Abstract;
using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using BannerActionType = Catalog.Domain.Enums.BannerActionType;
using SellerProductImage = Catalog.ApiContract.Response.Query.BannerQueries.ProductImage;
using SellerProductPrice = Catalog.ApiContract.Response.Query.BannerQueries.ProductPrice;

namespace Catalog.ApplicationService.Handler.Query.BannerQueries
{
    public class GetBannerListByChannelHandler : IRequestHandler<GetBannerChannelQuery, ResponseBase<GetBannerChannelQueryResponse>>
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IBannerLocationRepository _bannerLocationRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductSellerRepository _productSellerRepository;
        private readonly IMerhantCommunicator _merchantCommunicator;
        private readonly ICustomerHelper _customerHelper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IProductChannelRepository _productChannelRepository;
        private readonly IDiscountService _discountService;
        private readonly IConfiguration _configuration;

        public GetBannerListByChannelHandler(IBannerRepository bannerRepository, IBannerLocationRepository bannerLocationRepository, IProductRepository productRepository, IProductSellerRepository productSellerRepository, IMerhantCommunicator merchantCommunicator, ICustomerHelper customerHelper, ICategoryRepository categoryRepository, IBrandRepository brandRepository, IProductChannelRepository productChannelRepository, IDiscountService discountService, IConfiguration configuration)
        {
            _bannerRepository = bannerRepository;
            _bannerLocationRepository = bannerLocationRepository;
            _productRepository = productRepository;
            _productSellerRepository = productSellerRepository;
            _merchantCommunicator = merchantCommunicator;
            _customerHelper = customerHelper;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _productChannelRepository = productChannelRepository;
            _discountService = discountService;
            _configuration = configuration;
        }

        public async Task<ResponseBase<GetBannerChannelQueryResponse>> Handle(GetBannerChannelQuery request, CancellationToken cancellationToken)
        {
            var getBannerResponseList = new ResponseBase<GetBannerChannelQueryResponse>();

            var channelCode = _customerHelper.GetChannel();
            var bannerLocationList = await _bannerLocationRepository.FilterByAsync(p =>
                p.Location == request.BannerLocationType && p.ProductChannelCode == channelCode);

            bannerLocationList = bannerLocationList.OrderBy(bl => bl.Order).ToList();

            var bannerList = await _bannerRepository.FilterByAsync(x => bannerLocationList
                                                                            .Select(x => x.Id)
                                                                            .Contains(x.BannerLocationId) &&
                                                                        (x.EndDate >= DateTime.Now &&
                                                                         x.StartDate <= DateTime.Now) && x.ActionType == BannerActionType.ProductDetail);

            var responseList = new List<ChannelItem>();
            foreach (var bannerLocation in bannerLocationList)
            {
                Product prod = null;
                foreach (var banner in bannerList.Where(b => b.BannerLocationId == bannerLocation?.Id)
                    .OrderBy(o => o.Order).ToList())
                {
                    var item = new ChannelItem();
                    item.ChannelCode = channelCode;
                    item.Title = bannerLocation.Title;
                    item.Description = bannerLocation.Description;
                    item.BannerImageUrl = banner.ImageUrl;
                    item.StartedDate = banner.StartDate.ToString("dd.MM.yyyy hh:mm:ss", new CultureInfo("tr-TR"));
                    item.EndDate = banner.EndDate.ToString("dd.MM.yyyy hh:mm:ss", new CultureInfo("tr-TR"));

                    var productDetail = new ProductDetailsChannel();

                    var productSeller = await _productSellerRepository.FindByAsync(i => i.Id == banner.ActionId && i.StockCount > 0);
                    if (productSeller == null)
                        continue;

                    prod = await _productRepository.GetProductDetailToCreate(productSeller.ProductId);
                    if (prod == null)
                        continue;

                    var channel = await _productChannelRepository.FindByAsync(i =>
                        i.ProductId == prod.Id && i.IsActive && i.ChannelCode == (int)channelCode);

                    if (channel == null)
                        continue;

                    var categories = await _categoryRepository.FilterByAsync(z => prod.ProductCategories.Select(xx => xx.CategoryId).Contains(z.Id));
                    if (categories == null)
                        continue;

                    var productCategory = categories.FirstOrDefault(x => x.Type == CategoryTypeEnum.MainCategory);
                    if (productCategory == null)
                        continue;

                    #region Images

                    prod.ArrangeProductImagesBySellerId(productSeller.SellerId);

                    #endregion


                    var id = productSeller.SellerId;
                    var sellerSeoName = await _merchantCommunicator.GetSellerDetailByIds(new GetSellerDetailByIdsRequest() { SellerId = new List<Guid>() { id } });

                    var mainProductSellerName = await GetSellerName(productSeller.SellerId);

                    var prodBrand = await _brandRepository.FindByAsync(i => i.Id == prod.BrandId);

                    #region ProductDetail
                    productDetail.Name = prod.Name;
                    productDetail.DisplayName = prod.DisplayName;
                    productDetail.BrandName = prodBrand.Name;
                    productDetail.Description = prod.Description;
                    productDetail.SellerName = mainProductSellerName;
                    productDetail.CategoryName = productCategory.DisplayName;

                    productDetail.ProductImages = prod.ProductImages.Select(x => new SellerProductImage()
                    {
                        ImageUrl = x.Url
                    }).ToList();
                    #endregion

                    #region Discount

                    var discount = await _discountService.GetDiscountResult(productSeller.SellerId, productSeller.ProductId,
                        productSeller.SalePrice, productSeller.ListPrice,channelCode);
                    if (discount == null || discount.IsDiscounted == false)
                        continue;
                    #endregion


                    productSeller.SetSalePrice(discount.DiscountedAmount);
                    var discountRate = ArrangeDiscountRate(productSeller.SalePrice, productSeller.ListPrice);

                    #region Product Prices and Stock
                    productDetail.Prices = new SellerProductPrice
                    {
                        VatRate = prod.VatRate,
                        ListPrice = new Price(productSeller.ListPrice),
                        SalePrice = new Price(productSeller.SalePrice),
                        DiscountRate = new DiscountRateInfoByChannel
                        {
                            DiscountRate = discountRate.ToString(),
                            DiscountPrice = (productSeller.ListPrice - productSeller.SalePrice).ToString(),
                            DiscountRateText = discountRate == 0 ? null : new StyledText
                            {
                                Text = $"Sepette %{(int)discountRate} indirim fiyatı",
                                TextStyleInfo = new StyleInfo
                                { TextColor = StyleConstants.Blue, FontSize = StyleConstants.Font12 }
                            }
                        }
                    };
                    #endregion
                    #region Deeplink
                    if (prod != null && sellerSeoName != null)
                        item.Deeplink = $"{_configuration["DeeplinkBaseUrl"]}/{prod.SeoName}-p-{prod.Code}?magaza={sellerSeoName.Data[0].SellerSeoName}";
                    #endregion

                    item.ProductDetail = productDetail;
                    responseList.Add(item);
                };
            }

            getBannerResponseList.Success = true;

            return new ResponseBase<GetBannerChannelQueryResponse>()
            {
                Success = true,
                Data = new GetBannerChannelQueryResponse()
                {
                    AllOpportunityForIscepUrl = $"{_configuration["DeeplinkBaseUrl"]}/iscep/kampanyalar",
                    BannerItems = responseList
                }
            };
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
        private static decimal ArrangeDiscountRate(decimal salePrice, decimal listPrice)
        {
            if (listPrice == 0)
                return 0;
            return Decimal.Round((listPrice - salePrice) * 100 / listPrice);
        }

    }
}