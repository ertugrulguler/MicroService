using System;
using System.Collections.Generic;
using Catalog.ApiContract.Response.Query.CategoryQueries;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;
using Catalog.Domain.ProductAggregate;
using Framework.Core.Helpers.Abstract;
using Framework.Core.Model;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.ApiContract.Request.Query.CategoryQueries;
using Catalog.ApplicationService.Handler.Services;
using Catalog.Domain;
using Catalog.Domain.BannerAggregate;
using Catalog.Domain.BrandAggregate;
using Catalog.Domain.ProductAggregate.ServiceModels;

namespace Catalog.ApplicationService.Handler.Query.CategoryQueries
{
    public class GetCategoryGroupsQueryHandler : IRequestHandler<GetCategoryGroupsQuery, ResponseBase<GetCategoryGroupsResponse>>
    {
        private readonly ICustomerHelper _customerHelper;
        private readonly IProductRepository _productRepository;
        private readonly IProductChannelRepository _productChannelRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDiscountService _discountService;
        private readonly IProductSellerRepository _productSellerRepository;

        public GetCategoryGroupsQueryHandler(ICustomerHelper customerHelper, IProductRepository productRepository, IProductChannelRepository productChannelRepository, ICategoryRepository categoryRepository, IDiscountService discountService, IProductSellerRepository productSellerRepository)
        {
            _customerHelper = customerHelper;
            _productRepository = productRepository;
            _productChannelRepository = productChannelRepository;
            _categoryRepository = categoryRepository;
            _discountService = discountService;
            _productSellerRepository = productSellerRepository;
        }

        public async Task<ResponseBase<GetCategoryGroupsResponse>> Handle(GetCategoryGroupsQuery request, CancellationToken cancellationToken)
        {
            var channel = _customerHelper.GetChannel();

            var products = await _productRepository.GetProductsByChannel(channel, null, null);
            IEnumerable<CategoryGroup> response = null;

            var productIds = products.Select(i => i.Id).ToList();

            var couponWithSellers = await _discountService.GetCouponSellers(productIds);
            if (couponWithSellers.Data.Count == 0)
                return new ResponseBase<GetCategoryGroupsResponse>();

            var catGroups = new List<CategoryGroup>();
            if (products.Count > 0)
            {
                foreach (var product in products)
                {
                    bool productIsAvaliable = false;

                    var productSellers = await _productSellerRepository.FilterByAsync(i => i.IsActive && i.ProductId == product.Id && i.StockCount > 0);
                    if (productSellers == null)
                        continue;

                    foreach (var productSeller in productSellers)
                    {
                        if (couponWithSellers.Data.Exists(i => i.SellerId == productSeller.SellerId && i.ProductId == productSeller.ProductId))
                        {
                            productIsAvaliable = true;
                            break;
                        }
                    }

                    if (productIsAvaliable)
                    {
                        var categoryGroup = new CategoryGroup();
                        var categories = await _categoryRepository.FilterByAsync(z => product.ProductCategories.Select(xx => xx.CategoryId).Contains(z.Id));

                        var productCategory = categories.FirstOrDefault(x => x.Type == CategoryTypeEnum.MainCategory);
                        if (productCategory == null)
                            continue;


                        var cat = await _categoryRepository.FindByAsync(i => i.Id == product.ProductCategories.First().CategoryId);
                        if (cat != null)
                        {
                            categoryGroup.CategoryName = cat.DisplayName;
                            categoryGroup.CategoryId = cat.Id;

                            if (catGroups.Any(i => i.CategoryId == categoryGroup.CategoryId))
                                continue;

                            catGroups.Add(categoryGroup);
                        }
                    }
                }
                response = catGroups;
            }

            return new ResponseBase<GetCategoryGroupsResponse>
            {
                Data = new GetCategoryGroupsResponse() { CategoryGroups = response },
                Success = true
            };
        }
    }
}