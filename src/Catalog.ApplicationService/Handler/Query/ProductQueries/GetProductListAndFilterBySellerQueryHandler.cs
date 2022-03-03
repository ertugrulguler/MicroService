using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.ApplicationService.Assembler;
using Catalog.ApplicationService.Communicator.Merchant;
using Catalog.ApplicationService.Handler.Services;
using Catalog.Domain;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;
using Catalog.Domain.ProductAggregate;
using Catalog.Domain.ProductAggregate.ServiceModels;
using Framework.Core.Authorization;
using Framework.Core.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.ProductQueries
{
    public class GetProductListAndFilterBySellerQueryHandler : IRequestHandler<GetProductListAndFilterBySellerQuery, ResponseBase<GetProductListAndFilterBySellerQueryResult>>
    {
        private readonly IProductAssembler _productAssembler;
        private readonly ICategoryDomainService _categoryDomainService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductService _productService;
        private readonly Catalog.ApplicationService.Handler.Services.v2.IProductService _productServiceV2;
        private readonly IProductVariantService _productVariantService;
        private readonly IMerhantCommunicator _merhantCommunicator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityContext _identityContext;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryService _categoryService;

        public GetProductListAndFilterBySellerQueryHandler(IIdentityContext identityContext, IHttpContextAccessor httpContextAccessor, IProductService productService, IProductAssembler productAssembler, ICategoryDomainService categoryDomainService, IProductVariantService productVariantService, IMerhantCommunicator merhantCommunicator, ICategoryRepository categoryRepository, Catalog.ApplicationService.Handler.Services.v2.IProductService productServiceV2, IProductRepository productRepository, ICategoryService categoryService)
        {
            _categoryDomainService = categoryDomainService;
            _productService = productService;
            _productAssembler = productAssembler;
            _productVariantService = productVariantService;
            _merhantCommunicator = merhantCommunicator;
            _httpContextAccessor = httpContextAccessor;
            _identityContext = identityContext;
            _categoryRepository = categoryRepository;
            _productServiceV2 = productServiceV2;
            _productRepository = productRepository;
            _categoryService = categoryService;

        }

        public async Task<ResponseBase<GetProductListAndFilterBySellerQueryResult>> Handle(GetProductListAndFilterBySellerQuery request, CancellationToken cancellationToken)
        {
            //var bannedSellers = (await _merhantCommunicator.GetBannedSellers()).Data;
            var response = new ResponseBase<GetProductListAndFilterBySellerQueryResult>();
            var selectFiltersList = new List<SelectedFilters>();
            var favoriteList = new List<FavoriteProductsList>();
            var variantableProductList = new List<Guid>();
            var customerId = new Guid?();
            //if (bannedSellers.Contains(request.SellerId))
            //{
            //    throw new BusinessRuleException(ApplicationMessage.SellerNotAvailable,
            //                                  ApplicationMessage.SellerNotAvailable.Message(),
            //                                  ApplicationMessage.SellerNotAvailable.UserMessage());
            //}

            if (request.FilterModel.Where(y => y.FilterField == ProductFilterEnum.CategoryId.ToString()).Count() > 1)
                throw new BusinessRuleException(ApplicationMessage.CategoryIdNotOneOrThan,
                                              ApplicationMessage.CategoryIdNotOneOrThan.Message(),
                                              ApplicationMessage.CategoryIdNotOneOrThan.UserMessage());

            if (request.FilterModel.Where(y => y.FilterField == ProductFilterEnum.CategoryId.ToString()).Count() > 0)
            {
                var categoryId = new Guid(request.FilterModel.Where(y => y.FilterField == ProductFilterEnum.CategoryId.ToString()).FirstOrDefault().Id);
                var existingCategory = categoryId == new Guid() ? null : await _categoryRepository.FindByAsync(y => y.Id == categoryId);
                if (existingCategory == null)
                    throw new BusinessRuleException(ApplicationMessage.CategoryNotFound,
                                                ApplicationMessage.CategoryNotFound.Message(),
                                                ApplicationMessage.CategoryNotFound.UserMessage());
            }


            selectFiltersList.Add(new SelectedFilters { FilterField = ProductFilterEnum.SellerId.ToString(), Id = request.SellerId.ToString(), Type = ProductFilterEnum.ProductSeller.ToString() });

            Enum.TryParse(_httpContextAccessor.HttpContext.Request.Headers["X-ChannelCode"], out ProductChannelCode productChannelCode);

            request.FilterModel.ForEach(y => selectFiltersList.Add(new SelectedFilters { Id = y.Id, FilterField = y.FilterField, Type = y.Type }));

            request.FilterModel.Add(new FilterModel { Id = request.SellerId.ToString(), FilterField = ProductFilterEnum.SellerId.ToString(), Type = ProductFilterEnum.ProductSeller.ToString() });

            var newRequest = new GetProductList { SellerId = request.SellerId, FilterModel = request.FilterModel, OrderBy = request.OrderBy, PagerInput = request.PagerInput, ProductChannelCode = productChannelCode == 0 ? ProductChannelCode.Mobile : productChannelCode };


            //var query = await _productService.GetProductListBySeller(newRequest, bannedSellers);

            var getProductModel = _productServiceV2.ArrangeSpParameters(newRequest, ProductFilterEnum.Product, new List<Category>());
            var productFilterQuery = _productServiceV2.CreateProductFilterSpString(request.PagerInput,
                getProductModel.categorySubList.Select(u => u.Id).ToList(),
                getProductModel.attributeAllIdList, getProductModel.salePriceList, getProductModel.brandIdList,
                getProductModel.codeList,
                getProductModel.searchList, request.OrderBy,
                (int)newRequest.ProductChannelCode, getProductModel.sellerIdList);
            var productFilterCountQuery = _productServiceV2.CreateProductFilterCountSpString(request.PagerInput,
                getProductModel.categorySubList.Select(u => u.Id).ToList(),
                getProductModel.attributeAllIdList, getProductModel.salePriceList, getProductModel.brandIdList,
                getProductModel.codeList,
                getProductModel.searchList, request.OrderBy,
                (int)newRequest.ProductChannelCode, getProductModel.sellerIdList);


            var query = await _productRepository.GetProductAllRelationsSP(productFilterQuery, productFilterCountQuery);

            if (!query.ProductList.Any())
                throw new BusinessRuleException(ApplicationMessage.EmptySellerProducts,
                          ApplicationMessage.EmptySellerProducts.Message(),
                          ApplicationMessage.EmptySellerProducts.UserMessage());

            query.TotalCount = query.TotalCount;

            var productList = await _productRepository.GetProductListWithCodes(query.ProductList.Select(p => p.Code).ToList(), getProductModel.sellerIdList);
            var orderedList = new List<Product>();
            foreach (var item in query.ProductList)
            {
                var product = productList.FirstOrDefault(p => p.Code == item.Code);
                if (product != null)
                    orderedList.Add(product);
            }
            query.ProductList = orderedList;
            var variantableList = await _productVariantService.VariantableProductIds(query.ProductList);
            query.AllProductList = orderedList;

            try
            {
                customerId = _identityContext.GetUserInfo().Id;
            }
            catch (Exception)
            {
                customerId = null;
            };

            if (customerId != null)
                favoriteList = await _productService.GetFavoriteProductsForCustomerId(customerId.Value);

            var catNameList = await _categoryService.GetCagetoriesIdAndNameProducts(query.ProductList.Select(o => o.Id).ToList());

            response.Data = new GetProductListAndFilterBySellerQueryResult
            {

                SelectedFilters = selectFiltersList,
                TotalCount = query.TotalCount,
                ProductList = _productAssembler.MapToGetProductsByCategoryIdQueryResult(query.ProductList, request.OrderBy, favoriteList, variantableList, catNameList).Data,
                OrderList = _productService.GetOrderList()
            };

            response.Success = true;
            return response;
        }
    }
}