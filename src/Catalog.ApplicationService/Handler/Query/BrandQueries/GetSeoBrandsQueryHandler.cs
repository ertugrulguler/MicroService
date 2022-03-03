using Catalog.ApiContract.Request.Query.BrandQueries;
using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.ApiContract.Response.Query.BrandQueries;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.ApplicationService.Assembler;
using Catalog.ApplicationService.Communicator.Merchant;
using Catalog.ApplicationService.Handler.Services;
using Catalog.Domain.AttributeAggregate;
using Catalog.Domain.BrandAggregate;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;
using Catalog.Domain.ProductAggregate.ServiceModels;
using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Catalog.ApplicationService.Handler.Query.BrandQueries
{
    public class GetSeoBrandsQueryHandler : IRequestHandler<GetSeoBrandsQuery, ResponseBase<GetProductsFilterQueryResult>>
    {

        private readonly IBrandDomainService _brandDomainService;
        private readonly ICategoryService _categoryService;
        private readonly Catalog.ApplicationService.Handler.Services.v2.IProductService _productServiceV2;
        private readonly IAttributeValueService _attributeValueService;
        private readonly IGeneralAssembler _generalAssembler;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAttributeRepository _attributeRepository;
        private readonly IMerhantCommunicator _merhantCommunicator;
        public GetSeoBrandsQueryHandler(Catalog.ApplicationService.Handler.Services.v2.IProductService productServiceV2,
            IBrandRepository brandRepository, ICategoryService categoryService, IBrandDomainService brandDomainService,
            IGeneralAssembler generalAssembler, ICategoryRepository categoryRepository,
            IAttributeValueService attributeValueService, IMerhantCommunicator merhantCommunicator,
            IAttributeRepository attributeRepository)
        {
            _categoryService = categoryService;
            _productServiceV2 = productServiceV2;
            _brandDomainService = brandDomainService;
            _attributeValueService = attributeValueService;
            _generalAssembler = generalAssembler;
            _categoryRepository = categoryRepository;
            _attributeRepository = attributeRepository;
            _merhantCommunicator = merhantCommunicator;
        }

        public async Task<ResponseBase<GetProductsFilterQueryResult>> Handle(GetSeoBrandsQuery request, CancellationToken cancellationToken)
        {
            var result = new ResponseBase<GetProductsFilterQueryResult>();
            var breadCrumbs = new List<Breadcrumb>();
            var response = new GetSeoBrands
            {
                Filter = new GetProductListAndFilterQuery()
            };
            var catId = new Guid?();
            Uri myUri = new Uri(request.Url);
            string[] uriItem = myUri.Segments;
            var code = "";
            string page = HttpUtility.ParseQueryString(myUri.Query).Get("sayfa");
            string sort = HttpUtility.ParseQueryString(myUri.Query).Get("siralama");
            string pageSize = HttpUtility.ParseQueryString(myUri.Query).Get("size");
            string brandName = System.Net.WebUtility.UrlDecode(uriItem[1]); //1 brand
            if (uriItem.Count() > 2) //ise cat var demektir
            {
                string categoryName = System.Net.WebUtility.UrlDecode(uriItem[2]); //1 kategori
                if (categoryName != null)
                {
                    if (categoryName.Contains("-k-"))
                    {
                        var cat = categoryName.Split("-k-");
                        var category = await _categoryService.GetCategetoryByName(cat[0].Contains("/") ? cat[0].Replace("/", "") : cat[0], cat[1]);
                        catId = category.Id;
                        code = category.Code;
                    }
                }
            }
            var brandList = await _brandDomainService.GetBrandName(new System.Collections.Generic.List<string> { brandName.Replace("/", "") }, true);
            var brandId = brandList?.Values.First(); //bir tane gelecek
            response.Filter = new GetProductListAndFilterQuery();
            response.Filter.FilterModel = new List<FilterModel>();
            if (brandId != null && brandId != Guid.Empty)
            {
                response.Filter.FilterModel.Add(new FilterModel
                {
                    SeoUrl = "",
                    Id = brandId.ToString(),
                    FilterField = ProductFilterEnum.BrandId.ToString(),
                    Type = ProductFilterEnum.Product.ToString()
                });
            }
            if (catId != null && catId != Guid.Empty)
            {
                response.Filter.FilterModel.Add(new FilterModel
                {
                    SeoUrl = string.Join("", myUri.Segments),
                    Id = catId.ToString(),
                    FilterField = ProductFilterEnum.CategoryId.ToString(),
                    Type = ProductFilterEnum.ProductCategory.ToString()
                });
            }
            var dic = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(myUri.Query);
            foreach (var item in dic)
            {
                if (item.Key is ("satıcı"))
                {
                    if (item.Value.Where(j => j.Contains(",")).Any())
                    {
                        var sellerValues = item.Value.ToString().Split(",");
                        foreach (var i in sellerValues)
                        {
                            var sellerId = await _merhantCommunicator.GetSellerBySeoName(i);
                            response.Filter.FilterModel.Add(new FilterModel
                            {
                                Id = sellerId.Data.SellerId.ToString(),
                                FilterField = ProductFilterEnum.SellerId.ToString(),
                                Type = ProductFilterEnum.ProductSeller.ToString()
                            });
                        }
                    }
                    else
                    {
                        var sellerId = await _merhantCommunicator.GetSellerBySeoName(item.Value);
                        if (sellerId.Data != null)
                        {
                            response.Filter.FilterModel.Add(new FilterModel
                            {
                                Id = sellerId?.Data?.SellerId.ToString(),
                                FilterField = ProductFilterEnum.SellerId.ToString(),
                                Type = ProductFilterEnum.ProductSeller.ToString()
                            });
                        }
                    }
                }
                if (item.Key is ("fiyat"))
                {
                    if (item.Value.Where(j => j.Contains(",")).Any())
                    {
                        var salePrices = item.Value.ToString().Split(",");
                        foreach (var i in salePrices)
                        {
                            response.Filter.FilterModel.Add(new FilterModel
                            {
                                Id = i.ToString().Replace("-", ",").ToString(),
                                FilterField = ProductFilterEnum.SalePrice.ToString(),
                                Type = ProductFilterEnum.ProductSeller.ToString()
                            });
                        }
                    }
                    else
                    {
                        var price = item.Value.ToString().Replace("-", ",");
                        response.Filter.FilterModel.Add(new FilterModel
                        {
                            Id = price.ToString(),
                            FilterField = ProductFilterEnum.SalePrice.ToString(),
                            Type = ProductFilterEnum.ProductSeller.ToString()
                        });
                    }

                }

                if (item.Key is not ("siralama" or "sayfa" or "marka" or "fiyat"))
                {
                    string[] values = new string[] { };
                    if (item.Value.Where(j => j.Contains("&")).Any())
                    {
                        values = item.Value.ToString().Split("&");
                    }
                    else if (item.Value.Where(j => j.Contains(",")).Any())
                    {
                        values = item.Value.ToString().Split(",");
                    }
                    else values = item.Value;
                    foreach (var i in values)
                    {
                        var attValueId = await _attributeValueService.GetAttributeValueId(i);
                        if (attValueId != null && attValueId != Guid.Empty)
                        {
                            var seo = await _attributeRepository.FilterByAsync(x => x.SeoName == item.Key && !string.IsNullOrEmpty(x.Code) && x.IsActive);
                            if (seo.Any())
                            {
                                //string categoryName = System.Net.WebUtility.UrlDecode(uriItem[2]);
                                //var cat = categoryName.Split("-k-");
                                response.Filter.FilterModel.Add(new FilterModel
                                {
                                    Id = attValueId.ToString(),
                                    FilterField = "Attribute-" + seo[0]?.DisplayName?.Trim(),
                                    Type = item.Key.FirstOrDefault().ToString()?.ToUpper() + item.Key.Substring(1)
                                });
                            }
                        }
                    }
                }
                else //tekli att valuelar
                {
                    var seo = await _attributeRepository.FilterByAsync(x => x.SeoName == item.Key && !string.IsNullOrEmpty(x.Code) && x.IsActive);
                    if (seo.Any())
                    {
                        var attValueId = await _attributeValueService.GetAttributeValueId(item.Key);
                        if (attValueId != null && attValueId != Guid.Empty)
                        {
                            response.Filter.FilterModel.Add(new FilterModel
                            {
                                SeoUrl = string.Join("", myUri.Segments),
                                Id = attValueId.ToString(),
                                FilterField = "Attribute-" + seo?[0]?.DisplayName?.Trim(),
                                Type = item.Key.FirstOrDefault().ToString()?.ToUpper() + item.Key.Substring(1)
                            });
                        }
                    }
                }
            }
            if (brandName != null)
            {
                brandName = brandName.Contains("/") ? brandName.Replace("/", "") : brandName;
                breadCrumbs.Add(new Breadcrumb
                {
                    CategoryId = brandId.Value,
                    Name = brandName.FirstOrDefault().ToString()?.ToUpper() + brandName.Substring(1),
                    Url = "/" + brandName
                });
            }
            IEnumerable<Breadcrumb> breadCrumbsList = breadCrumbs;
            IEnumerable<Breadcrumb> breadCrumbsRevert = breadCrumbsList.Reverse().ToList();
            var orderBy = _generalAssembler.GetOrderBy(sort, false);
            var res = new GetProductsFilterQuery
            {
                FilterModel = response.Filter.FilterModel,
                IsSellerSearch = true,
                IsSellerVisible = false,
                IsVisibleAllFilters = true,
                OrderBy = orderBy,
                PagerInput = new PagerInput(page == null ? 0 : Convert.ToInt32(page), pageSize == null ? 20 : Convert.ToInt32(pageSize)),
                Query = null,
                Breadcrumb = breadCrumbsRevert
            };
            result = await _productServiceV2.GetProductListAndFilterV2(res);
            return result;
        }
    }
}