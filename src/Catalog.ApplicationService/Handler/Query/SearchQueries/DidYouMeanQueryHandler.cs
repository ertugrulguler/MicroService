using Catalog.ApiContract.Request.Query.SearchQueries;
using Catalog.ApiContract.Response.Query.SearchQueries;
using Catalog.ApplicationService.Assembler;
using Catalog.ApplicationService.Communicator.Search;
using Catalog.ApplicationService.Communicator.Search.Model;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;
using Catalog.Domain.ProductAggregate;
using Framework.Core.Authorization;
using Framework.Core.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.SearchQueries
{
    public class DidYouMeanQueryHandler : IRequestHandler<DidYouMeanQuery, ResponseBase<DidYouMeanDetail>>
    {
        private readonly ISearchCommunicator _searchCommunicator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityContext _identityContext;
        private readonly IGeneralAssembler _generalAssembler;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        public DidYouMeanQueryHandler(ISearchCommunicator searchCommunicator, IProductRepository productRepository, ICategoryRepository categoryRepository,
            IGeneralAssembler generalAssembler, IHttpContextAccessor httpContextAccessor, IIdentityContext identityContext)
        {
            _searchCommunicator = searchCommunicator;
            _httpContextAccessor = httpContextAccessor;
            _identityContext = identityContext;
            _generalAssembler = generalAssembler;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public async Task<ResponseBase<DidYouMeanDetail>> Handle(DidYouMeanQuery request, CancellationToken cancellationToken)
        {
            Enum.TryParse(_httpContextAccessor.HttpContext.Request.Headers["X-ChannelCode"], out ProductChannelCode productChannelCode);
            DidYouMeanRequest searchRequest = new DidYouMeanRequest()
            {
                Message = request.Message,
                UserId = !String.IsNullOrEmpty(_identityContext.GetUserInfo().Id.ToString()) ? _identityContext.GetUserInfo().Id.ToString() : "113223",
            };
            var exist = await _searchCommunicator.DidYouMean(searchRequest);

            DidYouMeanDetail res = new DidYouMeanDetail();
            SearchData data = new SearchData();

            if (exist.ServiceOutput != null)
            {
                foreach (var item in exist.ServiceOutput)
                {
                    string seoUrl = null;
                    if (item.type == SearchItemEnum.Word.GetDisplayName().ToLower())
                    {
                        item.type = SearchItemEnum.Product.GetDisplayName().ToLower();
                    }
                    var searchType = item.type != SearchItemEnum.Product.GetDisplayName().ToLower() ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(item.type) : "Search";
                    if (String.IsNullOrEmpty(item.deeplinkUrl))
                    {
                        List<Domain.ProductAggregate.ServiceModels.FilterModel> listFilter = new List<Domain.ProductAggregate.ServiceModels.FilterModel>();
                        if (item.type != SearchItemEnum.Product.GetDisplayName().ToLower())
                        {
                            Domain.ProductAggregate.ServiceModels.FilterModel filterData = new Domain.ProductAggregate.ServiceModels.FilterModel()
                            {
                                Id = item.id.ToString(),
                                Type = item.type == SearchItemEnum.Category.GetDisplayName().ToLower() ? "ProductCategory" :
                                item.type == SearchItemEnum.Brand.GetDisplayName().ToLower() ? "Product" :
                                item.type == SearchItemEnum.Seller.GetDisplayName().ToLower() ? "ProductSeller" : "Boş",
                                FilterField = item.type == SearchItemEnum.Category.GetDisplayName().ToLower() ? "CategoryId" :
                                item.type == SearchItemEnum.Brand.GetDisplayName().ToLower() ? "BrandId" :
                                item.type == SearchItemEnum.Seller.GetDisplayName().ToLower() ? "SellerId" : "Product",
                            };
                            listFilter.Add(filterData);
                        }
                        if (searchType == SearchItemEnum.Category.GetDisplayName() && item.id != "none")
                        {
                            var cat = await _categoryRepository.GetByIdAsync(new Guid(item.id));
                            seoUrl = cat.SeoName + "-k-" + cat.Code;
                        }

                        else if (searchType == SearchItemEnum.Seller.GetDisplayName())
                            seoUrl = "magaza/" + _generalAssembler.GetSeoName(item.name, SeoNameType.Seller);

                        else if (searchType == SearchItemEnum.Brand.GetDisplayName())
                            seoUrl = _generalAssembler.GetSeoName(item.name, SeoNameType.Brand);

                        else if (searchType == SearchItemEnum.Product.GetDisplayName() && item.id != "none")
                        {
                            var pro = await _categoryRepository.GetByIdAsync(new Guid(item.id));
                            seoUrl = pro.SeoName + "-p-" + pro.Code;
                        }
                        else if (searchType == SearchItemEnum.Search.GetDisplayName())
                            seoUrl = "search?q=" + _generalAssembler.GetSeoName(item.name, SeoNameType.Search);

                        data = new SearchData()
                        {
                            SearchType = searchType,
                            SearchTypeText = item.type == SearchItemEnum.Category.GetDisplayName().ToLower() ? "Kategori" : item.type == SearchItemEnum.Brand.GetDisplayName().ToLower() ? "Marka" :
                            item.type == SearchItemEnum.Seller.GetDisplayName().ToLower() ? "Mağaza" : "",
                            CategoryName = item.type == SearchItemEnum.Category.GetDisplayName().ToLower() ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(item.name) : null,
                            SellerId = item.type == SearchItemEnum.Seller.GetDisplayName().ToLower() ? item.id : "",
                            SellerName = item.type == SearchItemEnum.Seller.GetDisplayName().ToLower() ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(item.name) : "",
                            Filter = new FilterData()
                            {
                                Query = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(item.name),
                                FilterModel = item.type != SearchItemEnum.Product.GetDisplayName().ToLower() ? listFilter : null,
                            },
                            SeoUrl = seoUrl

                        };
                    }
                    else
                    {
                        data = new SearchData()
                        {
                            SearchType = "MaximumMobil",
                            SearchTypeText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(item.name),
                            DeeplinkData = new DeeplinkData()
                            {
                                Id = item.deeplinkId,
                                DeepLink = item.deeplinkUrl
                            },
                        };


                    }
                    res.Search.Add(data);
                }
            }
            return new ResponseBase<DidYouMeanDetail> { Data = res, Success = true };
        }
        public static DidYouMeanDetail DamiData(DidYouMeanQuery req)
        {
            #region FilterModel
            Domain.ProductAggregate.ServiceModels.FilterModel filterProduct = new Domain.ProductAggregate.ServiceModels.FilterModel()
            {
                Id = "69E7E6BA-4952-4DC0-4533-08D92131D7EC",
                Type = "Product",
                FilterField = "Id",
            };
            List<Domain.ProductAggregate.ServiceModels.FilterModel> listFilterProducts = new List<Domain.ProductAggregate.ServiceModels.FilterModel>();
            listFilterProducts.Add(filterProduct);
            Domain.ProductAggregate.ServiceModels.FilterModel filterBrand = new Domain.ProductAggregate.ServiceModels.FilterModel()
            {
                Id = "69E7E6BA-4952-4DC0-4533-08D92131D7EC",
                Type = "Product",
                FilterField = "BrandId",
            };
            List<Domain.ProductAggregate.ServiceModels.FilterModel> listFilterBrand = new List<Domain.ProductAggregate.ServiceModels.FilterModel>();
            listFilterBrand.Add(filterBrand);
            Domain.ProductAggregate.ServiceModels.FilterModel filterSeller = new Domain.ProductAggregate.ServiceModels.FilterModel()
            {
                Id = "69E7E6BA-4952-4DC0-4533-08D92131D7EC",
                Type = "ProductSeller",
                FilterField = "SellerId",
            };
            List<Domain.ProductAggregate.ServiceModels.FilterModel> listFilterSeller = new List<Domain.ProductAggregate.ServiceModels.FilterModel>();
            listFilterSeller.Add(filterSeller);
            Domain.ProductAggregate.ServiceModels.FilterModel filterCategory = new Domain.ProductAggregate.ServiceModels.FilterModel()
            {
                Id = "753a3a6f-82cc-4331-b90c-229a0e34de61",
                Type = "Product",
                FilterField = "CategoryId",
            };
            List<Domain.ProductAggregate.ServiceModels.FilterModel> listFilterCategory = new List<Domain.ProductAggregate.ServiceModels.FilterModel>();
            listFilterCategory.Add(filterCategory);
            #endregion
            SearchData data = new SearchData()
            {
                SearchType = "Category",
                SearchTypeText = "Kategori",
                CategoryName = "Android Telefonlar",
                Filter = new FilterData()
                {
                    Query = "Samsung",
                    FilterModel = listFilterCategory,
                }
            };
            SearchData data1 = new SearchData()
            {
                SearchType = "Brand",
                SearchTypeText = "Marka",
                Filter = new FilterData()
                {
                    Query = "Samsung",
                    FilterModel = listFilterBrand,
                }
            };
            SearchData data2 = new SearchData()
            {
                SearchType = "Seller",
                SearchTypeText = "Mağaza",
                Filter = new FilterData()
                {
                    Query = "Samsung",
                    FilterModel = listFilterSeller,
                }
            };
            SearchData data3 = new SearchData()
            {
                SearchType = "Search",
                SearchTypeText = "",
                DeeplinkData = null,
                Filter = new FilterData()
                {
                    Query = "Samsung",
                }
            };
            SearchData data4 = new SearchData()
            {
                SearchType = "MaximumMobil",
                SearchTypeText = "Para Gönder",
                DeeplinkData = new DeeplinkData()
                {
                    Id = 18,
                    DeepLink = "https://www.maximum.com.tr/indir/?sl=kkp"
                },
            };
            DidYouMeanDetail res = new DidYouMeanDetail();
            res.Search.Add(data);
            res.Search.Add(data1);
            res.Search.Add(data2);
            res.Search.Add(data3);
            res.Search.Add(data4);
            return res;
        }
    }
}
