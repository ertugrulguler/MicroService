using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.ApiContract.Response.Query.BrandQueries;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.ApiContract.Response.Query.SearchQueries;
using Catalog.ApplicationService.Assembler;
using Catalog.ApplicationService.Communicator.Merchant;
using Catalog.ApplicationService.Handler.Services;
using Catalog.Domain.AttributeAggregate;
using Catalog.Domain.BrandAggregate;
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
    public class GetSeoSearchValueQueryHandler : IRequestHandler<GetSeoSearchValueQuery, ResponseBase<GetProductsFilterQueryResult>>
    {
        private readonly ICategoryService _categoryService;
        private readonly Catalog.ApplicationService.Handler.Services.v2.IProductService _productServiceV2;
        private readonly IAttributeValueService _attributeValueService;
        private readonly IGeneralAssembler _generalAssembler;
        private readonly IAttributeRepository _attributeRepository;
        private readonly IMerhantCommunicator _merhantCommunicator;
        private readonly IBrandDomainService _brandDomainService;
        public GetSeoSearchValueQueryHandler(Catalog.ApplicationService.Handler.Services.v2.IProductService productServiceV2,
            ICategoryService categoryService, IBrandDomainService brandDomainService,
            IGeneralAssembler generalAssembler, IAttributeRepository attributeRepository, IMerhantCommunicator merhantCommunicator,
            IAttributeValueService attributeValueService)
        {
            _categoryService = categoryService;
            _productServiceV2 = productServiceV2;
            _attributeValueService = attributeValueService;
            _generalAssembler = generalAssembler;
            _attributeRepository = attributeRepository;
            _merhantCommunicator = merhantCommunicator;
            _brandDomainService = brandDomainService;
        }

        public async Task<ResponseBase<GetProductsFilterQueryResult>> Handle(GetSeoSearchValueQuery request, CancellationToken cancellationToken)
        {
            var result = new ResponseBase<GetProductsFilterQueryResult>();
            var response = new GetSeoBrands
            {
                Filter = new GetProductListAndFilterQuery()
            };
            Uri myUri = new Uri(request.Url);
            var brandId = new Guid?();
            response.Filter = new GetProductListAndFilterQuery();
            response.Filter.FilterModel = new List<FilterModel>();
            string page = HttpUtility.ParseQueryString(myUri.Query).Get("sayfa");
            string sort = HttpUtility.ParseQueryString(myUri.Query).Get("siralama");
            var isSearch = sort == null ? true : false;
            string pageSize = HttpUtility.ParseQueryString(myUri.Query).Get("size");
            var orderBy = _generalAssembler.GetOrderBy(sort, isSearch);
            var query = "";
            var dic = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(myUri.Query);
            foreach (var item in dic)
            {
                if (item.Key is ("q"))
                {
                    query = item.Value;
                }
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
                if (item.Key is ("marka"))
                {
                    if (item.Value.Where(j => j.Contains(",")).Any())
                    {
                        var values = item.Value.ToString().Split(",");
                        foreach (var i in values)
                        {
                            var brandList = await _brandDomainService.GetBrandName(new System.Collections.Generic.List<string> { i.ToString().Replace("/", "") }, true);
                            brandId = brandList?.Values.First();
                            if (brandId != null && brandId != Guid.Empty)
                            {
                                response.Filter.FilterModel.Add(new FilterModel
                                {
                                    Id = brandId.ToString(),
                                    FilterField = ProductFilterEnum.BrandId.ToString(),
                                    Type = ProductFilterEnum.Brand.ToString()
                                });
                            }
                        }
                    }
                    else
                    {
                        var brandList = await _brandDomainService.GetBrandName(new System.Collections.Generic.List<string> { item.Value.ToString().Replace("/", "") }, true);
                        brandId = brandList?.Values.First();
                        if (brandId != null && brandId != Guid.Empty)
                        {
                            response.Filter.FilterModel.Add(new FilterModel
                            {
                                Id = brandId.ToString(),
                                FilterField = ProductFilterEnum.BrandId.ToString(),
                                Type = ProductFilterEnum.Brand.ToString()
                            });
                        }
                    }
                }
                if (item.Key is ("kategori"))
                {
                    var cat = item.Value.ToString().Split("-k-");
                    var category = await _categoryService.GetCategetoryByName(cat[0].Contains("/") ? cat[0].Replace("/", "") : cat[0], cat[1]);
                    var code = category.Code;
                    response.Filter.FilterModel.Add(new FilterModel
                    {
                        SeoUrl = string.Join("", cat[0] + "-k-" + code),
                        Id = category.Id.ToString(),
                        FilterField = ProductFilterEnum.CategoryId.ToString(),
                        Type = ProductFilterEnum.ProductCategory.ToString()
                    });
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

                if (item.Key is not ("siralama" or "sayfa" or "marka" or "fiyat" or "q" or "kategori"))
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
                else
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
            var res = new GetProductsFilterQuery
            {
                FilterModel = response.Filter.FilterModel,
                IsSellerSearch = true,
                IsSellerVisible = false,
                IsVisibleAllFilters = true,
                OrderBy = orderBy,
                PagerInput = new PagerInput(page == null ? 0 : Convert.ToInt32(page), pageSize == null ? 20 : Convert.ToInt32(pageSize)),
                Query = query
            };
            result = await _productServiceV2.GetProductListAndFilterV2(res);
            return result;
        }
    }
}
