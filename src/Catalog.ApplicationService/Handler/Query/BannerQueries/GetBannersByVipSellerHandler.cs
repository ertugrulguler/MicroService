using Catalog.ApiContract.Request.Query.BannerQueries;
using Catalog.ApiContract.Response.Query.BannerQueries;
using Catalog.Domain.BannerAggregate;
using Catalog.Domain.Enums;
using Catalog.Domain.ProductAggregate;
using Catalog.Domain.ProductAggregate.ServiceModels;
using Framework.Core.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BannerActionType = Catalog.Domain.Enums.BannerActionType;

namespace Catalog.ApplicationService.Handler.Query.BannerQueries
{
    public class GetBannersByVipSellerHandler : IRequestHandler<GetBannersByVipSellerQuery, ResponseBase<List<GetBannersByVipSeller>>>
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IBannerLocationRepository _bannerLocationRepository;
        private readonly IBannerFiltersRepository _bannerFiltersRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductSellerRepository _productSellerRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public GetBannersByVipSellerHandler(IProductRepository productRepository, IBannerRepository bannerRepository,
            IBannerLocationRepository bannerLocationRepository,
            IBannerFiltersRepository bannerFiltersRepository, IProductSellerRepository productSellerRepository,
            IProductImageRepository productImageRepository, IHttpContextAccessor httpContextAccessor)
        {
            _productRepository = productRepository;
            _bannerRepository = bannerRepository;
            _bannerLocationRepository = bannerLocationRepository;
            _bannerFiltersRepository = bannerFiltersRepository;
            _productSellerRepository = productSellerRepository;
            _productImageRepository = productImageRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseBase<List<GetBannersByVipSeller>>> Handle(GetBannersByVipSellerQuery request, CancellationToken cancellationToken)
        {
            var getBannerResponseList = new ResponseBase<List<GetBannersByVipSeller>>
            {
                Data = new List<GetBannersByVipSeller>()
            };

            var bannerLocationList = await _bannerLocationRepository.FilterByAsync(p => p.Location == request.BannerLocationType && p.ProductChannelCode == request.ChannelCode && p.ActionId == request.SellerId);

            var bannerList = await _bannerRepository.FilterByAsync(x =>
                bannerLocationList.Select(x => x.Id).Contains(x.BannerLocationId) && (x.EndDate >= DateTime.Now && x.StartDate <= DateTime.Now));

            foreach (var bannerLocation in bannerLocationList.OrderBy(bl => bl.Order))
            {
                var responseBanner = new GetBannersByVipSeller
                {
                    VipItems = new List<VipItems>(),
                    Type = bannerLocation?.BannerType,
                    Title = bannerLocation?.Title
                };

                foreach (var banner in bannerList.Where(b => b.BannerLocationId == bannerLocation?.Id).OrderBy(o => o.Order).ToList())
                {
                    var item = new VipItems();
                    if (banner.ActionType == BannerActionType.Filter)
                    {
                        item.Name = banner?.Name;
                        item.ActionId = banner?.ActionId;
                        item.ActionType = banner.ActionType;
                        item.ImageUrl = banner?.ImageUrl;
                        item.Filter = new ApiContract.Request.Query.ProductQueries.GetProductListAndFilterQuery();
                        if (banner.ActionId != null)
                            item.Filter.FilterModel.Add(new FilterModel
                            {
                                Id = banner.ActionId.Value.ToString(),
                                FilterField = ProductFilterEnum.CategoryId.ToString(),
                                Type = ProductFilterEnum.ProductCategory.ToString()
                            });
                        var bannerFilters = await _bannerFiltersRepository.FilterByAsync(f => f.BannerId == banner.Id);

                        item.Filter.FilterModel = new List<FilterModel>();
                        foreach (var bannerFilter in bannerFilters)
                        {
                            var bannerFilterModel = new FilterModel { Id = bannerFilter.ActionId.ToString() };
                            if (bannerFilter.BannerFilterType == Domain.Enums.BannerFilterType.Brand)
                            {
                                bannerFilterModel.FilterField = "BrandId";
                                bannerFilterModel.Type = "Product";
                            }
                            else if (bannerFilter.BannerFilterType == Domain.Enums.BannerFilterType.Seller)
                            {
                                bannerFilterModel.FilterField = "SellerId";
                                bannerFilterModel.Type = "ProductSeller";
                            }
                            else if (bannerFilter.BannerFilterType == Domain.Enums.BannerFilterType.Text)
                            {
                                bannerFilterModel.FilterField = "Search";
                                bannerFilterModel.Type = "Product";
                            }
                            else if (bannerFilter.BannerFilterType == Domain.Enums.BannerFilterType.Category)
                            {
                                bannerFilterModel.FilterField = "CategoryId";
                                bannerFilterModel.Type = "ProductCategory";
                            }
                            else
                                continue;

                            item.Filter.FilterModel.Add(bannerFilterModel);
                        }
                    }
                    else if (banner.ActionType == BannerActionType.NoAction || banner.ActionType == BannerActionType.Seller || banner.ActionType == BannerActionType.ProductDetail)
                    {
                        item.Name = banner?.Name;
                        item.ActionId = banner?.ActionId;
                        item.ActionType = banner.ActionType;
                        item.ImageUrl = banner?.ImageUrl;
                        if (banner.ActionType == BannerActionType.ProductDetail)
                        {
                            var product = await _productSellerRepository.GetByIdAsync(banner.ActionId.Value);
                            if (product != null)
                                item.VipProductDetails = new VipProductDetails
                                {
                                    ProductId = product.ProductId,
                                    ProductSellerId = product.Id,
                                    SellerId = product.SellerId
                                };
                        }
                    }

                    responseBanner.VipItems.Add(item);
                }

                getBannerResponseList.Data.Add(responseBanner);
            }

            getBannerResponseList.Success = true;
            getBannerResponseList.Data = getBannerResponseList.Data.OrderBy(f => f.Type).ToList();
            return getBannerResponseList;
        }
    }
}