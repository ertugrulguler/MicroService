using Catalog.Domain.Enums;
using Catalog.Domain.ProductAggregate.ServiceModels;
using Catalog.Domain.ValueObject.StoreProcedure;
using Framework.Core.Model;
using Framework.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Catalog.Domain.ProductAggregate
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<Product>> GetProductsByChannel(ChannelCode channel, Guid? categoryId,Guid? brandId);
        Task<Product> GetProductWithProductSellerId(Guid productSellerId);
        Task<Product> GetProductWithAllRelations(Guid Id);
        Task<Product> GetProductWithAllRelations(string code);
        Task<Product> GetProductByCategoryIdWithAllRelations(Guid CategoryId);
        Task<Product> GetProductByProductSellerId(Guid productId, Guid sellerId);
        Task<Product> GetProductByProductCodeSellerId(string code, Guid sellerId);
        Task<List<Product>> GetProductBySellerId(Guid sellerId);
        Task<ProductListWithCount> GetProductAllRelations(PagerInput pagerInput, List<Guid> categoryId, List<List<Guid>> attributeIds, Expression<Func<Product, bool>> expressionProduct,
            Expression<Func<ProductSeller, bool>> expressionProductSeller, OrderBy orderby, List<Guid> bannedSellers, int productChannel, List<Guid> sellerList);

        Task<List<ProductFilter>> GetProductListGroupByBrand(List<Guid> categoryId, List<List<Guid>> attritubeIds, Expression<Func<Product,
                                                                   bool>> expressionProduct, Expression<Func<ProductSeller, bool>> expressionProductSeller, List<Guid> bannedSellers, int productChannel);
        Task<List<Guid>> GetProductListGroupBySeller(List<Guid> categoryId, List<List<Guid>> attritubeId, Expression<Func<Product,
                                                                   bool>> expressionProduct, Expression<Func<ProductSeller, bool>> expressionProductSeller, List<Guid> bannedSellers, int productChannel);
        Task<List<decimal>> GetProductListMaxPrice(List<Guid> categoryId, List<List<Guid>> attributeIds, Expression<Func<Product,
                                                                   bool>> expressionProduct, Expression<Func<ProductSeller, bool>> expressionProductSeller, List<Guid> bannedSellers, int productChannel, List<Guid> sellerList);
        Task<List<Product>> GetProductsDetailBySellerId(Guid sellerId, PagerInput pagerInput);
        Task<List<Product>> GetProductListForSeller(List<Guid> productList, Guid sellerId, string barcode, string productName, string brandName, OrderByDate orderBy, string groupCode);
        Task<List<Product>> GetProductListForSellerWithPaging(List<Guid> productList, Guid sellerId, string barcode, string productName, string brandName, OrderByDate orderBy, string groupCode, PagerInput pagerInput);
        Task<List<Product>> GetProductSearchNameOrCode(string code, string productName, PagerInput pagerInput);
        Task<List<Product>> GetProductsSearchOptimization(DateTime createdDate);
        Task<ProductFavorite> GetFavoriteProductsByProductId(Guid ProductId, Guid Id);
        Task<Product> GetProductDetailToCreate(Guid productId);
        Task<List<Product>> GetProductIsBanners(List<Guid?> productSellerId);
        Task<PagedList<Product>> GetProductsSearchOptimizationByPagingAsync(PagerInput pagerInput, DateTime createdDate);

        Task<ProductListWithCountV2> GetProductAllRelationsSP(PagerInput pagerInput, List<Guid> categoryId, List<List<Guid>> attributeIds, List<string> salePriceList, List<Guid> brandIdList,
            List<string> codeList, List<Guid> searchList, OrderBy orderBy, List<Guid> bannedSellers, int productChannel, List<Guid> sellerList);
        Task<ProductListWithCountV2> GetProductAllRelationsSP(string productFilterQuery, string productFilterCountQuery);

        Task<ProductBrandList> GetProductsBrandSearchOptimization(DateTime createdDate);
        Task<List<Product>> GetProductListWithCodes(List<string> codeList, List<Guid> sellerIdList);
        Task<List<BrandFilter>> GetProductBrandFilter(string brandFilterQuery);
        Task<List<RelatedCategories>> GetProductCategoryFilter(string categoryFilterQuery);
        Task<List<PriceFilter>> GetProductPriceFilter(string priceFilterQuery);
        Task<List<SellerFilter>> GetProductSellerFilter(string sellerFilterQuery);
        Task<List<XmlProduct>> GetXmlProducts(string productSellerIds, string sellerDeliveryIds);
        Task<List<XmlAttribute>> GetXmlAttributes(string productIds);

        Task<List<BrandFilter>> GetProductListGroupByBrandSP(List<Guid> categoryId, List<List<Guid>> attributeIds, List<string> salePriceList, List<Guid> brandIdList,
        List<string> codeList, List<Guid> searchList, List<Guid> bannedSellers, int productChannel, List<Guid> sellerIdList);

        Task<List<SellerFilter>> GetProductListGroupBySellerSP(List<Guid> categoryId, List<List<Guid>> attributeIds, List<string> salePriceList, List<Guid> brandIdList,
            List<string> codeList, List<Guid> searchList, List<Guid> bannedSellers, int productChannel, List<Guid> sellerIdList);

        Task<List<PriceFilter>> GetProductListMaxPriceSP(List<Guid> categoryId, List<List<Guid>> attributeIds,
            List<string> salePriceList, List<Guid> brandIdList, List<string> codeList, List<Guid> searchList, List<Guid> bannedSellers, int productChannel, List<Guid> sellerIdList);
        Task<int> GetProductListForSellerAndFilterByCodeTotalCount(List<Guid> productList, Guid sellerId, string code);
        Task<List<Product>> GetProductListForSellerForCount(List<Guid> productList, Guid sellerId);
        Task<Product> GetProductSellerInfo(Guid sellerId, string code);
        Task<List<ProductCountForBackoffice>> GetProductListForSellerForCountSP(Guid sellerId);
        Task<Product> GetProductWithCategory(Guid productId);
        Task<Product> GetProductWithCategoryByCode(string code);

    }
}