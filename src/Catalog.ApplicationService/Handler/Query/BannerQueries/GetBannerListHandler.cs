using Catalog.ApiContract.Request.Query.BannerQueries;
using Catalog.ApiContract.Response.Query.BannerQueries;
using Catalog.ApplicationService.Assembler;
using Catalog.ApplicationService.Communicator.Merchant;
using Catalog.ApplicationService.Handler.Services;
using Catalog.Domain.BannerAggregate;
using Catalog.Domain.BrandAggregate;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;
using Catalog.Domain.ProductAggregate;
using Catalog.Domain.ProductAggregate.ServiceModels;
using Framework.Core.Attribute;
using Framework.Core.Authorization;
using Framework.Core.Helpers.Abstract;
using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BannerActionType = Catalog.Domain.Enums.BannerActionType;

namespace Catalog.ApplicationService.Handler.Query.BannerQueries
{
    public class GetBannerListHandler : IRequestHandler<GetBannerQuery, ResponseBase<List<GetBannerList>>>
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IBannerLocationRepository _bannerLocationRepository;
        private readonly IBannerFiltersRepository _bannerFiltersRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductSellerRepository _productSellerRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IMerhantCommunicator _merchantCommunicator;
        private readonly ICustomerHelper _customerHelper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IIdentityContext _identityContext;
        private readonly IProductService _productService;
        private readonly IGeneralAssembler _generalAssembler;
        public GetBannerListHandler(IProductRepository productRepository, IBannerRepository bannerRepository,
            IBannerLocationRepository bannerLocationRepository,
            IBannerFiltersRepository bannerFiltersRepository, IProductSellerRepository productSellerRepository,
            IProductImageRepository productImageRepository, IMerhantCommunicator merchantCommunicator, ICustomerHelper customerHelper,
            IBrandRepository brandRepository, IIdentityContext identityContext, IProductService productService,
            ICategoryRepository categoryRepository, IGeneralAssembler generalAssembler)
        {
            _productRepository = productRepository;
            _bannerRepository = bannerRepository;
            _bannerLocationRepository = bannerLocationRepository;
            _bannerFiltersRepository = bannerFiltersRepository;
            _productSellerRepository = productSellerRepository;
            _productImageRepository = productImageRepository;
            _merchantCommunicator = merchantCommunicator;
            _customerHelper = customerHelper;
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
            _identityContext = identityContext;
            _productService = productService;
            _generalAssembler = generalAssembler;
        }

        private static int ArrangeDiscountRate(decimal salePrice, decimal listPrice)
        {
            if (listPrice == 0)
                return 0;
            return (int)((listPrice - salePrice) * 100 / listPrice);
        }

