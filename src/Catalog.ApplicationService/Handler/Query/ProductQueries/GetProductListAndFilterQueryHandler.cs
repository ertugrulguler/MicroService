using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.ApplicationService.Assembler;
using Catalog.ApplicationService.Communicator.Merchant;
using Catalog.ApplicationService.Communicator.Search;
using Catalog.ApplicationService.Communicator.Search.Model;
using Catalog.ApplicationService.Handler.Services;
using Catalog.Domain;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;
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
    public class GetProductListAndFilterQueryHandler : IRequestHandler<GetProductListAndFilterQuery,
        ResponseBase<ProductListAndFilterQueryResult>>
    {
        private readonly IProductAssembler _productAssembler;
        private readonly ICategoryDomainService _categoryDomainService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductService _productService;
        private readonly Catalog.ApplicationService.Handler.Services.v2.IProductService _productServiceV2;
        private readonly IProductVariantService _productVariantService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityContext _identityContext;
        private readonly IMerhantCommunicator _merhantCommunicator;
        private readonly ISearchCommunicator _searchCommunicator;


        public GetProductListAndFilterQueryHandler(IProductService productService, IProductAssembler productAssembler,
            ICategoryDomainService categoryDomainService, IProductVariantService productVariantService,
            IHttpContextAccessor httpContextAccessor, IIdentityContext identityContext,
            IMerhantCommunicator merhantCommunicator, ISearchCommunicator searchCommunicator,
            ICategoryRepository categoryRepository,
            Catalog.ApplicationService.Handler.Services.v2.IProductService productServiceV2)
        {
            _categoryDomainService = categoryDomainService;
            _productService = productService;
            _productAssembler = productAssembler;
            _httpContextAccessor = httpContextAccessor;
            _productVariantService = productVariantService;
            _identityContext = identityContext;
            _merhantCommunicator = merhantCommunicator;
            _searchCommunicator = searchCommunicator;
            _categoryRepository = categoryRepository;
            _productServiceV2 = productServiceV2;
        }

        public async Task<ResponseBase<ProductListAndFilterQueryResult>> Handle(GetProductListAndFilterQuery request,
            CancellationToken cancellationToken)
        {

            var response = new ResponseBase<ProductListAndFilterQueryResult>();
            var selectFiltersList = new List<SelectedFilters>();
            var favoriteList = new List<FavoriteProductsList>();
            var variantableProductList = new List<Guid>();
            var customerId = new Guid?();
            var categoryList = new List<Guid>();

            bool productSearch = false;
            Enum.TryParse(_httpContextAccessor.HttpContext.Request.Headers["X-ChannelCode"], out ProductChannelCode productChannelCode);
            #region Search için.(Yapılıyor)
            if (!String.IsNullOrEmpty(request.Query))
            {
                if ((request.FilterModel == null || request.FilterModel.Count < 1) || (request.FilterModel != null && request.IsSellerSearch))
                {
                    SearchRequest searchRequest = new SearchRequest()
                    {
                        Message = "{\"q\": \"" + request.Query + "\", \"d\": \"False\"}",//\"l\": 500,
                        UserId = !String.IsNullOrEmpty(_identityContext.GetUserInfo().Id.ToString()) ? _identityContext.GetUserInfo().Id.ToString() : "113223",
                    };
                    var searchRes = await _searchCommunicator.Search(searchRequest);

                    #region maximum mobil aksiyonu kaldırıldı.
                    //Maximum Mobil Kontrolü yap!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    //if (searchRes.serviceOutput != null && searchRes.serviceOutput.cbot != null && searchRes.serviceOutput.cbot.deeplinkId != 0 && productChannelCode == ProductChannelCode.Mobile)
                    //{
                    //    response.Data = new ProductListAndFilterQueryResult
                    //    {
                    //        MaximumMobilAction = new MMAction()
                    //        {
                    //            SearchType = searchRes.serviceOutput.cbot.type,
                    //            DeeplinkData = new ApiContract.Response.Query.SearchQueries.DeeplinkData()
                    //            {
                    //                Id = searchRes.serviceOutput.cbot.deeplinkId,
                    //                DeepLink = searchRes.serviceOutput.cbot.deeplinkUrl
                    //            }
                    //        }
                    //    };
                    //    response.Success = true;
                    //    return response;
                    //}
                    #endregion

                    if (request.FilterModel == null)
                    {
                        request.FilterModel = new List<FilterModel>();
                    }
                    foreach (var item in searchRes.serviceOutput.res)
                    {
                        FilterModel filterModel = new FilterModel()
                        {
                            FilterField = "Code",
                            Id = item.ID_match,
                            Type = "Product"
                        };
                        request.FilterModel.Add(filterModel);
                        productSearch = true;
                        request.OrderBy = OrderBy.None;
                    }
                }

            }
            #endregion


            if (request.FilterModel.Count() <= 0)
                throw new BusinessRuleException(ApplicationMessage.CategoryIdAndFilterModelNotEmpty,
                    ApplicationMessage.CategoryIdAndFilterModelNotEmpty.Message(),
                    ApplicationMessage.CategoryIdAndFilterModelNotEmpty.UserMessage());


            if (request.FilterModel.Where(y => y.FilterField == ProductFilterEnum.CategoryId.ToString()).Count() > 0)
            {

                foreach (var item in request.FilterModel)
                {
                    if (item.FilterField == ProductFilterEnum.CategoryId.ToString())
                    {
                        categoryList.Add(new Guid(item.Id));
                    }
                }
            }
            if (categoryList.Count > 0)
            {
                var notExistingCategoryList = await _categoryRepository.NotExistingCategories(categoryList);
                if (notExistingCategoryList.Count > 0)
                {
                    throw new BusinessRuleException(string.Join(',', notExistingCategoryList) + ApplicationMessage.CategoryNotFound,
                               ApplicationMessage.CategoryNotFound.Message(),
                                 ApplicationMessage.CategoryNotFound.UserMessage());
                }
            }

            request.FilterModel?.ForEach(y => selectFiltersList.Add(new SelectedFilters { Id = y.Id, FilterField = y.FilterField, Type = y.Type }));

            var newRequet = new GetProductList
            {
                IsSellerVisible = request.IsSellerVisible,
                CategoryIds = categoryList,
                FilterModel = request.FilterModel,
                OrderBy = request.OrderBy,
                PagerInput = request.PagerInput,
                ProductChannelCode = productChannelCode == 0 ? ProductChannelCode.Mobile : productChannelCode
            };

            var bannedSellers = (await _merhantCommunicator.GetBannedSellers()).Data;

            var categorySubList = await _categoryDomainService.GetLeafCategoriesV3(newRequet.CategoryIds);

            var query = await _productService.GetProductList(newRequet, bannedSellers, categorySubList);

            if (!query.ProductList.Any())
                return new ResponseBase<ProductListAndFilterQueryResult> { Data = null, Success = true };

            query.AllProductList = await _productVariantService.FilterVariants(query.AllProductList);
            #region search için yazıldı
            if (productSearch)
            {
                List<Domain.ProductAggregate.Product> newProductList1 = new List<Domain.ProductAggregate.Product>();
                foreach (var item in request.FilterModel)
                {
                    var searchProduct = query.AllProductList.Find(x => x.Code == item.Id);
                    if (searchProduct != null)
                    {
                        newProductList1.Add(searchProduct);
                    }
                }
                query.AllProductList = new List<Domain.ProductAggregate.Product>();
                query.AllProductList = newProductList1;
            }
            #endregion
            query.TotalCount = query.AllProductList.Count();
            query.ProductList = query.AllProductList.Skip((request.PagerInput.PageIndex - 1) * request.PagerInput.PageSize).Take(request.PagerInput.PageSize).ToList();
            var variantableList = await _productVariantService.VariantableProductIds(query.ProductList);

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



            if (!request.IsVisibleAllFilters)
            {
                var productFilters = await _productServiceV2.GetProductFilter(newRequet, query.AllProductList, bannedSellers, categorySubList, true);
                response.Data = new ProductListAndFilterQueryResult
                {
                    SelectedFilters = selectFiltersList,
                    TotalCount = query.TotalCount,
                    //ProductList = _productAssembler.MapToGetProductsByCategoryIdQueryResult(query.ProductList, request.OrderBy, favoriteList, variantableList).Data,
                    ProductFilters = null,
                    QuickFilteringList = productFilters.Where(f => f.Title == ProductFilterEnum.Brand.ToString()).ToList(),
                    OrderList = _productService.GetOrderList()
                };
            }
            else
            {
                var productFilters = await _productServiceV2.GetProductFilter(newRequet, query.AllProductList, bannedSellers, categorySubList, false);
                response.Data = new ProductListAndFilterQueryResult
                {
                    SelectedFilters = selectFiltersList,
                    TotalCount = query.TotalCount,
                    //ProductList = _productAssembler.MapToGetProductsByCategoryIdQueryResult(query.ProductList,request.OrderBy, favoriteList, variantableList).Data,
                    ProductFilters = productFilters,
                    QuickFilteringList = productFilters.Where(f => f.Title == ProductFilterEnum.Brand.ToString())
                        .ToList(),
                    OrderList = _productService.GetOrderList()
                };
            }
            response.Success = true;
            return response;
        }
    }
}