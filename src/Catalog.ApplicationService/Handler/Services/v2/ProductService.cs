using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.ApiContract.Response;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.ApplicationService.Assembler;
using Catalog.ApplicationService.Communicator.Merchant;
using Catalog.ApplicationService.Communicator.Merchant.Model;
using Catalog.ApplicationService.Communicator.Search;
using Catalog.ApplicationService.Communicator.Search.Model;
using Catalog.Domain;
using Catalog.Domain.AttributeAggregate;
using Catalog.Domain.BrandAggregate;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;
using Catalog.Domain.Helpers;
using Catalog.Domain.ProductAggregate;
using Catalog.Domain.ProductAggregate.ServiceModels;
using Catalog.Domain.ValueObject.StoreProcedure;

using Framework.Core.Authorization;
using Framework.Core.Model;

using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Services.v2
{
    public class ProductService : IProductService
    {
        private readonly IExpressionBinding _expressionBinding;
        private readonly ICategoryDomainService _categoryDomainService;
        private readonly IProductRepository _productRepository;
        private readonly IAttributeRepository _attributeRepository;
        private readonly IAttributeValueRepository _attributeValueRepository;
        private readonly IProductSellerRepository _productSellerRepository;
        private readonly IProductAttributeRepository _productAttributeRepository;
        private readonly IMerhantCommunicator _merchantCommunicator;
        private readonly ICategoryAttributeRepository _categoryAttributeRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IAttributeMapRepository _attributeMapRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductAssembler _productAssembler;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IAttributeRepository _atributeRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly Catalog.ApplicationService.Handler.Services.IProductService _productService;
        private readonly IProductVariantService _productVariantService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityContext _identityContext;
        private readonly IMerhantCommunicator _merhantCommunicator;
        private readonly ISearchCommunicator _searchCommunicator;
        private readonly ICategoryService _categoryService;
        private readonly IGeneralAssembler _generalAssembler;

        public ProductService(IExpressionBinding expressionBinding, ICategoryDomainService categoryDomainService,
            IProductRepository productRepository, IAttributeRepository attributeRepository,
            IAttributeValueRepository attributeValueRepository, IProductSellerRepository productSellerRepository,
            IProductAttributeRepository productAttributeRepository,
            IMerhantCommunicator merchantCommunicator, IProductCategoryRepository productCategoryRepository,
            ICategoryAttributeRepository categoryAttributeRepository, IAttributeMapRepository attributeMapRepository, ICategoryRepository categoryRepository,
            Catalog.ApplicationService.Handler.Services.IProductService productService, IProductAssembler productAssembler,
            IProductVariantService productVariantService,
            IHttpContextAccessor httpContextAccessor, IIdentityContext identityContext,
            IMerhantCommunicator merhantCommunicator, ISearchCommunicator searchCommunicator,
            IProductImageRepository productImageRepository,
            IBrandRepository brandRepository, IAttributeRepository atributeRepository, ICategoryService categoryService, IGeneralAssembler generalAssembler)
        {
            _expressionBinding = expressionBinding;
            _categoryDomainService = categoryDomainService;
            _productRepository = productRepository;
            _attributeRepository = attributeRepository;
            _attributeValueRepository = attributeValueRepository;
            _productSellerRepository = productSellerRepository;
            _productAttributeRepository = productAttributeRepository;
            _merchantCommunicator = merchantCommunicator;
            _productCategoryRepository = productCategoryRepository;
            _categoryAttributeRepository = categoryAttributeRepository;
            _attributeMapRepository = attributeMapRepository;
            _categoryRepository = categoryRepository;
            _productService = productService;
            _productAssembler = productAssembler;
            _httpContextAccessor = httpContextAccessor;
            _productVariantService = productVariantService;
            _identityContext = identityContext;
            _merhantCommunicator = merhantCommunicator;
            _searchCommunicator = searchCommunicator;
            _categoryRepository = categoryRepository;
            _productSellerRepository = productSellerRepository;
            _productImageRepository = productImageRepository;
            _brandRepository = brandRepository;
            _atributeRepository = atributeRepository;
            _categoryService = categoryService;
            _generalAssembler = generalAssembler;

        }
        public async Task<ResponseBase<GetProductsFilterQueryResult>> GetProductListAndFilterV2(GetProductsFilterQuery request)
        {
            var response = new ResponseBase<GetProductsFilterQueryResult>();
            var selectFiltersList = new List<SelectedFilters>();
            var favoriteList = new List<FavoriteProductsList>();
            var variantableProductList = new List<Guid>();
            var customerId = new Guid?();
            var categoryList = new List<Guid>();

            bool productSearch = false;
            Enum.TryParse(_httpContextAccessor.HttpContext.Request.Headers["X-ChannelCode"], out ProductChannelCode productChannelCode);
            #region Search için.(Yapılıyor)
            if (request.FilterModel != null)
            {
                if (request.FilterModel.Any(f => f.FilterField == ProductFilterEnum.Code.ToString()))
                {
                    request.OrderBy = OrderBy.None;
                    productSearch = true;
                }
            }
            if (!String.IsNullOrEmpty(request.Query))
            {
                if ((request.FilterModel == null || request.FilterModel.Count < 1) || (request.FilterModel != null && request.IsSellerSearch))
                {
                    SearchRequest searchRequest = new SearchRequest()
                    {
                        Message = "{\"q\": \"" + request.Query + "\", \"d\": \"False\"}",//\"l\": 500,
                        UserId = !String.IsNullOrEmpty(_identityContext.GetUserInfo().Id.ToString()) ? _identityContext.GetUserInfo().Id.ToString() : "113223"
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
                            FilterField = ProductFilterEnum.Code.ToString(),
                            Id = item.ID_match,
                            Type = ProductFilterEnum.Product.ToString(),
                            IsSelectedFilter = request.FilterModel.Where(j => j.FilterField == ProductFilterEnum.Code.ToString()).Any(y => y.Id == item.ID_match) ? true : false,
                        };
                        request.FilterModel.Add(filterModel);
                    }
                    productSearch = true;
                    //webse yapmasin.
                    if (productChannelCode != ProductChannelCode.Web) request.OrderBy = OrderBy.None;
                }
            }
            #endregion
            if (!request.FilterModel.Any())
                throw new BusinessRuleException(ApplicationMessage.CategoryIdAndFilterModelNotEmpty,
                    ApplicationMessage.CategoryIdAndFilterModelNotEmpty.Message(),
                    ApplicationMessage.CategoryIdAndFilterModelNotEmpty.UserMessage());


            if (request.FilterModel.Any(y => y.FilterField == ProductFilterEnum.CategoryId.ToString()))
            {
                categoryList.AddRange(from item in request.FilterModel where item.FilterField == ProductFilterEnum.CategoryId.ToString() select new Guid(item.Id));
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

            request.FilterModel?.ForEach(y => selectFiltersList.Add(new SelectedFilters { Id = y.Id, FilterField = y.FilterField, Type = y.Type, IsSelectedFilter = y.IsSelectedFilter }));

            var newRequet = new GetProductList
            {
                IsSellerVisible = request.IsSellerVisible,
                CategoryIds = categoryList,
                FilterModel = request.FilterModel,
                OrderBy = request.OrderBy,
                PagerInput = request.PagerInput,
                ProductChannelCode = productChannelCode == 0 ? ProductChannelCode.Mobile : productChannelCode
            };

            //var bannedSellers = (await _merhantCommunicator.GetBannedSellers()).Data;

            var categorySubList = await _categoryDomainService.GetLeafCategoriesV3(newRequet.CategoryIds);

            var getProductModel = ArrangeSpParameters(newRequet, ProductFilterEnum.Product, categorySubList);

            //var sellerList = (from item in request.FilterModel where item.FilterField == ProductFilterEnum.SellerId.ToString() && !bannedSellers.Contains(new Guid(item.Id)) select new Guid(item.Id)).ToList();

            var channelCode = newRequet.ProductChannelCode.GetHashCode() == 0 ? 1 : newRequet.ProductChannelCode.GetHashCode();
            var productFilterQuery = CreateProductFilterSpString(request.PagerInput,
                getProductModel.categorySubList.Select(u => u.Id).ToList(),
                getProductModel.attributeAllIdList, getProductModel.salePriceList, getProductModel.brandIdList,
                getProductModel.codeList,
                getProductModel.searchList, request.OrderBy,
                channelCode, getProductModel.sellerIdList);
            var productFilterCountQuery = CreateProductFilterCountSpString(request.PagerInput,
                getProductModel.categorySubList.Select(u => u.Id).ToList(),
                getProductModel.attributeAllIdList, getProductModel.salePriceList, getProductModel.brandIdList,
                getProductModel.codeList,
                getProductModel.searchList, request.OrderBy,
                channelCode, getProductModel.sellerIdList);

            var query = await _productRepository.GetProductAllRelationsSP(productFilterQuery, productFilterCountQuery);

            var attributeFilterQuery = CreateAttributeFilterSpString(request.PagerInput,
                getProductModel.categorySubList.Select(u => u.Id).ToList(),
                getProductModel.attributeAllIdList, getProductModel.salePriceList, getProductModel.brandIdList,
                getProductModel.codeList,
                getProductModel.searchList, request.OrderBy,
                channelCode, getProductModel.sellerIdList);

            var attributeList = await _atributeRepository.GetProductAttributeFilter(attributeFilterQuery);

            //if (!query.ProductList.Any())
            //    return new ResponseBase<GetProductsFilterQueryResult> { Data = null, Success = true };

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
            query.AllProductList = productList;
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
                query.ProductList = newProductList1;
                //query.ProductList = query.ProductList.Skip((request.PagerInput.PageIndex - 1) * request.PagerInput.PageSize).Take(request.PagerInput.PageSize).ToList();
            }
            #endregion


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
                var productFilters = new List<ProductFilterList>();
                var brandFilterQuery = CreateBrandFilterSpString(request.PagerInput,
                getProductModel.categorySubList.Select(u => u.Id).ToList(),
                getProductModel.attributeAllIdList, getProductModel.salePriceList, getProductModel.brandIdList,
                getProductModel.codeList,
                getProductModel.searchList, request.OrderBy,
                channelCode, getProductModel.sellerIdList);
                var brandList = await _productRepository.GetProductBrandFilter(brandFilterQuery);
                var catNameList = await _categoryService.GetCagetoriesIdAndNameProducts(query.ProductList.Select(o => o.Id).ToList());

                if (brandList.Any())
                {
                    var filterList = brandList.Select(item =>
                    new ProductFilter
                    {
                        SeoUrl = "/" + item.SeoName + "/" + request.FilterModel.Where(v => v.FilterField == ProductFilterEnum.BrandId.ToString()).FirstOrDefault()?.SeoUrl,
                        FilterField = ProductFilterEnum.BrandId.ToString(),
                        Id = item.BrandId.ToString(),
                        Value = item.Name,
                        Type = ProductFilterEnum.Product.ToString(),
                        IsSelectedFilter = request.FilterModel.Where(k => k.FilterField == ProductFilterEnum.BrandId.ToString()).Any(y => new Guid(y.Id) == item.BrandId) ? true : false,
                        SeoValue = item?.SeoName
                    }).ToList();
                    productFilters.AddRange(new List<ProductFilterList> { new ProductFilterList {
                    ParentName = ProductFilterParentNames.Marka.ToString(),
                    Type = (int)FilterSelectionEnum.MultiSelection,
                    Title = ProductFilterEnum.Brand.ToString(),
                    Value = filterList,
                    IsSearchField = true,
                    ParentSeoName= ProductFilterParentNames.Marka.ToString().ToLower(),
                } });
                }

                response.Data = new GetProductsFilterQueryResult
                {
                    SelectedFilters = selectFiltersList,
                    TotalCount = query.TotalCount,
                    ProductList = _productAssembler.MapToGetProductsByCategoryIdQueryResult(query.ProductList, request.OrderBy, favoriteList, variantableList, catNameList).Data,
                    ProductFilters = null,
                    QuickFilteringList = productFilters.Where(f => f.Title == ProductFilterEnum.Brand.ToString()).ToList(),
                    OrderList = _productService.GetOrderList(),
                    PageResponse = new PageResponse(query.TotalCount, new(request.PagerInput.PageIndex, request.PagerInput.PageSize)),
                    Breadcrumb = request.Breadcrumb,
                    OrderBy = request.OrderBy
                };
            }
            else
            {
                var productFilters = new List<ProductFilterList>();
                //category
                var categoryFilterQuery = CreateCategoryFilterSpString(request.PagerInput,
                getProductModel.categorySubList.Select(u => u.Id).ToList(),
                getProductModel.attributeAllIdList, getProductModel.salePriceList, getProductModel.brandIdList,
                getProductModel.codeList,
                getProductModel.searchList, request.OrderBy,
                channelCode, getProductModel.sellerIdList);

                productFilters.AddRange(await GetProductFilterforCategoryV3(newRequet, categoryFilterQuery));

                //brand
                var brandFilterQuery = CreateBrandFilterSpString(request.PagerInput,
                getProductModel.categorySubList.Select(u => u.Id).ToList(),
                getProductModel.attributeAllIdList, getProductModel.salePriceList, getProductModel.brandIdList,
                getProductModel.codeList,
                getProductModel.searchList, request.OrderBy,
                channelCode, getProductModel.sellerIdList);
                var brandList = await _productRepository.GetProductBrandFilter(brandFilterQuery);

                if (brandList.Any())
                {
                    var filterList = brandList.Select(item =>
                    new ProductFilter
                    {
                        SeoUrl = "/" + item.SeoName + "/" + request.FilterModel.Where(v => v.FilterField == ProductFilterEnum.BrandId.ToString()).FirstOrDefault()?.SeoUrl,
                        FilterField = ProductFilterEnum.BrandId.ToString(),
                        Id = item.BrandId.ToString(),
                        Value = item.Name,
                        Type = ProductFilterEnum.Product.ToString(),
                        IsSelectedFilter = request.FilterModel.Where(j => j.FilterField == ProductFilterEnum.BrandId.ToString()).Any(y => new Guid(y.Id) == item.BrandId) ? true : false,
                        SeoValue = item?.SeoName
                    }).ToList();
                    productFilters.AddRange(new List<ProductFilterList> { new ProductFilterList {
                    ParentName = ProductFilterParentNames.Marka.ToString(),
                    Type = (int)FilterSelectionEnum.MultiSelection,
                    Title = ProductFilterEnum.Brand.ToString(),
                    Value = filterList,
                    IsSearchField = true,
                    ParentSeoName= ProductFilterParentNames.Marka.ToString().ToLower()
                } });
                }
                //attribute
                productFilters.AddRange(GetProductFilterForAttributeV2(attributeList, new GetProductList { FilterModel = request.FilterModel, SeoUrl = request.FilterModel.Select(h => h.SeoUrl).FirstOrDefault() }));
                //price
                var priceFilterQuery = CreatePriceFilterSpString(request.PagerInput,
           getProductModel.categorySubList.Select(u => u.Id).ToList(),
           getProductModel.attributeAllIdList, getProductModel.salePriceList, getProductModel.brandIdList,
           getProductModel.codeList,
           getProductModel.searchList, request.OrderBy,
           channelCode, getProductModel.sellerIdList);
                var filterPrice = new List<ProductFilter>();
                var prices = await _productRepository.GetProductPriceFilter(priceFilterQuery);
                var priceList = prices.Select(x => x.SalePrice).ToList();
                if (priceList.Any())
                {
                    if (priceList.Min() < 100)
                    {
                        filterPrice.Add(new ProductFilter
                        {
                            Id = 0 + ", " + 100,
                            Value = 0 + " TL - " + 100 + " TL",
                            FilterField = ProductFilterEnum.SalePrice.ToString(),
                            Type = ProductFilterEnum.ProductSeller.ToString(),
                            IsSelectedFilter = request.FilterModel.Where(m => m.FilterField == ProductFilterEnum.SalePrice.ToString()).Any(y => y.Id == "0,100") ? true : false,
                            SeoValue = 0 + "-" + 100
                        });
                    }
                }

                if (priceList.Where(b => b > 100).ToList().Count() > 0)
                {
                    var pList = priceList.Where(b => b > 100).ToList();
                    if (pList.Any())
                    {
                        var minPrice = pList.Min();
                        var maxPrice = pList.Max();
                        var value = Convert.ToInt32(Math.Round((maxPrice - minPrice) / 6, MidpointRounding.AwayFromZero));
                        var startValue = minPrice;
                        if (value != 0)
                        {
                            for (var i = 0; i < 6; i++)
                            {
                                startValue = startValue + value;
                                var rangeMin = startValue - value;
                                var rangeMax = startValue;
                                if (i == 5)
                                {
                                    rangeMax = maxPrice;
                                    startValue = maxPrice;
                                }
                                if (i == 0 && Math.Ceiling(Convert.ToDecimal(Math.Ceiling(startValue) / 100)) * 100 != 100)
                                {
                                    if (pList.Where(t => t >= rangeMin && t <= rangeMax).Count() > 0)
                                    {
                                        filterPrice.Add(new ProductFilter
                                        {
                                            Id = 100 + ", " + Math.Ceiling(Convert.ToDecimal(Math.Ceiling(startValue) / 100)) * 100,
                                            Value = 100 + " TL - " + Math.Ceiling(Convert.ToDecimal(Math.Ceiling(startValue) / 100)) * 100 + " TL",
                                            FilterField = ProductFilterEnum.SalePrice.ToString(),
                                            Type = ProductFilterEnum.ProductSeller.ToString(),
                                            IsSelectedFilter = request.FilterModel.Where(m => m.FilterField == ProductFilterEnum.SalePrice.ToString()).Any(y => y.Id == 100 + "," + Math.Ceiling(Convert.ToDecimal(Math.Ceiling(startValue) / 100)) * 100) ? true : false,
                                            SeoValue = 100 + "-" + Math.Ceiling(Convert.ToDecimal(Math.Ceiling(startValue) / 100)) * 100
                                        });
                                    }
                                }

                                else if (i != 0 && (Math.Ceiling(Convert.ToDecimal(Math.Ceiling(startValue - value) / 100)) * 100 != Math.Ceiling(Convert.ToDecimal(Math.Ceiling(startValue) / 100)) * 100))
                                {
                                    filterPrice.Add(new ProductFilter
                                    {
                                        Id = Math.Ceiling(Convert.ToDecimal(Math.Ceiling(startValue - value) / 100)) * 100 + ", " + Math.Ceiling(Convert.ToDecimal(Math.Ceiling(startValue) / 100)) * 100,
                                        Value = Math.Ceiling(Convert.ToDecimal(Math.Ceiling(startValue - value) / 100)) * 100 + " TL - " + Math.Ceiling(Convert.ToDecimal(Math.Ceiling(startValue) / 100)) * 100 + " TL",
                                        FilterField = ProductFilterEnum.SalePrice.ToString(),
                                        Type = ProductFilterEnum.ProductSeller.ToString(),
                                        IsSelectedFilter = request.FilterModel.Where(m => m.FilterField == ProductFilterEnum.SalePrice.ToString()).Any(y => y.Id == Math.Ceiling(Convert.ToDecimal(Math.Ceiling(startValue - value) / 100)) * 100 + "," + Math.Ceiling(Convert.ToDecimal(Math.Ceiling(startValue) / 100)) * 100) ? true : false,
                                        SeoValue = Math.Ceiling(Convert.ToDecimal(Math.Ceiling(startValue - value) / 100)) * 100 + "-" + Math.Ceiling(Convert.ToDecimal(Math.Ceiling(startValue) / 100)) * 100
                                    });
                                }

                            }
                        }
                        else
                        {
                            filterPrice.Add(new ProductFilter
                            {
                                Id = 100 + ", " + Math.Ceiling(Convert.ToDecimal(Math.Ceiling(startValue)) / 100) * 100,
                                Value = 100 + " TL - " + Math.Ceiling(Convert.ToDecimal(Math.Ceiling(startValue)) / 100) * 100 + " TL",
                                FilterField = ProductFilterEnum.SalePrice.ToString(),
                                Type = ProductFilterEnum.ProductSeller.ToString(),
                                IsSelectedFilter = request.FilterModel.Where(m => m.FilterField == ProductFilterEnum.SalePrice.ToString()).Any(y => y.Id == 100 + "," + Math.Ceiling(Convert.ToDecimal(Math.Ceiling(startValue)) / 100) * 100) ? true : false,
                                SeoValue = 100 + "-" + Math.Ceiling(Convert.ToDecimal(Math.Ceiling(startValue)) / 100) * 100,
                            });
                        }
                    }

                }

                productFilters.Add(new ProductFilterList
                {
                    ParentName = ProductFilterParentNames.Fiyat.ToString(),
                    Type = (int)FilterSelectionEnum.RangeSelection,
                    Title = ProductFilterEnum.Price.ToString(),
                    Value = filterPrice,
                    IsSearchField = false,
                    ParentSeoName = ProductFilterParentNames.Fiyat.ToString().ToLower()
                });

                var catNameList = await _categoryService.GetCagetoriesIdAndNameProducts(query.ProductList.Select(o => o.Id).ToList());


                if (!request.IsSellerVisible)
                {
                    var sellerFilterQuery = CreateSellerFilterSpString(request.PagerInput,
            getProductModel.categorySubList.Select(u => u.Id).ToList(),
            getProductModel.attributeAllIdList, getProductModel.salePriceList, getProductModel.brandIdList,
            getProductModel.codeList,
            getProductModel.searchList, request.OrderBy,
            channelCode, getProductModel.sellerIdList);
                    var sellerIdList = await _productRepository.GetProductSellerFilter(sellerFilterQuery);


                    if (sellerIdList.Any())
                    {
                        var merchantRequest = new GetSellerFilterInfosRequest();
                        var filter = new List<ProductFilter>();
                        merchantRequest.SellerIdList = sellerIdList.Select(x => x.SellerId).ToList();


                        var sellers = await _merchantCommunicator.GetProductSellersWithFilters(merchantRequest);

                        if (sellers.Data?.SellerInfoLists != null)
                        {
                            filter.AddRange(sellers.Data.SellerInfoLists.Select(item => new ProductFilter
                            {
                                SeoUrl = request.SeoUrl + "?" + item.Name + "=" + item.SeoName,
                                FilterField = ProductFilterEnum.SellerId.ToString(),
                                Id = item.Id.ToString(),
                                Value = item.CompanyName,
                                Type = ProductFilterEnum.ProductSeller.ToString(),
                                IsSelectedFilter = request.FilterModel.Where(m => m.FilterField == ProductFilterEnum.SellerId.ToString()).Any(y => new Guid(y.Id) == item.Id) ? true : false,
                                SeoValue = item.SeoName
                            }));
                        }

                        productFilters.Add(new ProductFilterList
                        {
                            ParentName = ProductFilterParentNames.Satıcı.ToString(),
                            Type = (int)FilterSelectionEnum.MultiSelection,
                            Title = ProductFilterEnum.ProductSeller.ToString(),
                            Value = filter.GroupBy(u => u.Id, (k, g) => new { Key = k, Value = g.FirstOrDefault() })
                                    .Select(y => y.Value).ToList(),
                            IsSearchField = true,
                            ParentSeoName = ProductFilterParentNames.Satıcı.ToString().ToLower()
                        });
                    }
                }

                response.Data = new GetProductsFilterQueryResult
                {
                    SelectedFilters = selectFiltersList,
                    TotalCount = query.TotalCount,
                    ProductList = _productAssembler.MapToGetProductsByCategoryIdQueryResult(query.ProductList,
                request.OrderBy, favoriteList, variantableList, catNameList).Data,
                    ProductFilters = productFilters,
                    QuickFilteringList = productFilters.Where(f => f.Title == ProductFilterEnum.Brand.ToString())
                .ToList(),
                    OrderList = _productService.GetOrderList(),
                    PageResponse = new PageResponse(query.TotalCount, new(request.PagerInput.PageIndex,
            request.PagerInput.PageSize)),
                    Breadcrumb = request.Breadcrumb,
                    OrderBy = request.OrderBy

                };
            }
            response.Success = true;
            return response;
        }
        public async Task<List<ProductFilterList>> GetProductFilter(GetProductList request, List<Product> productList,
            List<Guid> bannedSellers, List<Category> categorySubList, bool quickFilter)
        {
            var result = new List<ProductFilterList>();
            if (quickFilter)
            {
                var getExpressionModel = GetProductListExpressions(request, ProductFilterEnum.Brand, categorySubList);
                result.AddRange(await GetProductFilterBrand(getExpressionModel, bannedSellers, request.ProductChannelCode));
                return result;
            }
            else
            {
                var getExpressionModel = GetProductListExpressions(request, ProductFilterEnum.Brand, categorySubList);
                result.AddRange(await GetProductFilterBrand(getExpressionModel, bannedSellers, request.ProductChannelCode));
                if (categorySubList.Count > 0)
                    result.AddRange(await GetProductFilterForAttribute(productList, categorySubList.Select(u => u.Id).ToList()));
                else
                    result.AddRange(await GetProductFilterForAttribute(productList, new List<Guid>()));
                getExpressionModel = GetProductListExpressions(request, ProductFilterEnum.SalePrice, categorySubList);
                result.AddRange(await GetProductFilterPrice(request, getExpressionModel, bannedSellers, request.ProductChannelCode));

                if (!request.IsSellerVisible)
                {
                    getExpressionModel = GetProductListExpressions(request, ProductFilterEnum.ProductSeller, categorySubList);
                    result.AddRange(await GetProductFilterBySeller(getExpressionModel, bannedSellers, request.ProductChannelCode));
                }
                result.AddRange(await GetProductFilterforCategory(request, productList));
                return result;
            }
        }

        public async Task<List<ProductFilterList>> GetProductFilterV2(GetProductList request,
             List<Guid> bannedSellers, List<Category> categorySubList, bool quickFilter, List<AttributeFilter> attributeFilterList, FilterResponseModel filterResponseModel, int channelCode, GetProductsFilterQuery product)
        {
            var result = new List<ProductFilterList>();
            if (quickFilter)
            {
                var getProductModel = ArrangeSpParameters(request, ProductFilterEnum.Brand, categorySubList);
                result.AddRange(await GetProductFilterBrandV2(getProductModel, bannedSellers, request.ProductChannelCode, product));
                return result;
            }
            else
            {
                result.AddRange(await GetProductFilterforCategoryV2(request, filterResponseModel, channelCode, bannedSellers));

                var getProductModel = ArrangeSpParameters(request, ProductFilterEnum.Brand, categorySubList);
                result.AddRange(await GetProductFilterBrandV2(getProductModel, bannedSellers, request.ProductChannelCode, product));

                getProductModel = ArrangeSpParameters(request, ProductFilterEnum.SalePrice, categorySubList);
                result.AddRange(await GetProductFilterPriceV2(request, getProductModel, bannedSellers, request.ProductChannelCode));

                if (!request.IsSellerVisible)
                {
                    getProductModel = ArrangeSpParameters(request, ProductFilterEnum.ProductSeller, categorySubList);
                    result.AddRange(await GetProductFilterBySellerV2(getProductModel, bannedSellers, request.ProductChannelCode));
                }
                result.AddRange(GetProductFilterForAttributeV2(attributeFilterList, request));

                return result;
            }
        }

        public ExpressionsModel GetProductListExpressions(GetProductList request, ProductFilterEnum productFilterEnum,
            List<Category> categorySubList)
        {
            List<Expression<Func<Product, bool>>> expressionProductList = new List<Expression<Func<Product, bool>>>();
            List<Expression<Func<ProductSeller, bool>>> expressionProductSellersList =
                new List<Expression<Func<ProductSeller, bool>>>();
            Expression<Func<Product, bool>> expressionAllProduct = null;
            Expression<Func<ProductSeller, bool>> expressionAllProductSeller = null;
            var attributeAllIdList = new List<List<Guid>>();
            var attributeList = new List<Guid>();


            var listAttribute = request.FilterModel
                         .Where(y => y.FilterField.Split('-')[0] == ProductFilterEnum.Attribute.ToString()).GroupBy(y => y.Type,
                             (k, g) => new
                             {
                                 Key = k,
                                 Value = g.ToList()
                             }).ToDictionary(u => u.Key, u => u.Value);

            foreach (var item in listAttribute)
            {
                foreach (var item1 in item.Value)
                {
                    attributeList.Add(new Guid(item1.Id));
                }
                attributeAllIdList.Add(attributeList);
                attributeList = new List<Guid>();
            }


            var listProduct = request.FilterModel.Where(y => y.Type == ProductFilterEnum.Product.ToString()).GroupBy(
                y => y.FilterField, (k, g) => new
                {
                    Key = k,
                    Value = g.ToList()
                }).ToDictionary(u => u.Key, u => u.Value);

            foreach (var item in listProduct)
            {
                Expression<Func<Product, bool>> expressionProduct = null;
                if (productFilterEnum != ProductFilterEnum.Brand)
                {
                    if (item.Key == ProductFilterEnum.BrandId.ToString())
                    {
                        expressionProduct = _expressionBinding.GenericExpressionBinding<Product>(item.Value,
                            Domain.Enums.ExpressionJoint.Or, FilterOperatorEnum.IsEqualToNotNullGuid);
                        expressionProductList.Add(expressionProduct);
                    }
                }

                if (item.Key == ProductFilterEnum.Code.ToString())
                {
                    expressionProduct = _expressionBinding.GenericExpressionBinding<Product>(item.Value,
                        Domain.Enums.ExpressionJoint.Or, FilterOperatorEnum.IsEqualTo);
                    expressionProductList.Add(expressionProduct);
                }

                if (item.Key == ProductFilterEnum.Search.ToString())
                {
                    expressionProduct = _expressionBinding.GenericExpressionBinding<Product>(item.Value,
                        Domain.Enums.ExpressionJoint.Or, FilterOperatorEnum.Contains);
                    expressionProductList.Add(expressionProduct);
                }
            }

            var listSeller = request.FilterModel.Where(y => y.Type == ProductFilterEnum.ProductSeller.ToString())
                .GroupBy(y => y.FilterField, (k, g) => new
                {
                    Key = k,
                    Value = g.ToList()
                }).ToDictionary(u => u.Key, u => u.Value);

            foreach (var item in listSeller)
            {
                Expression<Func<ProductSeller, bool>> expressionProductSeller = null;
                if (productFilterEnum != ProductFilterEnum.SalePrice)
                {
                    if (item.Key == ProductFilterEnum.SalePrice.ToString())
                    {
                        expressionProductSeller = _expressionBinding.GenericExpressionBinding<ProductSeller>(item.Value,
                            Domain.Enums.ExpressionJoint.Or, FilterOperatorEnum.Between);
                        expressionProductSellersList.Add(expressionProductSeller);
                    }
                }

                if (productFilterEnum != ProductFilterEnum.ProductSeller)
                {
                    if (item.Key == ProductFilterEnum.SellerId.ToString())
                    {
                        expressionProductSeller = _expressionBinding.GenericExpressionBinding<ProductSeller>(item.Value,
                            Domain.Enums.ExpressionJoint.Or, FilterOperatorEnum.IsEqualToNotNullGuid);
                        expressionProductSellersList.Add(expressionProductSeller);
                    }
                }
            }

            expressionAllProduct =
                _expressionBinding.GenericExpressionBindingListProduct<Product>(expressionProductList,
                    Domain.Enums.ExpressionJoint.And);


            expressionAllProductSeller =
                _expressionBinding.GenericExpressionBindingListProductSeller<ProductSeller>(
                    expressionProductSellersList, Domain.Enums.ExpressionJoint.And);


            return new ExpressionsModel
            {
                categorySubList = categorySubList,
                expressionAllProduct = expressionAllProduct,
                attributeAllIdList = attributeAllIdList,
                expressionAllProductSeller = expressionAllProductSeller
            };
        }


        public List<ProductFilterList> GetProductFilterBrand(ExpressionsModel getExpressionModel,
            List<Guid> bannedSellers, ProductChannelCode code, List<Product> productList)
        {
            var result = new List<ProductFilterList>();

            if (productList.Any())
            {
                var filter = new List<ProductFilter>();

                foreach (var item in productList.GroupBy(u => u.Brand).Select(y => y.Key))
                {
                    filter.Add(new ProductFilter
                    {
                        FilterField = ProductFilterEnum.BrandId.ToString(),
                        Id = item.Id.ToString(),
                        Value = item.Name,
                        Type = ProductFilterEnum.Product.ToString(),
                        SeoValue = item?.SeoName
                    });
                }

                result.Add(new ProductFilterList
                {
                    ParentName = ProductFilterParentNames.Marka.ToString(),
                    Type = (int)FilterSelectionEnum.MultiSelection,
                    Title = ProductFilterEnum.Brand.ToString(),
                    Value = filter.GroupBy(u => u.Id, (k, g) => new { Key = k, Value = g.FirstOrDefault() })
                        .Select(y => y.Value).ToList(),
                    IsSearchField = true,
                    ParentSeoName = ProductFilterParentNames.Marka.ToString().ToLower()
                });
            }
            return result;
        }


        public async Task<List<ProductFilterList>> GetProductFilterForAttribute(List<Product> productList,
            List<Guid> categoryIdList)
        {
            var result = new List<ProductFilterList>();
            var filter = new List<ProductFilter>();
            var attributeAndValueList = new List<AttributeAndValueList>();
            var allattValueList = new List<AttributeAndValueList>();
            var valueList = new List<Guid>();
            if (categoryIdList.Count() > 0)
            {
                if (categoryIdList.Count == 1)
                {
                    var keys = await _categoryDomainService.GetCategoryAttributeList(categoryIdList.Select(p => p).FirstOrDefault());
                    if (productList.Any() && keys.Any())
                    {

                        var attributeList =
                            await _productAttributeRepository.FilterByAsync(y =>
                                productList.Select(p => p.Id).Contains(y.ProductId));

                        var groupAttribute = attributeList.GroupBy(y => y.AttributeId, (k, g) => new
                        {
                            Key = k,
                            Value = g.ToList()

                        }).ToDictionary(g => g.Key, g => g.Value.Select(y => y.AttributeValueId).Distinct().ToList());

                        var category = await _categoryRepository.FindByAsync(y => y.Id == categoryIdList.Select(u => u).FirstOrDefault());
                        if (category.Code != null)
                        {
                            var mapList = await _attributeRepository.GetAttributeMapList(groupAttribute.Select(o => o.Key).ToList());
                            var mapAttList = await _attributeMapRepository.FilterByAsync(u => mapList.Select(o => o).Contains(u.AttributeId));

                            foreach (var item in mapAttList)
                            {
                                allattValueList.Add(
                                        new
                                        AttributeAndValueList
                                        {
                                            AttributeId = item.AttributeId,
                                            AttributeValueId = item.AttributeValueId
                                        });
                            }
                        }
                        else
                        {
                            var valueAttList = await _attributeValueRepository.FilterByAsync(u => groupAttribute.Select(o => o.Key).ToList().Contains(u.AttributeId.Value));

                            foreach (var item in valueAttList)
                            {
                                allattValueList.Add(
                                        new
                                        AttributeAndValueList
                                        {
                                            AttributeId = item.AttributeId.Value,
                                            AttributeValueId = item.Id,
                                        });
                            }
                        }

                        if (allattValueList.Count > 0)
                        {
                            var attValue = await _attributeValueRepository.FilterByAsync(k => allattValueList.Select(y => y.AttributeValueId).Contains(k.Id));

                            var attributes = await _attributeRepository.FilterByAsync(k => allattValueList.Select(y => y.AttributeId).Contains(k.Id));
                            if (groupAttribute.Any())
                            {
                                foreach (var item in groupAttribute.OrderBy(o => keys[o.Key]))
                                {
                                    foreach (var value in item.Value)
                                    {
                                        filter.Add(new ProductFilter
                                        {
                                            FilterField = ProductFilterEnum.Attribute.ToString() + "-" + attributes?.Where(u => u.Id == item.Key)?.FirstOrDefault()?.DisplayName,
                                            Id = value.ToString(),
                                            Value = attValue?.Where(u => u.Id == value)?.FirstOrDefault()?.Value,
                                            Type = attributes?.Where(u => u.Id == item.Key)?.FirstOrDefault()?.DisplayName,
                                            ValueDetail = attValue?.Where(u => u.Id == value)?.FirstOrDefault()?.Unit,
                                            SeoValue = attValue?.Where(u => u.Id == value)?.FirstOrDefault()?.SeoName
                                        });
                                    }

                                    if (filter.Any())
                                        result.Add(new ProductFilterList
                                        {
                                            ParentName = attributes?.Where(u => u.Id == item.Key)?.FirstOrDefault()?.DisplayName,
                                            Type = (int)FilterSelectionEnum.MultiSelection,
                                            Title = ProductFilterEnum.Attribute.ToString(),
                                            Value = filter,
                                            IsSearchField = true,
                                            ParentSeoName = _generalAssembler.GetSeoName(attributes?.Where(u => u.Id == item.Key)?.FirstOrDefault()?.DisplayName, SeoNameType.Attribute)
                                        });
                                    filter = new List<ProductFilter>();
                                }
                            }
                            return result.ToList();
                        }
                    }
                }
                else
                {
                    var attributeList = await _categoryAttributeRepository.GetAttributeByCategoryList(categoryIdList);

                    var productAttribute = await _productAttributeRepository.FilterByAsync(u => productList.Select(o => o.Id).Distinct().ToList().Contains(u.ProductId) && attributeList.Contains(u.AttributeId));

                    var groupAttribute = productAttribute.GroupBy(y => y.AttributeId, (k, g) => new
                    {
                        Key = k,
                        Value = g.ToList()

                    }).ToDictionary(g => g.Key, g => g.Value.Select(y => y.AttributeValueId).Distinct().ToList());


                    var mapList = await _attributeRepository.GetAttributeMapList(groupAttribute.Select(o => o.Key).ToList());
                    foreach (var item in groupAttribute.Select(o => o.Key).ToList())
                    {
                        if (!mapList.Contains(item))
                        {
                            valueList.Add(item);
                        }
                    }
                    var mapAttList = await _attributeMapRepository.FilterByAsync(u => mapList.Select(o => o).Contains(u.AttributeId));

                    foreach (var item in mapAttList)
                    {
                        allattValueList.Add(
                                new
                                AttributeAndValueList
                                {
                                    AttributeId = item.AttributeId,
                                    AttributeValueId = item.AttributeValueId
                                });
                    }

                    var valueAttList = await _attributeValueRepository.FilterByAsync(u => valueList.Select(o => o).Contains(u.AttributeId.Value));

                    foreach (var item in valueAttList)
                    {
                        allattValueList.Add(
                                new
                                AttributeAndValueList
                                {
                                    AttributeId = item.AttributeId.Value,
                                    AttributeValueId = item.Id,
                                });
                    }

                    if (allattValueList.Count > 0)
                    {
                        var attValue = await _attributeValueRepository.FilterByAsync(k => allattValueList.Select(y => y.AttributeValueId).Contains(k.Id));

                        var attributes = await _attributeRepository.FilterByAsync(k => allattValueList.Select(y => y.AttributeId).Contains(k.Id));

                        if (groupAttribute.Any())
                        {
                            foreach (var item in groupAttribute)
                            {
                                foreach (var value in item.Value)
                                {
                                    filter.Add(new ProductFilter
                                    {
                                        FilterField = ProductFilterEnum.Attribute.ToString() + "-" + attributes?.Where(u => u.Id == item.Key)?.FirstOrDefault()?.DisplayName,
                                        Id = value.ToString(),
                                        Value = attValue?.Where(u => u.Id == value)?.FirstOrDefault()?.Value,
                                        Type = attributes?.Where(u => u.Id == item.Key)?.FirstOrDefault()?.DisplayName,
                                        ValueDetail = attValue?.Where(u => u.Id == value)?.FirstOrDefault()?.Unit,
                                        SeoValue = attValue?.Where(u => u.Id == value)?.FirstOrDefault()?.SeoName
                                    });
                                }

                                if (filter.Any())
                                    result.Add(new ProductFilterList
                                    {
                                        ParentName = attributes?.Where(u => u.Id == item.Key)?.FirstOrDefault()?.DisplayName,
                                        Type = (int)FilterSelectionEnum.MultiSelection,
                                        Title = ProductFilterEnum.Attribute.ToString(),
                                        Value = filter,
                                        IsSearchField = true,
                                        ParentSeoName = _generalAssembler.GetSeoName(attributes?.Where(u => u.Id == item.Key)?.FirstOrDefault()?.DisplayName, SeoNameType.Attribute)

                                    });
                                filter = new List<ProductFilter>();
                            }
                            return result.ToList();
                        }
                    }
                }

            }
            else
            {
                var productCategoryList = await _productCategoryRepository.FilterByAsync(u => productList.Select(y => y.Id).Contains(u.ProductId));
                if (productCategoryList.Count > 0)
                {
                    var attributeList = await _categoryAttributeRepository.GetAttributeByCategoryList(productCategoryList.Select(u => u.CategoryId).Distinct().ToList());

                    var productAttribute = await _productAttributeRepository.FilterByAsync(u => productList.Select(o => o.Id).Distinct().ToList().Contains(u.ProductId) && attributeList.Contains(u.AttributeId));

                    var groupAttribute = productAttribute.GroupBy(y => y.AttributeId, (k, g) => new
                    {
                        Key = k,
                        Value = g.ToList()

                    }).ToDictionary(g => g.Key, g => g.Value.Select(y => y.AttributeValueId).Distinct().ToList());


                    var mapList = await _attributeRepository.GetAttributeMapList(groupAttribute.Select(o => o.Key).ToList());
                    foreach (var item in groupAttribute.Select(o => o.Key).ToList())
                    {
                        if (!mapList.Contains(item))
                        {
                            valueList.Add(item);
                        }
                    }
                    var mapAttList = await _attributeMapRepository.FilterByAsync(u => mapList.Select(o => o).Contains(u.AttributeId));

                    foreach (var item in mapAttList)
                    {
                        allattValueList.Add(
                                new
                                AttributeAndValueList
                                {
                                    AttributeId = item.AttributeId,
                                    AttributeValueId = item.AttributeValueId
                                });
                    }

                    var valueAttList = await _attributeValueRepository.FilterByAsync(u => valueList.Select(o => o).Contains(u.AttributeId.Value));

                    foreach (var item in valueAttList)
                    {
                        allattValueList.Add(
                                new
                                AttributeAndValueList
                                {
                                    AttributeId = item.AttributeId.Value,
                                    AttributeValueId = item.Id,
                                });
                    }

                    if (allattValueList.Count > 0)
                    {
                        var attValue = await _attributeValueRepository.FilterByAsync(k => allattValueList.Select(y => y.AttributeValueId).Contains(k.Id));

                        var attributes = await _attributeRepository.FilterByAsync(k => allattValueList.Select(y => y.AttributeId).Contains(k.Id));

                        if (groupAttribute.Any())
                        {
                            foreach (var item in groupAttribute)
                            {
                                foreach (var value in item.Value)
                                {
                                    filter.Add(new ProductFilter
                                    {
                                        FilterField = ProductFilterEnum.Attribute.ToString() + "-" + attributes?.Where(u => u.Id == item.Key)?.FirstOrDefault()?.DisplayName,
                                        Id = value.ToString(),
                                        Value = attValue?.Where(u => u.Id == value)?.FirstOrDefault()?.Value,
                                        Type = attributes?.Where(u => u.Id == item.Key)?.FirstOrDefault()?.DisplayName,
                                        ValueDetail = attValue?.Where(u => u.Id == value)?.FirstOrDefault()?.Unit,
                                        SeoValue = attValue?.Where(u => u.Id == value)?.FirstOrDefault()?.SeoName
                                    });
                                }

                                if (filter.Any())
                                    result.Add(new ProductFilterList
                                    {
                                        ParentName = attributes?.Where(u => u.Id == item.Key)?.FirstOrDefault()?.DisplayName,
                                        Type = (int)FilterSelectionEnum.MultiSelection,
                                        Title = ProductFilterEnum.Attribute.ToString(),
                                        Value = filter,
                                        IsSearchField = true,
                                        ParentSeoName = _generalAssembler.GetSeoName(attributes?.Where(u => u.Id == item.Key)?.FirstOrDefault()?.DisplayName, SeoNameType.Attribute)
                                    });
                                filter = new List<ProductFilter>();
                            }
                        }
                    }
                }
            }
            return result.ToList();
        }

        public List<ProductFilterList> GetProductFilterForAttributeV2(List<AttributeFilter> attributeFilterList, GetProductList request)
        {
            var result = new List<ProductFilterList>();
            foreach (var item in attributeFilterList)
            {
                if (!result.Exists(r => r.ParentName == item.DisplayName))
                {
                    result.Add(new ProductFilterList
                    {
                        ParentName = item.DisplayName,
                        Type = (int)FilterSelectionEnum.MultiSelection,
                        Title = ProductFilterEnum.Attribute.ToString(),
                        Value = new List<ProductFilter>()
                        {
                             new ProductFilter {
                                SeoUrl = request.SeoUrl+ "?"+ item.DisplayName+ "=" + item.SeoName,
                                FilterField = ProductFilterEnum.Attribute.ToString() + "-" + item.DisplayName,
                                Id = item.AttributeValueId.ToString(),
                                Value = item.Value,
                                Type = item.DisplayName,
                                ValueDetail = item.Unit,
                                IsSelectedFilter =request.FilterModel.Where(j=>j.FilterField != ProductFilterEnum.Code.ToString() && j.FilterField != ProductFilterEnum.SalePrice.ToString()).Any(y => new Guid(y.Id) == item.AttributeValueId) ? true : false,
                                SeoValue = item?.SeoName
                            }
                        },
                        IsSearchField = true,
                        ParentSeoName = item?.AttributeSeoName
                    });


                }
                else
                {
                    var filter = result.SingleOrDefault(r => r.ParentName == item.DisplayName);
                    if (!filter.Value.Exists(f => f.Id == item.AttributeValueId.ToString()))
                    {
                        filter.Value.Add(new ProductFilter
                        {
                            SeoUrl = request.SeoUrl + "?" + item.DisplayName + "=" + item.SeoName,
                            FilterField = ProductFilterEnum.Attribute.ToString() + "-" + item.DisplayName,
                            Id = item.AttributeValueId.ToString(),
                            Value = item.Value,
                            Type = item.DisplayName,
                            ValueDetail = item.Unit,
                            IsSelectedFilter = request.FilterModel.Where(j => j.FilterField != ProductFilterEnum.Code.ToString() && j.FilterField != ProductFilterEnum.SalePrice.ToString()).Any(y => new Guid(y.Id) == item.AttributeValueId) ? true : false,
                            SeoValue = item?.SeoName

                        });
                    }
                }
            }
            return result;
        }


        public async Task<List<ProductFilterList>> GetProductFilterPrice(GetProductList request, ExpressionsModel getExpressionModel,
            List<Guid> bannedSellers, ProductChannelCode code)
        {
            var result = new List<ProductFilterList>();
            var sellerList = new List<Guid>();
            foreach (var item in request.FilterModel)
            {
                if (item.FilterField == ProductFilterEnum.SellerId.ToString() && !bannedSellers.Contains(new Guid(item.Id)))
                {
                    sellerList.Add(new Guid(item.Id));
                }
            }

            var priceList = await _productRepository.GetProductListMaxPrice(getExpressionModel.categorySubList?.Select(u => u.Id).ToList(), getExpressionModel.attributeAllIdList, getExpressionModel.expressionAllProduct, getExpressionModel.expressionAllProductSeller, bannedSellers, (int)code, sellerList);

            if (priceList.Any())
            {
                var filter = new List<ProductFilter>();
                var minPrice = priceList.Min();
                var maxPrice = priceList.Max();

                var value = Convert.ToInt32(Math.Round((maxPrice - minPrice) / 6, MidpointRounding.AwayFromZero));
                var startValue = minPrice;
                if (value != 0)
                {
                    for (var i = 0; i < 6; i++)
                    {
                        startValue = startValue + value;
                        var rangeMin = startValue - value;
                        var rangeMax = startValue;
                        if (i == 5)
                        {
                            rangeMax = maxPrice;
                            startValue = maxPrice;
                        }
                        if (i == 0)
                        {
                            if (priceList.Where(t => t >= rangeMin && t <= rangeMax).Count() > 0)
                            {
                                filter.Add(new ProductFilter
                                {
                                    Id = Convert.ToInt32(Math.Floor(startValue - value)).ToString() + ", " + Convert.ToInt32(Math.Ceiling(startValue) / 100) * 100,
                                    Value = Convert.ToInt32(Math.Floor(startValue - value)) + " TL - " + Convert.ToInt32(Math.Ceiling(startValue) / 100) * 100 + " TL",
                                    FilterField = ProductFilterEnum.SalePrice.ToString(),
                                    Type = ProductFilterEnum.ProductSeller.ToString(),
                                    SeoValue = Convert.ToInt32(Math.Floor(startValue - value)).ToString() + "-" + Convert.ToInt32(Math.Ceiling(startValue) / 100) * 100
                                });
                            }
                        }
                        else
                        {
                            if (priceList.Where(t => t >= rangeMin && t <= rangeMax).Count() > 0)
                            {
                                filter.Add(new ProductFilter
                                {
                                    Id = (Convert.ToInt32(Math.Floor(startValue - value) / 100) * 100).ToString() + ", " + Convert.ToInt32(Math.Ceiling(startValue) / 100) * 100,
                                    Value = (Convert.ToInt32(Math.Floor(startValue - value) / 100) * 100) + " TL - " + Convert.ToInt32(Math.Ceiling(startValue) / 100) * 100 + " TL",
                                    FilterField = ProductFilterEnum.SalePrice.ToString(),
                                    Type = ProductFilterEnum.ProductSeller.ToString(),
                                    SeoValue = (Convert.ToInt32(Math.Floor(startValue - value) / 100) * 100).ToString() + "-" + Convert.ToInt32(Math.Ceiling(startValue) / 100) * 100,
                                });
                            }
                        }

                    }
                }
                else
                {
                    filter.Add(new ProductFilter
                    {
                        Id = Convert.ToInt32(value).ToString() + ", " + (Convert.ToInt32(Math.Ceiling(startValue) / 100) * 100).ToString(),
                        Value = Convert.ToInt32(value).ToString() + " TL - " + (Convert.ToInt32(Math.Ceiling(startValue) / 100) * 100).ToString() + " TL",
                        FilterField = ProductFilterEnum.SalePrice.ToString(),
                        Type = ProductFilterEnum.ProductSeller.ToString(),
                        SeoValue = Convert.ToInt32(value).ToString() + "-" + (Convert.ToInt32(Math.Ceiling(startValue) / 100) * 100).ToString(),
                    });
                }


                result.Add(new ProductFilterList
                {
                    ParentName = ProductFilterParentNames.Fiyat.ToString(),
                    Type = (int)FilterSelectionEnum.RangeSelection,
                    Title = ProductFilterEnum.Price.ToString(),
                    Value = filter,
                    IsSearchField = false,
                    ParentSeoName = ProductFilterParentNames.Fiyat.ToString().ToLower()
                });
            }
            return result;
        }


        public async Task<List<ProductFilterList>> GetProductFilterBySeller(ExpressionsModel getExpressionModel,
            List<Guid> bannedSellers, ProductChannelCode code)
        {
            var result = new List<ProductFilterList>();

            var sellerIdList = await _productRepository.GetProductListGroupBySeller(getExpressionModel.categorySubList.Select(u => u.Id).ToList(), getExpressionModel.attributeAllIdList,
                getExpressionModel.expressionAllProduct, getExpressionModel.expressionAllProductSeller, bannedSellers, (int)code);

            if (sellerIdList.Any())
            {
                var merchantRequest = new GetSellerFilterInfosRequest();
                var filter = new List<ProductFilter>();
                merchantRequest.SellerIdList = sellerIdList;

                var query = await _merchantCommunicator.GetProductSellersWithFilters(merchantRequest);

                if (query.Data?.SellerInfoLists != null)
                {
                    foreach (var item in query.Data.SellerInfoLists)
                    {
                        filter.Add(new ProductFilter
                        {
                            FilterField = ProductFilterEnum.SellerId.ToString(),
                            Id = item.Id.ToString(),
                            Value = item.CompanyName,
                            Type = ProductFilterEnum.ProductSeller.ToString(),
                            SeoValue = item?.SeoName
                        });
                    }
                }

                result.Add(new ProductFilterList
                {
                    ParentName = ProductFilterParentNames.Satıcı.ToString(),
                    Type = (int)FilterSelectionEnum.MultiSelection,
                    Title = ProductFilterEnum.ProductSeller.ToString(),
                    Value = filter.GroupBy(u => u.Id, (k, g) => new { Key = k, Value = g.FirstOrDefault() })
                  .Select(y => y.Value).ToList(),
                    IsSearchField = true,
                    ParentSeoName = ProductFilterParentNames.Satıcı.ToString().ToLower()
                });
            }
            return result;
        }

        public async Task<List<ProductFilterList>> GetProductFilterforCategory(GetProductList request, List<Product> productList)
        {
            var result = new List<ProductFilterList>();
            var categoryIdList = new List<Guid>();
            var list = new List<RelatedCategories>();
            if (request.FilterModel.Where(u => u.FilterField == ProductFilterEnum.CategoryId.ToString()).Count() <= 0 || (request.FilterModel.Any(u => u.FilterField == ProductFilterEnum.CategoryId.ToString()) && request.FilterModel.Count() > 1))
            {
                list = await _categoryDomainService.GetRelatedCategoryforProductList(productList.Select(u => u.Id).ToList());
            }
            else
            {
                var catList = request.FilterModel.Where(u => u.FilterField == ProductFilterEnum.CategoryId.ToString()).Select(o => o.Id).ToList();
                catList.ForEach(u => categoryIdList.Add(new Guid(u)));
                list = await _categoryDomainService.GetRelatedCategoryV2(categoryIdList);
            }

            list = list.Where(c => c.HasProduct).ToList();

            if (list.Any())
            {
                var filter = new List<ProductFilter>();
                foreach (var item in list)
                {

                    filter.Add(new ProductFilter
                    {
                        SeoUrl = "/" + item?.SeoName + "-k-" + item?.Code,
                        FilterField = ProductFilterEnum.CategoryId.ToString(),
                        Id = item.Id.ToString(),
                        Value = item.Name,
                        Type = ProductFilterEnum.ProductCategory.ToString(),
                        IsSelectedFilter = request.FilterModel.Where(m => m.FilterField == ProductFilterEnum.CategoryId.ToString()).Any(y => new Guid(y.Id) == item.Id) ? true : false,
                        SeoValue = item?.SeoName
                    });
                }
                result.Add(new ProductFilterList { ParentName = ProductFilterParentNames.Kategori.ToString(), Type = (int)FilterSelectionEnum.MultiSelection, Title = ProductFilterEnum.Category.ToString(), Value = filter, IsSearchField = true, ParentSeoName = ProductFilterParentNames.Kategori.ToString().ToLower() });
            }
            return result;
        }

        public async Task<List<ProductFilterList>> GetProductFilterforCategoryV2(GetProductList request, FilterResponseModel getProductModel, int channelCode, List<Guid> bannedSellers)
        {
            var result = new List<ProductFilterList>();
            var categoryIdList = new List<Guid>();
            var list = new List<RelatedCategories>();
            if (request.FilterModel.Where(u => u.FilterField == ProductFilterEnum.CategoryId.ToString()).Count() <= 0
                || (request.FilterModel.Any(u => u.FilterField == ProductFilterEnum.CategoryId.ToString()) && request.FilterModel.Count() > 1))
            {
                list = await _categoryRepository.GetProductCategoryFilter(getProductModel.categorySubList.Select(u => u.Id).ToList(),
                getProductModel.sellerIdList, getProductModel.attributeAllIdList, getProductModel.salePriceList, getProductModel.brandIdList, getProductModel.codeList,
                getProductModel.searchList, bannedSellers, channelCode, getProductModel.sellerIdList);
                //list = await _categoryDomainService.GetRelatedCategoryforProductList(productList.Select(u => u.Id).ToList());
            }
            else
            {
                var catList = request.FilterModel.Where(u => u.FilterField == ProductFilterEnum.CategoryId.ToString()).Select(o => o.Id).ToList();
                catList.ForEach(u => categoryIdList.Add(new Guid(u)));
                list = await _categoryDomainService.GetRelatedCategoryV2(categoryIdList);
            }

            list = list.Where(c => c.HasProduct).ToList();

            if (list.Any())
            {
                var filter = new List<ProductFilter>();
                foreach (var item in list)
                {

                    filter.Add(new ProductFilter
                    {
                        SeoUrl = "/" + item?.SeoName + "-k-" + item?.Code,
                        FilterField = ProductFilterEnum.CategoryId.ToString(),
                        Id = item.Id.ToString(),
                        Value = item.Name,
                        Type = ProductFilterEnum.ProductCategory.ToString(),
                        IsSelectedFilter = request.FilterModel.Where(h => h.FilterField == ProductFilterEnum.CategoryId.ToString()).Any(y => new Guid(y.Id) == item.Id) ? true : false,
                        SeoValue = item?.SeoName
                    });
                }
                result.Add(new ProductFilterList
                {
                    ParentName = ProductFilterParentNames.Kategori.ToString(),
                    Type = (int)FilterSelectionEnum.ListSelection,
                    Title = ProductFilterEnum.Category.ToString(),
                    Value = filter,
                    IsSearchField = true,
                    ParentSeoName = ProductFilterParentNames.Kategori.ToString().ToLower()
                });
            }
            return result;
        }

        public async Task<List<ProductFilterList>> GetProductFilterforCategoryV3(GetProductList request, string categoryFilterQuery)
        {
            var result = new List<ProductFilterList>();
            var categoryIdList = new List<Guid>();
            var list = new List<RelatedCategories>();
            if (request.FilterModel.Where(u => u.FilterField == ProductFilterEnum.CategoryId.ToString()).Count() <= 0
                || (request.FilterModel.Any(u => u.FilterField == ProductFilterEnum.CategoryId.ToString()) && request.FilterModel.Count() > 1))
            {
                list = await _productRepository.GetProductCategoryFilter(categoryFilterQuery);
            }
            else
            {
                var catList = request.FilterModel.Where(u => u.FilterField == ProductFilterEnum.CategoryId.ToString()).Select(o => o.Id).ToList();
                catList.ForEach(u => categoryIdList.Add(new Guid(u)));
                list = await _categoryDomainService.GetRelatedCategoryV2(categoryIdList);
            }

            list = list.Where(c => c.HasProduct).ToList();

            if (list.Any())
            {
                var filter = new List<ProductFilter>();
                foreach (var item in list)
                {

                    filter.Add(new ProductFilter
                    {
                        SeoUrl = "/" + item?.SeoName + "-k-" + item?.Code,
                        FilterField = ProductFilterEnum.CategoryId.ToString(),
                        Id = item.Id.ToString(),
                        Value = item.Name,
                        Type = ProductFilterEnum.ProductCategory.ToString(),
                        IsSelectedFilter = request.FilterModel.Where(h => h.FilterField == ProductFilterEnum.CategoryId.ToString()).Any(y => new Guid(y.Id) == item.Id) ? true : false,
                        SeoValue = item?.SeoName
                    });
                }
                result.Add(new ProductFilterList
                {
                    ParentName = ProductFilterParentNames.Kategori.ToString(),
                    Type = (int)FilterSelectionEnum.MultiSelection,
                    Title = ProductFilterEnum.Category.ToString(),
                    Value = filter,
                    IsSearchField = true,
                    ParentSeoName = ProductFilterParentNames.Kategori.ToString().ToLower()
                });
            }
            return result;
        }

        public async Task<List<ProductFilterList>> GetProductFilterBrand(ExpressionsModel getExpressionModel, List<Guid> bannedSellers, ProductChannelCode code)
        {
            var result = new List<ProductFilterList>();

            var filter = await _productRepository.GetProductListGroupByBrand(getExpressionModel.categorySubList?.Select(u => u.Id).ToList(), getExpressionModel.attributeAllIdList, getExpressionModel.expressionAllProduct, getExpressionModel.expressionAllProductSeller, bannedSellers, (int)code);

            if (filter.Any())
            {
                result.Add(new ProductFilterList
                {
                    ParentName = ProductFilterParentNames.Marka.ToString(),
                    Type = (int)FilterSelectionEnum.MultiSelection,
                    Title = ProductFilterEnum.Brand.ToString(),
                    Value = filter,
                    IsSearchField = true,
                    ParentSeoName = ProductFilterParentNames.Marka.ToString().ToLower()
                });
            }
            return result;
        }

        public async Task<List<ProductFilterList>> GetProductFilterBrandV2(FilterResponseModel filterResponseModel, List<Guid> bannedSellers, ProductChannelCode code, GetProductsFilterQuery brand)
        {
            var result = new List<ProductFilterList>();

            var filterBrands = await _productRepository.GetProductListGroupByBrandSP(filterResponseModel.categorySubList.Select(u => u.Id).ToList(),
                filterResponseModel.attributeAllIdList, filterResponseModel.salePriceList, filterResponseModel.brandIdList, filterResponseModel.codeList,
                filterResponseModel.searchList, bannedSellers, (int)code, filterResponseModel.sellerIdList);

            if (filterBrands.Any())
            {
                var filterList = filterBrands.Select(item =>
                      new ProductFilter
                      {
                          FilterField = ProductFilterEnum.BrandId.ToString(),
                          Id = item.BrandId.ToString(),
                          Value = item.Name,
                          Type = ProductFilterEnum.Product.ToString(),
                          SeoValue = item.SeoName
                      }).ToList();

                result.AddRange(new List<ProductFilterList> { new ProductFilterList {
                    ParentName = ProductFilterParentNames.Marka.ToString(),
                    Type = (int)FilterSelectionEnum.MultiSelection,
                    Title = ProductFilterEnum.Brand.ToString(),
                    Value = filterList,
                    IsSearchField = true,
                    ParentSeoName = ProductFilterParentNames.Marka.ToString().ToLower()
                } });

            }
            return result;
        }

        public async Task<List<ProductFilterList>> GetProductFilterBySellerV2(FilterResponseModel filterResponseModel, List<Guid> bannedSellers, ProductChannelCode code)
        {
            var result = new List<ProductFilterList>();
            var sellerIdList = await _productRepository.GetProductListGroupBySellerSP(filterResponseModel.categorySubList.Select(u => u.Id).ToList(),
                 filterResponseModel.attributeAllIdList, filterResponseModel.salePriceList, filterResponseModel.brandIdList, filterResponseModel.codeList,
                filterResponseModel.searchList, bannedSellers, (int)code, filterResponseModel.sellerIdList);

            if (sellerIdList.Any())
            {
                var merchantRequest = new GetSellerFilterInfosRequest();
                var filter = new List<ProductFilter>();
                merchantRequest.SellerIdList = sellerIdList.Select(x => x.SellerId).ToList();


                var query = await _merchantCommunicator.GetProductSellersWithFilters(merchantRequest);

                if (query.Data?.SellerInfoLists != null)
                {
                    filter.AddRange(query.Data.SellerInfoLists.Select(item => new ProductFilter
                    {
                        FilterField = ProductFilterEnum.SellerId.ToString(),
                        Id = item.Id.ToString(),
                        Value = item.CompanyName,
                        Type = ProductFilterEnum.ProductSeller.ToString(),
                        SeoValue = item?.SeoName
                    }));
                }

                result.Add(new ProductFilterList
                {
                    ParentName = ProductFilterParentNames.Satıcı.ToString(),
                    Type = (int)FilterSelectionEnum.MultiSelection,
                    Title = ProductFilterEnum.ProductSeller.ToString(),
                    Value = filter.GroupBy(u => u.Id, (k, g) => new { Key = k, Value = g.FirstOrDefault() })
                       .Select(y => y.Value).ToList(),
                    IsSearchField = true,
                    ParentSeoName = ProductFilterParentNames.Satıcı.ToString().ToLower()
                });
            }
            return result;
        }

        public async Task<List<ProductFilterList>> GetProductFilterPriceV2(GetProductList request, FilterResponseModel filterResponseModel,
           List<Guid> bannedSellers, ProductChannelCode code)
        {
            var result = new List<ProductFilterList>();
            var sellerList = new List<Guid>();
            foreach (var item in request.FilterModel)
            {
                if (item.FilterField == ProductFilterEnum.SellerId.ToString() && !bannedSellers.Contains(new Guid(item.Id)))
                {
                    sellerList.Add(new Guid(item.Id));
                }
            }

            var prices = await _productRepository.GetProductListMaxPriceSP(filterResponseModel.categorySubList.Select(u => u.Id).ToList(),
                filterResponseModel.attributeAllIdList, filterResponseModel.salePriceList, filterResponseModel.brandIdList, filterResponseModel.codeList,
                filterResponseModel.searchList, bannedSellers, (int)code, filterResponseModel.sellerIdList);



            var priceList = prices.Select(x => x.SalePrice).ToList();

            //priceList.Add(Convert.ToDecimal(prices));


            if (priceList.Any())
            {
                var filter = new List<ProductFilter>();
                var minPrice = priceList.Min();
                var maxPrice = priceList.Max();

                var value = Convert.ToInt32(Math.Round((maxPrice - minPrice) / 6, MidpointRounding.AwayFromZero));
                var startValue = minPrice;
                if (value != 0)
                {
                    for (var i = 0; i < 6; i++)
                    {
                        startValue = startValue + value;
                        var rangeMin = startValue - value;
                        var rangeMax = startValue;
                        if (i == 5)
                        {
                            rangeMax = maxPrice;
                            startValue = maxPrice;
                        }
                        if (priceList.Where(t => t >= rangeMin && t <= rangeMax).Count() > 0)
                        {
                            filter.Add(new ProductFilter
                            {
                                Id = Convert.ToInt32(Math.Floor(startValue - value)).ToString() + ", " + Convert.ToInt32(Math.Ceiling(startValue)),
                                Value = Convert.ToInt32(Math.Floor(startValue - value)) + " TL - " + Convert.ToInt32(Math.Ceiling(startValue)) + " TL",
                                FilterField = ProductFilterEnum.SalePrice.ToString(),
                                Type = ProductFilterEnum.ProductSeller.ToString(),
                                SeoValue = Convert.ToInt32(Math.Floor(startValue - value)).ToString() + "-" + Convert.ToInt32(Math.Ceiling(startValue))
                            });
                        }
                    }
                }
                else
                {
                    filter.Add(new ProductFilter
                    {
                        Id = Convert.ToInt32(value).ToString() + ", " + Convert.ToInt32(Math.Ceiling(startValue)).ToString(),
                        Value = Convert.ToInt32(value).ToString() + " TL - " + Convert.ToInt32(Math.Ceiling(startValue)).ToString() + " TL",
                        FilterField = ProductFilterEnum.SalePrice.ToString(),
                        Type = ProductFilterEnum.ProductSeller.ToString(),
                        SeoValue = Convert.ToInt32(value).ToString() + "-" + Convert.ToInt32(Math.Ceiling(startValue)).ToString()
                    });
                }


                result.Add(new ProductFilterList
                {
                    ParentName = ProductFilterParentNames.Fiyat.ToString(),
                    Type = (int)FilterSelectionEnum.RangeSelection,
                    Title = ProductFilterEnum.Price.ToString(),
                    Value = filter,
                    IsSearchField = false,
                    ParentSeoName = ProductFilterParentNames.Fiyat.ToString().ToLower()
                });
            }
            return result;
        }

        public FilterResponseModel ArrangeSpParameters(GetProductList request, ProductFilterEnum productFilterEnum, List<Category> categorySubList)
        {
            var attributeAllIdList = new List<List<Guid>>();
            var attributeList = new List<Guid>();
            var brandIdList = new List<Guid>();
            var codeList = new List<string>();
            var searchList = new List<string>();
            var salePriceList = new List<string>();
            var sellerIdList = new List<Guid>();

            var listAttribute = request.FilterModel.Where(y => y.FilterField.Split('-')[0] ==
            ProductFilterEnum.Attribute.ToString()).GroupBy(y => y.Type, (k, g) => new
            {
                Key = k,
                Value = g.ToList()

            }).ToDictionary(u => u.Key, u => u.Value);

            foreach (var item in listAttribute)
            {
                attributeList.AddRange(item.Value.Select(item1 => new Guid(item1.Id)));
                attributeAllIdList.Add(attributeList);
                attributeList = new List<Guid>();
            }

            var listProduct = request.FilterModel.Where(y => y.Type ==
            ProductFilterEnum.Product.ToString()).GroupBy(y => y.FilterField, (k, g) => new
            {
                Key = k,
                Value = g.ToList()

            }).ToDictionary(u => u.Key, u => u.Value);


            foreach (var item in listProduct)
            {
                if (productFilterEnum != ProductFilterEnum.Brand)
                {
                    if (item.Key == ProductFilterEnum.BrandId.ToString())
                    {
                        brandIdList.AddRange(item.Value.Select(item1 => new Guid(item1.Id))); //in 
                    }
                }

                if (item.Key == ProductFilterEnum.Code.ToString())
                {
                    codeList.AddRange(item.Value.Select(item1 => item1.Id)); //in 
                }

                if (item.Key == ProductFilterEnum.Search.ToString())
                {
                    searchList.AddRange(item.Value.Select(item1 => item1.Id)); //displayname,name,description like
                }
            }

            var listSeller = request.FilterModel.Where(y => y.Type == ProductFilterEnum.ProductSeller.ToString())
                .GroupBy(y => y.FilterField, (k, g) => new
                {
                    Key = k,
                    Value = g.ToList()

                }).ToDictionary(u => u.Key, u => u.Value);

            foreach (var item in listSeller)
            {
                if (productFilterEnum != ProductFilterEnum.SalePrice)
                {
                    if (item.Key == ProductFilterEnum.SalePrice.ToString())
                    {
                        salePriceList.AddRange(item.Value.Select(item1 => item1.Id)); //10,20 100,200 stok>0 sale>=1
                    }
                }
                if (productFilterEnum != ProductFilterEnum.ProductSeller)
                {
                    if (item.Key == ProductFilterEnum.SellerId.ToString())
                    {
                        sellerIdList.AddRange(item.Value.Select(item1 => new Guid(item1.Id)));
                    }
                }
            }

            return new FilterResponseModel()
            {
                brandIdList = brandIdList,
                sellerIdList = sellerIdList,
                searchList = sellerIdList,
                salePriceList = salePriceList,
                codeList = codeList,
                attributeAllIdList = attributeAllIdList,
                categorySubList = categorySubList

            };
        }

        public string CreateProductFilterSpString(PagerInput pagerInput, List<Guid> categoryIdList,
                    List<List<Guid>> attributeIdList, List<string> salePriceList, List<Guid> brandIdList,
                    List<string> codeList, List<Guid> searchList, OrderBy orderBy, int productChannel, List<Guid> sellerList)
        {
            StringBuilder sp = new StringBuilder();
            StringBuilder sellerIdCondition = new StringBuilder();
            sellerIdCondition = sellerList.Any() ? sellerIdCondition.AppendFormat(" and ps.SellerId in ({0})", GenerateIdListWithComma(sellerList)) : sellerIdCondition.Append("");
            var codeListQuery = ArrangeSearchTableQuery(codeList);

            string beginning = @"
                select pp.* 
                from (select Id,MIN(RowNumber) as RowNumber 
                        from (select new.ProductId, new.code, new.CategoryId, new.GroupCode, new.RowNumber, case when new.Id is null THEN new.ProductId ELSE new.Id END as Id 
                        from (select x.ProductId, x.code, x.CategoryId, x.GroupCode,MIN(x.RowNumber) as RowNumber, (select k.ProductId from (
			select  ps.ProductId,ROW_NUMBER() over (order by {0}) as RowNumber
				from ProductAttribute pa inner join ProductSeller ps on ps.ProductId = pa.ProductId and ps.StockCount > 0 and ps.SalePrice >= 1
                     where AttributeId IN (select AttributeId from CategoryAttribute with(nolock) where CategoryId = x.CategoryId and IsVariantable = 1 and IsListed = 1)
                           and pa.productId in (select productId from ProductGroup with(nolock) where groupcode = x.groupcode and IsActive = 1) " + sellerIdCondition.ToString() + @" ) k where k.RowNumber = 1) as Id
                from (select p.*, ROW_NUMBER() over (order by {1}, {2}, p.Id asc) as RowNumber from  V_Products p " + codeListQuery.ToString() +
                " where 1=1 and p.ChannelCode = {3} ";

            switch (orderBy)
            {
                case OrderBy.Suggession:
                    sp.AppendFormat(beginning, "ps.SalePrice asc", "p.DisplayOrder asc", "p.ProductFilterOrder asc,p.SalePrice asc", productChannel);
                    break;
                case OrderBy.AscPrice:
                    sp.AppendFormat(beginning, "ps.SalePrice asc", "p.SalePrice asc", "p.Id", productChannel);
                    break;
                case OrderBy.DescPrice:
                    sp.AppendFormat(beginning, "ps.SalePrice desc", "p.SalePrice desc", "p.Id", productChannel);
                    break;
                case OrderBy.Newest:
                    sp.AppendFormat(beginning, "ps.CreatedDate asc", "p.ProductCreatedDate asc", "p.Id", productChannel);
                    break;
                case OrderBy.None:
                    sp.AppendFormat(beginning, "ps.SalePrice asc", "ct.rowNo", "p.Id", productChannel);
                    break;
                default:
                    sp.AppendFormat(beginning, "ps.SalePrice asc", "p.SalePrice asc", "p.Id", productChannel);
                    break;
            }

            if (categoryIdList.Any())
            {
                sp.AppendFormat(" and p.CategoryId in ({0})", GenerateIdListWithComma(categoryIdList));
            }

            if (attributeIdList.Any())
            {
                sp.AppendFormat(" and p.AttributeValueId in ({0})", GenerateAttributeIdListWithComma(attributeIdList));
            }

            if (brandIdList.Any())
            {
                sp.AppendFormat(" and p.BrandId in ({0})", GenerateIdListWithComma(brandIdList));
            }

            //if (codeList.Any())
            //{
            //    sp.AppendFormat(" and p.Code in ({0})", GenerateIdListWithComma(codeList));
            //}

            if (sellerList.Any())
            {
                sp.AppendFormat(" and p.SellerId in ({0})", GenerateIdListWithComma(sellerList));
            }

            if (salePriceList.Any())
            {
                sp.Append(" and (");
                var firstItem = salePriceList.First();
                foreach (var item in salePriceList)
                {
                    if (!item.Equals(firstItem))
                    {
                        sp.Append(" OR ");
                    }

                    sp.AppendFormat(" p.SalePrice BETWEEN {0} AND {1}", item.Split(",")[0], item.Split(",")[1]);
                }

                sp.Append(')');
            }

            sp.AppendFormat(
                @") x group by  x.ProductId, x.code, x.CategoryId, x.GroupCode) as new) last group by Id) pIds 
inner join Product pp  WITH(NOLOCK) on pp.Id = pIds.Id
order by pIds.RowNumber
OFFSET  {0} ROWS  FETCH NEXT {1} ROWS ONLY OPTION(RECOMPILE)", (pagerInput.PageIndex - 1) * pagerInput.PageSize, pagerInput.PageSize);

            return sp.ToString();
        }

        public string CreateProductFilterCountSpString(PagerInput pagerInput, List<Guid> categoryIdList,
            List<List<Guid>> attributeIdList, List<string> salePriceList, List<Guid> brandIdList,
            List<string> codeList, List<Guid> searchList, OrderBy orderBy, int productChannel, List<Guid> sellerList)
        {
            StringBuilder sp = new StringBuilder();
            StringBuilder sellerIdCondition = new StringBuilder();
            sellerIdCondition = sellerList.Any() ? sellerIdCondition.AppendFormat(" and ps.SellerId in ({0})", GenerateIdListWithComma(sellerList)) : sellerIdCondition.Append("");
            string beginning = @"select @rowcountOut = count( DISTINCT Id) from (
			select 
			case when new.Id is null THEN new.ProductId ELSE new.Id END as Id
			from
			(
				select x.ProductId,
				(
					select  TOP 1 pa.ProductId from ProductAttribute pa inner join ProductSeller ps on ps.ProductId = pa.ProductId and ps.StockCount>0 and ps.SalePrice>=1
					where AttributeId IN (select AttributeId from CategoryAttribute with(nolock) where CategoryId = x.CategoryId and IsVariantable = 1 and IsListed = 1)
					and pa.productId in (select productId from ProductGroup with(nolock) where groupcode = x.groupcode and IsActive = 1) "
                    + sellerIdCondition.ToString() +
                @") as Id
				from
				(
					select p.ProductId, p.code, p.CategoryId, p.GroupCode
					from  V_Products p
					where 1=1 and p.ChannelCode = {0} ";

            sp.AppendFormat(beginning, productChannel);

            if (categoryIdList.Any())
            {
                sp.AppendFormat(" and p.CategoryId in ({0})", GenerateIdListWithComma(categoryIdList));
            }

            if (attributeIdList.Any())
            {
                sp.AppendFormat(" and p.AttributeValueId in ({0})", GenerateAttributeIdListWithComma(attributeIdList));
            }

            if (brandIdList.Any())
            {
                sp.AppendFormat(" and p.BrandId in ({0})", GenerateIdListWithComma(brandIdList));
            }

            if (codeList.Any())
            {
                sp.AppendFormat(" and p.Code in ({0})", GenerateIdListWithComma(codeList));
            }

            if (sellerList.Any())
            {
                sp.AppendFormat(" and p.SellerId in ({0})", GenerateIdListWithComma(sellerList));
            }

            if (salePriceList.Any())
            {
                sp.Append(" and (");
                var firstItem = salePriceList.First();
                foreach (var item in salePriceList)
                {
                    if (!item.Equals(firstItem))
                    {
                        sp.Append(" OR ");
                    }

                    sp.AppendFormat(" p.SalePrice BETWEEN {0} AND {1}", item.Split(",")[0], item.Split(",")[1]);
                }

                sp.Append(')');
            }

            sp.Append(
                @") x
				group by  x.ProductId, x.code, x.CategoryId, x.GroupCode
			) as new
		) last OPTION(RECOMPILE)");

            return sp.ToString();
        }

        public string CreateAttributeFilterSpString(PagerInput pagerInput, List<Guid> categoryIdList,
            List<List<Guid>> attributeIdList, List<string> salePriceList, List<Guid> brandIdList,
            List<string> codeList, List<Guid> searchList, OrderBy orderBy, int productChannel, List<Guid> sellerList)
        {
            StringBuilder sp = new StringBuilder();
            string beginning = @"select attIds.AttributeId, a.DisplayName,av.SeoName, a.SeoName as AttributeSeoName, attIds.AttributeValueId, av.Value, av.Unit, av.[Order], MIN(ca.FilterOrder) as AttributeOrder 
from (select p.AttributeId,p.AttributeValueId,p.CategoryId from  V_Products p where 1=1 and p.ChannelCode = {0} ";

            sp.AppendFormat(beginning, productChannel);

            if (categoryIdList.Any())
            {
                sp.AppendFormat(" and p.CategoryId in ({0})", GenerateIdListWithComma(categoryIdList));
            }

            //if (attributeIdList.Any())
            //{
            //    sp.AppendFormat(" and p.AttributeValueId in ({0})", GenerateAttributeIdListWithComma(attributeIdList));
            //}

            if (brandIdList.Any())
            {
                sp.AppendFormat(" and p.BrandId in ({0})", GenerateIdListWithComma(brandIdList));
            }

            if (codeList.Any())
            {
                sp.AppendFormat(" and p.Code in ({0})", GenerateIdListWithComma(codeList));
            }

            if (sellerList.Any())
            {
                sp.AppendFormat(" and p.SellerId in ({0})", GenerateIdListWithComma(sellerList));
            }

            if (salePriceList.Any())
            {
                sp.Append(" and (");
                var firstItem = salePriceList.First();
                foreach (var item in salePriceList)
                {
                    if (!item.Equals(firstItem))
                    {
                        sp.Append(" OR ");
                    }

                    sp.AppendFormat(" p.SalePrice BETWEEN {0} AND {1}", item.Split(",")[0], item.Split(",")[1]);
                }

                sp.Append(')');
            }

            sp.Append(
                @" group by p.AttributeId, AttributeValueId, p.CategoryId) attIds 
				 inner join Attribute a with (nolock) on attIds.AttributeId = a.Id and a.IsActive = 1
				 inner join AttributeValue av with (nolock) on attIds.AttributeValueId = av.Id and av.IsActive = 1
				 inner join CategoryAttribute ca with (nolock) on ca.CategoryId = attIds.CategoryId and ca.AttributeId = attIds.AttributeId and ca.IsActive = 1 and ca.IsFilter = 1
				 group by attIds.AttributeId,a.DisplayName,attIds.AttributeValueId,av.Value,av.SeoName,a.SeoName,av.Unit,av.[Order] order by MIN(ca.FilterOrder),attIds.AttributeId,av.[Order]");

            return sp.ToString();
        }

        public string CreateBrandFilterSpString(PagerInput pagerInput, List<Guid> categoryIdList,
            List<List<Guid>> attributeIdList, List<string> salePriceList, List<Guid> brandIdList,
            List<string> codeList, List<Guid> searchList, OrderBy orderBy, int productChannel, List<Guid> sellerList)
        {
            StringBuilder sp = new StringBuilder();
            string beginning = @"select * from (select p.BrandId from V_Products p where 1=1 and p.ChannelCode = {0} ";

            sp.AppendFormat(beginning, productChannel);

            if (categoryIdList.Any())
            {
                sp.AppendFormat(" and p.CategoryId in ({0})", GenerateIdListWithComma(categoryIdList));
            }

            if (attributeIdList.Any())
            {
                sp.AppendFormat(" and p.AttributeValueId in ({0})", GenerateAttributeIdListWithComma(attributeIdList));
            }
            //sadece filtermodelde brand varsa açılmalı TODO:ÖZLEM

            if (brandIdList.Any() && !categoryIdList.Any() && !attributeIdList.Any() && !codeList.Any() && !sellerList.Any()
                && !salePriceList.Any())
            {
                sp.AppendFormat(" and p.BrandId in ({0})", GenerateIdListWithComma(brandIdList));
            }

            if (codeList.Any())
            {
                sp.AppendFormat(" and p.Code in ({0})", GenerateIdListWithComma(codeList));
            }

            if (sellerList.Any())
            {
                sp.AppendFormat(" and p.SellerId in ({0})", GenerateIdListWithComma(sellerList));
            }

            if (salePriceList.Any())
            {
                sp.Append(" and (");
                var firstItem = salePriceList.First();
                foreach (var item in salePriceList)
                {
                    if (!item.Equals(firstItem))
                    {
                        sp.Append(" OR ");
                    }

                    sp.AppendFormat(" p.SalePrice BETWEEN {0} AND {1}", item.Split(",")[0], item.Split(",")[1]);
                }

                sp.Append(')');
            }

            sp.Append(
                @" group by p.BrandId) brandIds 
				 inner join Brand b with (nolock) on brandIds.BrandId = b.Id and b.IsActive = 1 order by b.Name");

            return sp.ToString();
        }

        public string CreateCategoryFilterSpString(PagerInput pagerInput, List<Guid> categoryIdList,
           List<List<Guid>> attributeIdList, List<string> salePriceList, List<Guid> brandIdList,
           List<string> codeList, List<Guid> searchList, OrderBy orderBy, int productChannel, List<Guid> sellerList)
        {
            StringBuilder sp = new StringBuilder();
            string beginning = @"select c.Id,c.DisplayName as name,c.HasProduct,c.Code,c.SeoName from ( 
				   select x.CategoryId from (select p.* from  V_Products p where 1=1 and p.ChannelCode = {0} ";

            sp.AppendFormat(beginning, productChannel);

            if (categoryIdList.Any())
            {
                sp.AppendFormat(" and p.CategoryId in ({0})", GenerateIdListWithComma(categoryIdList));
            }

            if (attributeIdList.Any())
            {
                sp.AppendFormat(" and p.AttributeValueId in ({0})", GenerateAttributeIdListWithComma(attributeIdList));
            }

            if (brandIdList.Any())
            {
                sp.AppendFormat(" and p.BrandId in ({0})", GenerateIdListWithComma(brandIdList));
            }

            if (codeList.Any())
            {
                sp.AppendFormat(" and p.Code in ({0})", GenerateIdListWithComma(codeList));
            }

            if (sellerList.Any())
            {
                sp.AppendFormat(" and p.SellerId in ({0})", GenerateIdListWithComma(sellerList));
            }

            if (salePriceList.Any())
            {
                sp.Append(" and (");
                var firstItem = salePriceList.First();
                foreach (var item in salePriceList)
                {
                    if (!item.Equals(firstItem))
                    {
                        sp.Append(" OR ");
                    }

                    sp.AppendFormat(" p.SalePrice BETWEEN {0} AND {1}", item.Split(",")[0], item.Split(",")[1]);
                }

                sp.Append(')');
            }

            sp.Append(
                @") x group by  x.CategoryId) catIds inner join Category c with(nolock) on catIds.CategoryId = c.Id and c.Type = 0 and c.IsActive = 1 order by c.DisplayOrder");

            return sp.ToString();
        }

        public string CreatePriceFilterSpString(PagerInput pagerInput, List<Guid> categoryIdList,
           List<List<Guid>> attributeIdList, List<string> salePriceList, List<Guid> brandIdList,
           List<string> codeList, List<Guid> searchList, OrderBy orderBy, int productChannel, List<Guid> sellerList)
        {
            StringBuilder sp = new StringBuilder();
            string beginning = @"select p.SalePrice from V_Products p where 1=1 and p.ChannelCode = {0} ";

            sp.AppendFormat(beginning, productChannel);

            if (categoryIdList.Any())
            {
                sp.AppendFormat(" and p.CategoryId in ({0})", GenerateIdListWithComma(categoryIdList));
            }

            if (attributeIdList.Any())
            {
                sp.AppendFormat(" and p.AttributeValueId in ({0})", GenerateAttributeIdListWithComma(attributeIdList));
            }

            if (brandIdList.Any())
            {
                sp.AppendFormat(" and p.BrandId in ({0})", GenerateIdListWithComma(brandIdList));
            }

            if (codeList.Any())
            {
                sp.AppendFormat(" and p.Code in ({0})", GenerateIdListWithComma(codeList));
            }

            if (sellerList.Any())
            {
                sp.AppendFormat(" and p.SellerId in ({0})", GenerateIdListWithComma(sellerList));
            }

            //if (salePriceList.Any())
            //{
            //    sp.Append(" and (");
            //    var firstItem = salePriceList.First();
            //    foreach (var item in salePriceList)
            //    {
            //        if (!item.Equals(firstItem))
            //        {
            //            sp.Append(" OR ");
            //        }

            //        sp.AppendFormat(" p.SalePrice BETWEEN {0} AND {1}", item.Split(",")[0], item.Split(",")[1]);
            //    }

            //    sp.Append(')');
            //}

            sp.Append(
                @" group by  p.SalePrice order by  p.SalePrice");

            return sp.ToString();
        }
        public string CreateSellerFilterSpString(PagerInput pagerInput, List<Guid> categoryIdList,
           List<List<Guid>> attributeIdList, List<string> salePriceList, List<Guid> brandIdList,
           List<string> codeList, List<Guid> searchList, OrderBy orderBy, int productChannel, List<Guid> sellerList)
        {
            StringBuilder sp = new StringBuilder();
            string beginning = @"select p.SellerId from V_Products p where 1=1 and p.ChannelCode = {0} ";

            sp.AppendFormat(beginning, productChannel);

            if (categoryIdList.Any())
            {
                sp.AppendFormat(" and p.CategoryId in ({0})", GenerateIdListWithComma(categoryIdList));
            }

            if (attributeIdList.Any())
            {
                sp.AppendFormat(" and p.AttributeValueId in ({0})", GenerateAttributeIdListWithComma(attributeIdList));
            }

            if (brandIdList.Any())
            {
                sp.AppendFormat(" and p.BrandId in ({0})", GenerateIdListWithComma(brandIdList));
            }

            if (codeList.Any())
            {
                sp.AppendFormat(" and p.Code in ({0})", GenerateIdListWithComma(codeList));
            }
            //sadece filtermodelde seller varsa açılmalı TODO:ÖZLEM

            if (sellerList.Any() && !categoryIdList.Any() && !attributeIdList.Any() && !codeList.Any() && !brandIdList.Any()
                && !salePriceList.Any())
            {
                sp.AppendFormat(" and p.SellerId in ({0})", GenerateIdListWithComma(sellerList));
            }

            if (salePriceList.Any())
            {
                sp.Append(" and (");
                var firstItem = salePriceList.First();
                foreach (var item in salePriceList)
                {
                    if (!item.Equals(firstItem))
                    {
                        sp.Append(" OR ");
                    }

                    sp.AppendFormat(" p.SalePrice BETWEEN {0} AND {1}", item.Split(",")[0], item.Split(",")[1]);
                }

                sp.Append(')');
            }

            sp.Append(
                @"group by  p.SellerId order by p.SellerId");

            return sp.ToString();
        }

        public StringBuilder GenerateIdListWithComma<T>(List<T> list)
        {
            var result = new StringBuilder();
            foreach (var item in list)
            {
                result.Append('\'');
                result.Append(item);
                result.Append('\'');
                result.Append(',');
            }

            result.Length--; //remove last comma
            return result;
        }

        public StringBuilder GenerateAttributeIdListWithComma(List<List<Guid>> list)
        {
            var result = new StringBuilder();
            foreach (var attributes in list)
            {
                foreach (var value in attributes)
                {
                    result.Append('\'');
                    result.Append(value);
                    result.Append('\'');
                    result.Append(',');
                }
            }

            result.Length--; //remove last comma
            return result;
        }

        public StringBuilder ArrangeSearchTableQuery(List<string> codeList)
        {
            var result = new StringBuilder("");
            if (!codeList.Any())
                return result;
            result.Append(" inner join (Select ct.* from (VALUES ");
            var i = 1;
            foreach (var item in codeList)
            {
                i++;
                result.AppendFormat(" ('{0}',{1}),", item, i);
            }
            result.Length--;
            result.Append(" ) ct([code],[rowNo])) ct ON ct.Code = p.Code");
            return result;
        }
    }
}