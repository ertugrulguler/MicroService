using Catalog.ApiContract.Request.Query.CategoryQueries;
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
    public class GetSeoCategoriesQueryHandler : IRequestHandler<GetSeoCategoriesQuery, ResponseBase<GetProductsFilterQueryResult>>
    {
        private readonly ICategoryService _categoryService;
        private readonly Catalog.ApplicationService.Handler.Services.v2.IProductService _productServiceV2;
        private readonly IAttributeValueService _attributeValueService;
        private readonly IGeneralAssembler _generalAssembler;
        private readonly IBrandDomainService _brandDomainService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAttributeRepository _attributeRepository;
        private readonly IMerhantCommunicator _merhantCommunicator;
        public GetSeoCategoriesQueryHandler(Catalog.ApplicationService.Handler.Services.v2.IProductService productServiceV2,
            IBrandRepository brandRepository, ICategoryService categoryService, IBrandDomainService brandDomainService,
            IGeneralAssembler generalAssembler, ICategoryRepository categoryRepository,
            IAttributeValueService attributeValueService, IAttributeRepository attributeRepository,
            IMerhantCommunicator merhantCommunicator)
        {
            _categoryService = categoryService;
            _productServiceV2 = productServiceV2;
            _attributeValueService = attributeValueService;
            _generalAssembler = generalAssembler;
            _brandDomainService = brandDomainService;
            _categoryRepository = categoryRepository;
            _attributeRepository = attributeRepository;
            _merhantCommunicator = merhantCommunicator;
        }

        public async Task<ResponseBase<GetProductsFilterQueryResult>> Handle(GetSeoCategoriesQuery request, CancellationToken cancellationToken)
        {
            var result = new ResponseBase<GetProductsFilterQueryResult>();
            var response = new GetSeoBrands
            {
                Filter = new GetProductListAndFilterQuery()
            };
            response.Filter = new GetProductListAndFilterQuery();
            response.Filter.FilterModel = new List<FilterModel>();
            var catId = new Guid?();
            var code = "";
            var brandId = new Guid?();
            var breadCrumbs = new List<Breadcrumb>();
            List<string> list = new List<string>();
            Uri myUri = new Uri(request.Url);
            string[] segments = myUri.Segments;
            var url = HttpUtility.UrlDecode(myUri.Query, System.Text.Encoding.UTF8);
            string page = HttpUtility.ParseQueryString(myUri.Query).Get("sayfa");
            string sort = HttpUtility.ParseQueryString(myUri.Query).Get("siralama");
            string pageSize = HttpUtility.ParseQueryString(myUri.Query).Get("size");
            if (segments.Count() > 2)
            {
                string brandName = System.Net.WebUtility.UrlDecode(segments[1]); //brandi
                var brandList = await _brandDomainService.GetBrandName(new System.Collections.Generic.List<string> { brandName.Replace("/", "") }, true);
                brandId = brandList?.Values.First(); //bir tane gelecek
                string categoryName = System.Net.WebUtility.UrlDecode(segments[2]); //kategorisi
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
            else
            {
                string categoryName = System.Net.WebUtility.UrlDecode(segments[1]); //kategorisi
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

            if (brandId != null && brandId != Guid.Empty)
            {
                string categoryName = segments.Count() > 2 ? System.Net.WebUtility.UrlDecode(segments[2]) : System.Net.WebUtility.UrlDecode(segments[1]);
                var cat = categoryName.Split("-k-");
                response.Filter.FilterModel.Add(new FilterModel
                {
                    SeoUrl = string.Join("", cat[0] + "-k-" + code),
                    Id = brandId.ToString(),
                    FilterField = ProductFilterEnum.BrandId.ToString(),
                    Type = ProductFilterEnum.Product.ToString()
                });
            }
            if (catId != null && catId != Guid.Empty)
            {
                string categoryName = segments.Count() > 2 ? System.Net.WebUtility.UrlDecode(segments[2]) : System.Net.WebUtility.UrlDecode(segments[1]);
                var cat = categoryName.Split("-k-");
                response.Filter.FilterModel.Add(new FilterModel
                {
                    SeoUrl = string.Join("", cat[0] + "-k-" + code),
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
                            if (sellerId.Data != null)
                            {
                                response.Filter.FilterModel.Add(new FilterModel
                                {
                                    Id = sellerId.Data.SellerId.ToString(),
                                    FilterField = ProductFilterEnum.SellerId.ToString(),
                                    Type = ProductFilterEnum.ProductSeller.ToString()
                                });
                            }
                        }
                    }
                    else
                    {
                        var sellerId = await _merhantCommunicator.GetSellerBySeoName(item.Value);
                        if (sellerId.Data != null)
                        {
                            response.Filter.FilterModel.Add(new FilterModel
                            {
                                Id = sellerId.Data.SellerId.ToString(),
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
                                string categoryName = segments.Count() > 2 ? System.Net.WebUtility.UrlDecode(segments[2]) : System.Net.WebUtility.UrlDecode(segments[1]);
                                var cat = categoryName.Split("-k-");
                                response.Filter.FilterModel.Add(new FilterModel
                                {
                                    SeoUrl = string.Join("", cat[0] + "-k-" + code),
                                    Id = attValueId.ToString(),
                                    FilterField = "Attribute-" + seo?[0]?.DisplayName?.Trim(),
                                    Type = i.First().ToString().ToUpper() + i.Substring(1)
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
                        var attValueId = await _attributeValueService.GetAttributeValueId(item.Value);
                        if (attValueId != null && attValueId != Guid.Empty)
                        {
                            response.Filter.FilterModel.Add(new FilterModel
                            {
                                SeoUrl = string.Join("", myUri.Segments),
                                Id = attValueId.ToString(),
                                FilterField = "Attribute-" + seo?[0]?.DisplayName?.Trim(),
                                Type = item.Key.First().ToString().ToUpper() + item.Key.Substring(1)
                            });
                        }
                    }
                }

                if (item.Key is ("marka"))
                {
                    if (item.Value.Where(j => j.Contains(",")).Any())
                    {
                        var brands = item.Value.ToString().Split(",");
                        var listBrand = new List<string>();
                        foreach (var i in brands)
                        {
                            listBrand.Add(i);
                        }
                        var b = await _brandDomainService.GetBrandName(listBrand, true);
                        if (b.Any())
                        {
                            foreach (var h in b)
                            {
                                if (h.Value != Guid.Empty)
                                {
                                    response.Filter.FilterModel.Add(new FilterModel
                                    {
                                        SeoUrl = string.Join("", myUri.Segments),
                                        Id = h.Value.ToString(),
                                        FilterField = ProductFilterEnum.BrandId.ToString(),
                                        Type = ProductFilterEnum.Product.ToString()
                                    });
                                }
                            }
                        }
                    }
                    else //tekli markadır
                    {
                        var b = await _brandDomainService.GetBrandName(new System.Collections.Generic.List<string> { item.Key.Replace("/", "") }, true);
                        var bId = b?.Values.First(); //bir tane gelecek
                        response.Filter.FilterModel = new List<FilterModel>();
                        if (brandId != null || brandId != Guid.Empty)
                        {
                            response.Filter.FilterModel.Add(new FilterModel
                            {
                                SeoUrl = string.Join("", myUri.Segments),
                                Id = bId.ToString(),
                                FilterField = ProductFilterEnum.BrandId.ToString(),
                                Type = ProductFilterEnum.Product.ToString(),
                            });
                        }
                    }
                }
            }
            var catList = new List<Category>();
            if (catId != null)
            {
                var leafCategory = await _categoryRepository.FindByAsync(c => c.Id == catId);
                catList.Add(leafCategory);
                var parentId = leafCategory.ParentId;
                while (parentId != null)
                {
                    var parentCategories = await _categoryRepository.FindByAsync(c => c.Id == parentId);
                    catList.Add(parentCategories);
                    if (parentCategories.ParentId == null)
                    {
                        break;
                    }

                    parentId = parentCategories.ParentId;

                }
                foreach (var category in catList)
                {
                    if (catList.First().Id != category.Id)
                    {
                        breadCrumbs.Add(new Breadcrumb
                        {
                            CategoryId = category.Id,
                            Name = category.DisplayName?.Trim(),
                            Url = category.SeoName + "-k-" + category.Code
                        });
                    }
                    else
                    {
                        var cat = await _categoryRepository.GetByIdAsync(category.Id);
                        breadCrumbs.Add(new Breadcrumb
                        {
                            CategoryId = category.Id,
                            Name = cat.DisplayName?.Trim(),
                            Url = cat.SeoName + "-k-" + cat.Code
                        });
                    }

                }
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