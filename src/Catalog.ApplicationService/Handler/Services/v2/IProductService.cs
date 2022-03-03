using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;
using Catalog.Domain.ProductAggregate;
using Catalog.Domain.ProductAggregate.ServiceModels;
using Catalog.Domain.ValueObject.StoreProcedure;
using Framework.Core.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Services.v2
{
    public interface IProductService
    {
        Task<ResponseBase<GetProductsFilterQueryResult>> GetProductListAndFilterV2(GetProductsFilterQuery request);
        Task<List<ProductFilterList>> GetProductFilter(GetProductList request, List<Product> productList,
            List<Guid> bannedSellers, List<Category> categorySubList, bool quickFilter);
        Task<List<ProductFilterList>> GetProductFilterV2(GetProductList request,
            List<Guid> bannedSellers, List<Category> categorySubList, bool quickFilter, List<AttributeFilter> productList, FilterResponseModel filterResponseModel, int channelCode, GetProductsFilterQuery product);

        ExpressionsModel GetProductListExpressions(GetProductList request, ProductFilterEnum productFilterEnum,
            List<Category> categorySubList);

        Task<List<ProductFilterList>> GetProductFilterBySeller(ExpressionsModel getExpressionModel,
            List<Guid> bannedSellers, ProductChannelCode code);

        List<ProductFilterList> GetProductFilterBrand(ExpressionsModel getExpressionModel,
            List<Guid> bannedSellers, ProductChannelCode code, List<Product> productList);

        Task<List<ProductFilterList>> GetProductFilterForAttribute(List<Product> productList,
              List<Guid> categoryId);

        Task<List<ProductFilterList>> GetProductFilterPrice(GetProductList request, ExpressionsModel getExpressionModel,
            List<Guid> bannedSellers, ProductChannelCode code);

        Task<List<ProductFilterList>> GetProductFilterforCategory(GetProductList request, List<Product> productList);
        FilterResponseModel ArrangeSpParameters(GetProductList request, ProductFilterEnum productFilterEnum, List<Category> categorySubList);
        string CreateProductFilterSpString(PagerInput pagerInput, List<Guid> categoryIdList,
            List<List<Guid>> attributeIdList, List<string> salePriceList, List<Guid> brandIdList,
            List<string> codeList, List<Guid> searchList, OrderBy orderBy, int productChannel, List<Guid> sellerList);

        string CreateProductFilterCountSpString(PagerInput pagerInput, List<Guid> categoryIdList,
            List<List<Guid>> attributeIdList, List<string> salePriceList, List<Guid> brandIdList,
            List<string> codeList, List<Guid> searchList, OrderBy orderBy, int productChannel, List<Guid> sellerList);
        string CreateAttributeFilterSpString(PagerInput pagerInput, List<Guid> categoryIdList,
            List<List<Guid>> attributeIdList, List<string> salePriceList, List<Guid> brandIdList,
            List<string> codeList, List<Guid> searchList, OrderBy orderBy, int productChannel, List<Guid> sellerList);
        string CreateBrandFilterSpString(PagerInput pagerInput, List<Guid> categoryIdList,
            List<List<Guid>> attributeIdList, List<string> salePriceList, List<Guid> brandIdList,
            List<string> codeList, List<Guid> searchList, OrderBy orderBy, int productChannel, List<Guid> sellerList);
        string CreateCategoryFilterSpString(PagerInput pagerInput, List<Guid> categoryIdList,
           List<List<Guid>> attributeIdList, List<string> salePriceList, List<Guid> brandIdList,
           List<string> codeList, List<Guid> searchList, OrderBy orderBy, int productChannel, List<Guid> sellerList);
        Task<List<ProductFilterList>> GetProductFilterforCategoryV3(GetProductList request, string categoryFilterQuery);
        List<ProductFilterList> GetProductFilterForAttributeV2(List<AttributeFilter> attributeFilterList, GetProductList request);
        string CreatePriceFilterSpString(PagerInput pagerInput, List<Guid> categoryIdList,
           List<List<Guid>> attributeIdList, List<string> salePriceList, List<Guid> brandIdList,
           List<string> codeList, List<Guid> searchList, OrderBy orderBy, int productChannel, List<Guid> sellerList);
        string CreateSellerFilterSpString(PagerInput pagerInput, List<Guid> categoryIdList,
           List<List<Guid>> attributeIdList, List<string> salePriceList, List<Guid> brandIdList,
           List<string> codeList, List<Guid> searchList, OrderBy orderBy, int productChannel, List<Guid> sellerList);
    }
}