        [CacheInfoAttribute(1)]
        public async Task<ResponseBase<List<GetBannerList>>> Handle(GetBannerQuery request, CancellationToken cancellationToken)
        {
            var getBannerResponseList = new ResponseBase<List<GetBannerList>>
            {
                Data = new List<GetBannerList>()
            };
            var channelCode = _customerHelper.GetChannel();
            var customerId = new Guid?();
            var favoriteList = new List<FavoriteProductsList>();
            var bannerLocationList = await _bannerLocationRepository.FilterByAsync(p => p.Location == request.BannerLocationType && p.ProductChannelCode == channelCode && p.ActionId == request.ActionId);

            bannerLocationList = bannerLocationList.OrderBy(bl => bl.Order).ToList();
            var bannerList = await _bannerRepository.FilterByAsync(x => bannerLocationList
                                                                            .Select(x => x.Id)
                                                                            .Contains(x.BannerLocationId) &&
                                                                        (x.EndDate >= DateTime.Now &&
                                                                         x.StartDate <= DateTime.Now));
            try
            {
                customerId = _identityContext.GetUserInfo().Id;
            }
            catch (Exception)
            {
                customerId = null;
            };
            var bannedSeller = (await _merchantCommunicator.GetBannedSellers()).Data;
            if (customerId != null)
                favoriteList = await _productService.GetFavoriteProductsForCustomerId(customerId.Value);

            var favoriteProductsCount = favoriteList.Count() > 0 ? true : false;
            foreach (var bannerLocation in bannerLocationList)
            {
                var responseBanner = new GetBannerList
                {
                    Items = new List<Items>(),
                    Type = bannerLocation?.BannerType,
                    BannerLocationId = bannerLocation?.Id,
                    Title = bannerLocation?.Title
                };
                foreach (var banner in bannerList.Where(b => b.BannerLocationId == bannerLocation?.Id).OrderBy(o => o.Order).ToList())
                {
                    var item = new Items();

                    if (!_customerHelper.IsVersionCompatible(banner.MinIosVersion, banner.MinAndroidVersion))
                    {
                        continue;
                    }
                    if (banner.ActionType == BannerActionType.MaximumAction)
                    {
                        item.BannerId = banner.Id;
                        item.Name = banner?.Name;
                        item.ActionId = banner?.ActionId;
                        item.ActionType = banner.ActionType;
                        item.ImageUrl = banner?.ImageUrl;
                        item.MMActionId = banner?.MMActionId;

                    }
                    if (banner.ActionType == BannerActionType.Filter)
                    {
                        item.BannerId = banner.Id;
                        item.Name = banner?.Name;
                        item.ActionId = banner?.ActionId;
                        item.ActionType = banner.ActionType;
                        item.ImageUrl = banner?.ImageUrl;
                        item.Filter = new ApiContract.Request.Query.ProductQueries.GetProductListAndFilterQuery();
                        item.Filter.FilterModel = new List<FilterModel>();
                        if (banner.ActionId != null)
                        {
                            item.Filter.FilterModel.Add(new FilterModel
                            {
                                Id = banner.ActionId.Value.ToString(),
                                FilterField = ProductFilterEnum.CategoryId.ToString(),
                                Type = ProductFilterEnum.ProductCategory.ToString()
                            });
                        }
                        var bannerFilters = await _bannerFiltersRepository.FilterByAsync(f => f.BannerId == banner.Id);
                        foreach (var bannerFilter in bannerFilters)
                        {

                            var bannerFilterModel = new FilterModel { Id = bannerFilter.ActionId.ToString() };
                            if (bannerFilter.BannerFilterType == Domain.Enums.BannerFilterType.Brand)
                            {
                                var brand = await _brandRepository.GetByIdAsync(bannerFilter.ActionId);
                                bannerFilterModel.FilterField = "BrandId";
                                bannerFilterModel.Type = "Product";
                                bannerFilterModel.SeoUrl = "/" + brand?.SeoName;
                                item.SeoUrl = "/" + brand?.SeoName;
                            }
                            else if (bannerFilter.BannerFilterType == Domain.Enums.BannerFilterType.Seller)
                            {
                                item.SeoUrl = "/magaza/" + _generalAssembler.GetSeoName(banner.Name, SeoNameType.Seller);
                                bannerFilterModel.FilterField = "SellerId";
                                bannerFilterModel.Type = "ProductSeller";
                                bannerFilterModel.SeoUrl = "/magaza/" + _generalAssembler.GetSeoName(banner.Name, SeoNameType.Seller);
                            }
                            else if (bannerFilter.BannerFilterType == Domain.Enums.BannerFilterType.Text)
                            {
                                bannerFilterModel.FilterField = "Search";
                                bannerFilterModel.Type = "Product";
                            }
                            else if (bannerFilter.BannerFilterType == Domain.Enums.BannerFilterType.Category)
                            {
                                var category = await _categoryRepository.GetByIdAsync(bannerFilter.ActionId);
                                if (category != null)
                                {
                                    item.SeoUrl = "/" + category?.SeoName + "-k-" + category?.Code;
                                    bannerFilterModel.SeoUrl = "/" + category?.SeoName + "-k-" + category?.Code;
                                }
                                bannerFilterModel.FilterField = "CategoryId";
                                bannerFilterModel.Type = "ProductCategory";
                            }
                            else
                                continue;

                            item.Filter.FilterModel.Add(bannerFilterModel);
                        }
                    }
                    else if (banner.ActionType == BannerActionType.NoAction || banner.ActionType == BannerActionType.Seller || banner.ActionType == BannerActionType.ProductDetail || banner.ActionType == BannerActionType.Category)
                    {
                        item.BannerId = banner.Id;
                        item.Name = banner?.Name;
                        item.ActionId = banner?.ActionId;
                        item.ActionType = banner.ActionType;
                        item.ImageUrl = banner?.ImageUrl;
                        item.SeoUrl = _generalAssembler.GetSeoName(banner.Name, SeoNameType.Seller);
                        if (banner.ActionType == BannerActionType.ProductDetail)
                        {
                            var product = await _productRepository.GetProductWithProductSellerId(banner.ActionId.Value);
                            if (product != null && product.ProductSellers.FirstOrDefault(s => s.Id == banner.ActionId.Value).StockCount > 0)
                            {
                                if (item.ActionId != null) //productid
                                {
                                    item.SeoUrl = "/" + product?.SeoName + "-p-" + product?.Code; //name kalsın seoname yapılacak mergeden sonra TODO:Özlem
                                }
                                item.ProductDetail = new ProductDetails
                                {
                                    ProductId = product.Id,
                                    ProductSellerId = banner.ActionId.Value,
                                    SellerId = product.ProductSellers.FirstOrDefault(s => s.Id == banner.ActionId.Value).SellerId
                                };
                            }
                        }
                        else if (banner.ActionType == BannerActionType.Seller)
                        {
                            item.SeoUrl = "/magaza/" + _generalAssembler.GetSeoName(banner.Name, SeoNameType.Seller);
                        }
                        else
                        {
                            continue;
                        }
                    }

                    else if (banner.ActionType == BannerActionType.Product)
                    {
                        var group = bannerList.Where(b => b.BannerLocationId == bannerLocation.Id).OrderBy(o => o.Order)
                            .ToList();

                        var productSellers =
                            (await _productSellerRepository.FilterByAsync(ps => group.Select(x => x.ActionId).Contains(ps.Id)))
                            .Where(x => !bannedSeller.Contains(x.SellerId));

                        var products = await _productRepository.FilterByAsync(p => productSellers.Select(x => x.ProductId).Contains(p.Id));
                        var productsModel = new List<BannerProductList>();

                        foreach (var productSeller in productSellers.OrderBy(x => group.FirstOrDefault(b => b.ActionId == x.Id)?.Order))
                        {
                            if (productSeller.StockCount > 0)
                            {
                                var imageList = await _productImageRepository.FilterByAsync(p =>
                                    p.SellerId == productSeller.SellerId && p.ProductId == productSeller.ProductId);
                                if (imageList.Any())
                                {
                                    var discountRate = ArrangeDiscountRate(productSeller.SalePrice, productSeller.ListPrice);
                                    var product = new BannerProductList();
                                    var mainProduct = products?.SingleOrDefault(p => p.Id == productSeller.ProductId);
                                    product.SeoUrl = "/" + mainProduct?.SeoName + "-p-" + mainProduct?.Code;
                                    product.BannerId = group.FirstOrDefault(x => x.ActionId == productSeller.Id).Id;
                                    product.Name = mainProduct?.Name;
                                    product.DisplayName = mainProduct?.DisplayName;
                                    product.ListPrice = productSeller.ListPrice > productSeller.SalePrice
                                        ? new Price(productSeller.ListPrice)
                                        : null;
                                    product.SalePrice = new Framework.Core.Model.Price(productSeller.SalePrice);
                                    product.SellerId = productSeller.SellerId;
                                    product.Id = productSeller.ProductId;
                                    product.ProductSellerId = productSeller.Id;
                                    product.IsFavorite = favoriteProductsCount ? favoriteList.Where(y => y.ProductId == product.Id).Count() > 0 ? true : false : false;
                                    product.ImageUrl = new List<string>()
                                {
                                    imageList.FirstOrDefault().Url
                                };
                                    productsModel.Add(product);
                                }
                            }
                        }

                        responseBanner.ProductList = productsModel;
                        continue;
                    }

                    responseBanner.Items.Add(item);
                }

                getBannerResponseList.Data.Add(responseBanner);
            }
            getBannerResponseList.Success = true;
            getBannerResponseList.Data = getBannerResponseList.Data;
            return getBannerResponseList;
        }
    }
}
