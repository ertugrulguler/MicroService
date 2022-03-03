using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.ApplicationService.Communicator.BackOffice.Model;
using Catalog.ApplicationService.Communicator.Parameter.Model;
using Catalog.Domain.AttributeAggregate;
using Catalog.Domain.BrandAggregate;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.CategoryAggregate.ServiceModel;
using Catalog.Domain.Enums;
using Catalog.Domain.ProductAggregate;
using Catalog.Domain.ProductAggregate.ServiceModels;
using Framework.Core.Model;
using System;
using System.Collections.Generic;
using Attribute = Catalog.Domain.AttributeAggregate.Attribute;

namespace Catalog.ApplicationService.Assembler
{
    public interface IProductAssembler
    {
        ResponseBase<ProductDto> MapToCreateProductCommandResult(Product product);
        ResponseBase<ProductDto> MapToUpdateProductCommandResult(Product product);
        ResponseBase<ProductDto> MapToDeleteProductCommandResult(Product product);
        ResponseBase<ProductDetail> MapToGetProductQueryResult(Product product);
        ResponseBase<List<ProductList>> MapToGetProductsByCategoryIdQueryResult(List<Product> product, OrderBy orderBy, List<FavoriteProductsList> favoriteProductLists, List<Guid> variantedProducts, List<CategoryIdAndNameforProducts> categoryList);
        ResponseBase<GetProductDetailResponse> MapToGetProductDetailBySellerIdQueryResult(Product product, ProductSeller productSeller, string sellerName, Brand brand,
            List<Category> categories, List<CategoryAttribute> categoryAttributes, List<Attribute> attributes, List<AttributeValue> attributeValues, List<List<ProductVariantGroup>> productGroups,
            List<OtherSellers> otherSellers, DeliveryOptions deliveryOptions, string installmentTable, bool isFavorite, AllCategoryUrl allCategoryUrl, AllBrandsUrl allBrandsUrl, Category mainCategory, List<Breadcrumb> breadcrumbs, Guid sellerId, Guid productId, string sellerSeoName);

        ResponseBase<GetProductVariantsResponse> MapToGetProductVariantsQueryResult(Product product, ProductSeller productSeller, List<List<ProductVariantGroup>> productGroups);

        ResponseBase<ProductSellerDto> MapToUpdatePriceControlCommandResult(ProductSeller productSeller);

        ResponseBase<List<SellerProducts>> MapToGetProductsDetailBySellerIdQueryResult(List<Product> product, List<Attribute> attributes, Brand
            brand, List<Category> categories, List<AttributeValue> attributeValues, List<ProductGroups> productGroups, List<CityListResponse> cityList);

        ResponseBase<GetProductListForSellerQueryResult> MapToGetProductsForSellerQueryResult(List<Product> products, List<Category> categories, List<ProductVariantGroup> productVariants);

        ResponseBase<GetProductDelivery> MapToGetProductDeliveryListQueryResult(List<Product> product, List<CategoryCompanyInstallmentResponse> categoryCompanyInstallments);

        ResponseBase<GetProductSearchNameOrCodeQueryResult> MapToGetSearchProductQueryResult(List<Product> products,
            List<Category> categories, List<Attribute> attributes, List<AttributeValue> attributeValues);

        ResponseBase<GetProductDetailToCreate> MapToGetProductDetailToCreateQueryResult(Product product,
            List<Category> categories, List<Attribute> attributes, List<AttributeValue> attributeValues);

        ResponseBase<FavoriteProductDto> MapToCreateAndUpdateFavoriteProductCommandResult(FavoriteProduct product);

        ResponseBase<FavoriteProductList> MapToGetFavoriteProductListQueryResult(List<ProductFavorite> productList, int count, List<Guid> variantedProducts, List<CategoryIdAndNameforProducts> categoryList);
    }
}



