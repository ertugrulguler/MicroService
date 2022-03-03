using Catalog.ApiContract.Request.Command.BannerCommands;
using Catalog.ApplicationService.AutoMapper;
using Catalog.ApplicationService.Communicator.Merchant;
using Catalog.ApplicationService.Communicator.Merchant.Model;
using Catalog.Domain;
using Catalog.Domain.BannerAggregate;
using Catalog.Domain.BrandAggregate;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;
using Catalog.Domain.ProductAggregate;
using Framework.Core.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BannerActionType = Catalog.Domain.Enums.BannerActionType;
using BannerFilters = Catalog.Domain.BannerAggregate.BannerFilters;
using BannerFilterType = Catalog.Domain.Enums.BannerFilterType;

namespace Catalog.ApplicationService.Handler.Command.BannerCommands
{
    public class CreateBannerCommandHandler : IRequestHandler<CreateBannerCommand, ResponseBase<List<string>>>
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IAutoMapperConfiguration _autoMapper;
        private readonly IBannerLocationRepository _bannerLocationRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IProductSellerRepository _productSellerRepository;
        private readonly IMerhantCommunicator _merchantCommunicator;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDbContextHandler _dbContextHandler;
        private readonly IBannerFiltersRepository _bannerFiltersRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public CreateBannerCommandHandler(IBannerRepository bannerRepository, IDbContextHandler dbContextHandler, IAutoMapperConfiguration autoMapper,
            IBannerLocationRepository bannerLocationRepository, IBrandRepository brandRepository, IProductSellerRepository productSellerRepository,
            IMerhantCommunicator merchantCommunicator, ICategoryRepository categoryRepository, IBannerFiltersRepository bannerFiltersRepository, IHttpContextAccessor httpContextAccessor)
        {
            _bannerRepository = bannerRepository;
            _dbContextHandler = dbContextHandler;
            _autoMapper = autoMapper;
            _bannerLocationRepository = bannerLocationRepository;
            _brandRepository = brandRepository;
            _productSellerRepository = productSellerRepository;
            _merchantCommunicator = merchantCommunicator;
            _categoryRepository = categoryRepository;
            _bannerFiltersRepository = bannerFiltersRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResponseBase<List<string>>> Handle(CreateBannerCommand request, CancellationToken cancellationToken)
        {
            Enum.TryParse(_httpContextAccessor.HttpContext.Request.Headers["X-ChannelCode"], out ProductChannelCode productChannelCode);

            var bannerLocation = await _bannerLocationRepository.FindByAsync(bl => bl.Id == request.BannerLocationId);
            var errorList = new List<string>();
            var name = request.Name;
            var imageUrl = request.ImageUrl;

            if (bannerLocation == null)
                errorList.Add(ApplicationMessage.NotBannnerLocation.UserMessage());

            if (request.StartDate > request.EndDate || request.StartDate == request.EndDate)
                errorList.Add(ApplicationMessage.NotDate.UserMessage());

            if (request.ActionType == BannerActionType.Seller) //sellers.
            {
                var seller = (await _merchantCommunicator.GetSellerById(new GetSellerRequest { SellerId = request.ActionId.Value })).Data;
                name = seller?.CompanyName != null ? seller?.CompanyName : seller?.FirmName;
            }

            if (request.ActionType == BannerActionType.Filter && request.BannerFilters != null)
            {
                foreach (var bannerFilter in request.BannerFilters)
                {
                    if (bannerFilter.BannerFilterType == BannerFilterType.Seller)
                    {
                        var productSeller = await _productSellerRepository.FindByAsync(ps => ps.Id == bannerFilter.ActionId);
                        var seller = (await _merchantCommunicator.GetSellerById(new GetSellerRequest { SellerId = request.ActionId.Value })).Data;
                        name = seller?.CompanyName != null ? seller?.CompanyName : seller?.FirmName;
                    }

                    if (bannerFilter.BannerFilterType == BannerFilterType.Brand)
                    {
                        var brand = await _brandRepository.FindByAsync(b => b.Id == bannerFilter.ActionId);
                        name = brand?.Name;
                    }

                    if (bannerFilter.BannerFilterType == BannerFilterType.Category)
                    {
                        var categoryIsNotLeaf = await _categoryRepository.Exist(c => c.ParentId == bannerFilter.ActionId);
                        if (categoryIsNotLeaf)
                            errorList.Add(bannerFilter.ActionId + ApplicationMessage.CategoryIsNotLeaf.UserMessage());

                        var categoryName = await _categoryRepository.FindByAsync(c => c.Id == bannerFilter.ActionId);
                        if (categoryName != null)
                            name = categoryName?.Name;
                    }
                }
            }
            var banner = new Banner(bannerLocation.Id, request.ActionType, name,
                            request.ActionId, imageUrl, request.Order, request.StartDate, request.EndDate,
                            request.Description, request.MMActionId, request.MinIosVersion, request.MinAndroidVersion);
            await _bannerRepository.SaveAsync(banner);

            if (request.BannerFilters != null)
            {
                foreach (var bannerFilter in request.BannerFilters)
                {
                    var bannerFilterCheck = await _bannerFiltersRepository.FindByAsync(bf => bf.BannerId == banner.Id && bf.ActionId == bannerFilter.ActionId);
                    if (bannerFilterCheck == null)
                    {
                        var newBannerFilters = new BannerFilters(banner.Id, bannerFilter.BannerFilterType, bannerFilter.ActionId);
                        await _bannerFiltersRepository.SaveAsync(newBannerFilters);
                    }
                    else bannerFilterCheck.SetBannerFilters(bannerFilter.BannerFilterType, bannerFilter.ActionId);
                }
            }

            if (errorList.Count == 0)
                await _dbContextHandler.SaveChangesAsync();

            return new ResponseBase<List<string>>()
            {
                Data = errorList,
                Success = errorList.Count > 0 ? false : true
            };
        }
    }
}
