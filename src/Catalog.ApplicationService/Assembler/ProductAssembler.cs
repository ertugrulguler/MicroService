using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.ApplicationService.Communicator.Parameter.Model;
using Catalog.Domain.AttributeAggregate;
using Catalog.Domain.BrandAggregate;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.CategoryAggregate.ServiceModel;
using Catalog.Domain.Enums;
using Catalog.Domain.Helpers;
using Catalog.Domain.ProductAggregate;
using Catalog.Domain.ProductAggregate.ServiceModels;
using Framework.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Attribute = Catalog.Domain.AttributeAggregate.Attribute;

namespace Catalog.ApplicationService.Assembler
{
    public class ProductAssembler : IProductAssembler
    {
        public ResponseBase<ProductDto> MapToUpdateProductCommandResult(Product product)
        {
            return new()
            {
                Data = new ProductDto
                {
                    Name = product.Name,
                    DisplayName = product.DisplayName,
                    Description = product.Description,
                }
            };
        }

        public ResponseBase<ProductDetail> MapToGetProductQueryResult(Product product)
        {
            return new()
            {
                Data = new ProductDetail
                {
                    Id = product.Id,
                    IsActive = product.IsActive,
                    Name = product.Name,
                    DisplayName = product.DisplayName,
                    Description = product.Description,
                    Code = product.Code,
                    PriorityRank = product.PriorityRank,
                    ProductMainId = product.ProductMainId,
                    BrandId = product.BrandId,
                    Brand = new BrandDto()
                    {
                        LogoUrl = product.Brand?.LogoUrl,
                        Name = product.Brand?.Name,
                        Website = product.Brand?.WebSite
                    },
                    SimilarProducts = product.SimilarProducts.Select(x => new SimilarProductDto()
                    {
                        FirstProductId = x.ProductId,
                        SecondProductId = x.SecondProductId
                    }).ToList(),
                    ProductSellers = product.ProductSellers.Select(c => new ProductSellerDto()
                    {
                        SellerId = c.SellerId,
                        ListPrice = c.ListPrice,
                        SalePrice = c.SalePrice,
                        DiscountId = c.DiscountId
                    }).ToList()
                }
            };
        }

        public ResponseBase<ProductDto> MapToDeleteProductCommandResult(Product product)

        {
            return new()
            {
                Data = new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    DisplayName = product.DisplayName,
                    Description = product.Description,
                    Code = product.Code,
                    PriorityRank = product.PriorityRank,
                    ProductMainId = product.ProductMainId,
                }
            };
        }

        public ResponseBase<ProductDto> MapToCreateProductCommandResult(Product product)
        {
            throw new System.NotImplementedException();
        }

        public ResponseBase<List<ProductList>> MapToGetProductsByCategoryIdQueryResult(List<Product> productList,
            OrderBy orderBy, List<FavoriteProductsList> favoriteProductLists, List<Guid> variantedProducts, List<CategoryIdAndNameforProducts> categoryList)
        {
            var productDtoList = new List<ProductList>();
            var favoriteProductsCount = favoriteProductLists.Count() > 0 ? true : false;

            foreach (var product in productList)
            {
                var productPrice = product.ProductSellers.OrderBy(u => u.SalePrice).FirstOrDefault();
                if (productPrice == null)
                    continue;
                var info = categoryList.Where(p => product.ProductCategories.Select(o => o.CategoryId).Contains(p.Id)).FirstOrDefault();
                if (info != null)
                    productDtoList.Add(new ProductList
                    {
                        Name = product.Name,
                        ProductSellerId = productPrice.Id,
                        SellerId = productPrice.SellerId,
                        DisplayName = product.DisplayName,
                        SeoUrl = product.SeoName + "-p-" + product.Code,
                        ProductCode = product.Code,
                        ImageUrl = product.ProductImages.Count > 0
                        ? product.ProductImages?.Where(j => j.SellerId == productPrice.SellerId)?.OrderByDescending(u => u.IsDefault)?.Select(u => u.Url)?.Take(5)?.ToList()
                        : null,
                        Id = product.Id,
                        BrandDto = new BrandDto
                        {
                            Id = product.Brand.Id,
                            Name = product.Brand.Name
                        },
                        ListPrice = new Price(productPrice.ListPrice),
                        SalePrice = new Price(productPrice.SalePrice),
                        DiscountRate = new DiscountRateInfo
                        {
                            DiscountRate = productPrice.ListPrice != 0
                            ? Decimal.Round(
                                    ((productPrice.ListPrice - productPrice.SalePrice) / productPrice.ListPrice) * 100)
                                .ToString()
                            : "0",
                        },
                        IsFavorite = favoriteProductsCount
                        ? favoriteProductLists.Where(y => y.ProductId == product.Id).Count() > 0 ? true : false
                        : false,
                        IsVariantable = variantedProducts.Contains(product.Id),
                        IsVariantableStr = variantedProducts.Contains(product.Id) == true ? "Ürünün farklı seçenekleri vardır" : string.Empty,
                        CategoryInfo = info
                    });
            }

            return new()
            {
                Data = productDtoList
            };
        }

        #region Product Detail

        public ResponseBase<GetProductDetailResponse> MapToGetProductDetailBySellerIdQueryResult(Product product,
            ProductSeller productSeller, string sellerName, Brand brand,
            List<Category> categories, List<CategoryAttribute> categoryAttributes, List<Attribute> attributes,
            List<AttributeValue> attributeValues, List<List<ProductVariantGroup>> productGroups,
            List<OtherSellers> otherSellers, DeliveryOptions deliveryOptions, string installmentTable,
            bool isFavorite, AllCategoryUrl allCategoryUrl, AllBrandsUrl allBrandsUrl, Category mainCategory, List<Breadcrumb> breadcrumbs, Guid sellerId, Guid productId, string sellerSeoName)
        {
            var discountRate = ArrangeDiscountRate(productSeller.SalePrice, productSeller.ListPrice);

            var getProductDetailBySellerId = new GetProductDetailResponse()
            {
                Name = product.Name,
                DisplayName = product.DisplayName,
                SellerName = sellerName,
                SellerSeoName = sellerSeoName,
                Description = product.Description,
                IsDescriptionHtml = StringExtensions.IsHtml(product.Description),
                Code = product.Code,
                BrandId = product.BrandId,
                BrandName = brand.Name,
                BrandSeoName = brand.SeoName,
                CategoryName = mainCategory.DisplayName,
                IsFavorite = isFavorite,
                Attributes = product.ProductAttributes.Select(x => new SellerProductAttribute
                {
                    AttributeName = attributes.FirstOrDefault(a => a.Id == x.AttributeId) != null
                        ? attributes.FirstOrDefault(a => a.Id == x.AttributeId).DisplayName
                        : "not found!",
                    AttributeValue = attributeValues.FirstOrDefault(a => a.Id == x.AttributeValueId) != null
                        ? attributeValues.FirstOrDefault(a => a.Id == x.AttributeValueId).Value
                        : "not found!",
                    IsVariantable = categoryAttributes.Select(ca => ca.AttributeId)
                        .Contains(attributes.FirstOrDefault(a => a.Id == x.AttributeId).Id),
                    IsRequired = categoryAttributes.Where(c => c.IsVariantable).Select(ca => ca.AttributeId)
                        .Contains(attributes.FirstOrDefault(a => a.Id == x.AttributeId).Id)
                }).ToList(),
                Categories = product.ProductCategories.Select(x => new SellerProductCategory
                {
                    CategoryName = categories.FirstOrDefault(c => c.Id == x.CategoryId) != null
                        ? categories.FirstOrDefault(c => c.Id == x.CategoryId).Name
                        : "not found!"
                }).ToList(),
                Images = product.ProductImages.Select(x => new SellerProductImage
                {
                    ImageUrl = x.Url
                }).ToList(),
                FirstVariantGroup = productGroups.Count > 0
                    ? productGroups.ElementAt(0)
                    : new List<ProductVariantGroup>(),
                SecondVariantGroup = productGroups.Count > 1
                    ? productGroups.ElementAt(1)
                    : new List<ProductVariantGroup>(),
                Prices = new SellerProductPrice
                {
                    VatRate = product.VatRate,
                    ListPrice = productSeller.ListPrice > productSeller.SalePrice
                        ? new Price(productSeller.ListPrice)
                        : null,
                    SalePrice = new Price(productSeller.SalePrice),
                    DiscountRate = new DiscountRateInfo
                    {
                        DiscountRate = discountRate.ToString(),
                        DiscountRateText = discountRate == 0
                            ? null
                            : new StyledText
                            {
                                Text = $"Sepette %{discountRate} indirim fiyatı",
                                TextStyleInfo = new StyleInfo { TextColor = StyleConstants.Blue, FontSize = StyleConstants.Font12 }
                            }
                    }
                },
                Installments = installmentTable,
                StockCode = product.ProductSellers.FirstOrDefault() != null
                    ? product.ProductSellers.FirstOrDefault().StockCode
                    : string.Empty,
                StockCount = product.ProductSellers.FirstOrDefault() != null
                    ? product.ProductSellers.FirstOrDefault().StockCount
                    : 0,
                OtherSellers = otherSellers,
                DeliveryOptions = deliveryOptions,
                AllCategoryText = new StyledText
                {
                    Text =
                        $"Tüm {categories.FirstOrDefault(x => x.Type == CategoryTypeEnum.MainCategory).DisplayName} Kategorisi",
                    TextStyleInfo = new StyleInfo { TextColor = StyleConstants.Black, FontSize = 14 },
                    Styles = new List<SubStyleInfo>
                    {
                        new SubStyleInfo
                        {
                            SubText = $"{categories.FirstOrDefault(x => x.Type == CategoryTypeEnum.MainCategory).DisplayName}",
                            FontSize = 16, TextColor = StyleConstants.Blue
                        }
                    }
                },
                AllBrandText = new StyledText
                {
                    Text = $"Tüm {brand.Name} Ürünleri",
                    TextStyleInfo = new StyleInfo { TextColor = StyleConstants.Black, FontSize = 14 },
                    Styles = new List<SubStyleInfo>
                    { new SubStyleInfo { SubText = $"{brand.Name}", FontSize = 16, TextColor = StyleConstants.Blue } }
                },
                AllCategoryUrl = allCategoryUrl,
                AllBrandsUrl = allBrandsUrl,
                ShareUrl = $"https://www.pazarama.com/" + product.SeoName + "-p-" + product.Code,
                Breadcrumb = breadcrumbs,
                SellerId = sellerId,
                ProductId = productId
            };

            return new ResponseBase<GetProductDetailResponse>() { Data = getProductDetailBySellerId, Success = true };
        }

        private static decimal ArrangeDiscountRate(decimal salePrice, decimal listPrice)
        {
            if (listPrice == 0)
                return 0;
            return Decimal.Round((listPrice - salePrice) * 100 / listPrice);
        }

        #endregion

        public ResponseBase<GetProductVariantsResponse> MapToGetProductVariantsQueryResult(Product product,
            ProductSeller productSeller, List<List<ProductVariantGroup>> productGroups)
        {
            var productImage = "no image found";
            var productDefaultImage = product.ProductImages.FirstOrDefault(x => x.IsDefault);
            var productFirstImage = product.ProductImages.FirstOrDefault();

            if (productDefaultImage != null)
                productImage = productDefaultImage.Url;
            else if (productFirstImage != null)
                productImage = productFirstImage.Url;

            var discountRate = ArrangeDiscountRate(productSeller.SalePrice, productSeller.ListPrice);

            var getProductVariants = new GetProductVariantsResponse
            {
                DisplayName = product.DisplayName,
                Image = new SellerProductImage { ImageUrl = productImage },
                FirstVariantGroup = productGroups.Count > 0
                    ? productGroups.ElementAt(0)
                    : new List<ProductVariantGroup>(),
                SecondVariantGroup = productGroups.Count > 1
                    ? productGroups.ElementAt(1)
                    : new List<ProductVariantGroup>(),
                ListPrice = productSeller.ListPrice > productSeller.SalePrice
                        ? new Price(productSeller.ListPrice)
                        : null,
                SalePrice = new Price(productSeller.SalePrice),
                DiscountRate = new DiscountRateInfo
                {
                    DiscountRate = discountRate.ToString(),
                    DiscountRateText = discountRate == 0
                            ? null
                            : new StyledText
                            {
                                Text = $"Sepette %{discountRate} indirim fiyatı",
                                TextStyleInfo = new StyleInfo { TextColor = StyleConstants.Blue, FontSize = StyleConstants.Font12 }
                            }
                }
            };

            return new ResponseBase<GetProductVariantsResponse>() { Data = getProductVariants, Success = true };
        }

        public ResponseBase<ProductSellerDto> MapToUpdatePriceControlCommandResult(ProductSeller productSeller)

        {
            return new()
            {
                Data = new ProductSellerDto
                {
                    ProductId = productSeller.ProductId,
                    StockCode = productSeller.StockCode,
                    StockCount = productSeller.StockCount,
                    SellerId = productSeller.SellerId,
                    ListPrice = productSeller.ListPrice,
                    SalePrice = productSeller.SalePrice,
                    CurrencyId = productSeller.CurrencyId,
                    DiscountId = productSeller.DiscountId,
                    InstallmentCount = productSeller.InstallmentCount
                }
            };
        }

        public ResponseBase<List<SellerProducts>> MapToGetProductsDetailBySellerIdQueryResult(List<Product> products,
            List<Attribute> attributes,
            Brand brand, List<Category> categories, List<AttributeValue> attributeValues,
            List<ProductGroups> productGroups, List<CityListResponse> cityList)

        {
            var productsDetailBySellerId = new List<SellerProducts>();
            foreach (var product in products)
            {
                productsDetailBySellerId.Add(new SellerProducts
                {
                    Name = product.Name,
                    DisplayName = product.DisplayName,
                    Description = product.Description,
                    Code = product.Code,
                    BrandName = brand.Name,
                    Attributes = product.ProductAttributes.Select(x => new ProductAttributes
                    {
                        AttributeName = attributes.FirstOrDefault(a => a.Id == x.AttributeId) != null
                            ? attributes.FirstOrDefault(a => a.Id == x.AttributeId).Name
                            : "not found!",
                        AttributeValue = attributeValues.FirstOrDefault(a => a.Id == x.AttributeValueId) != null
                            ? attributeValues.FirstOrDefault(a => a.Id == x.AttributeValueId).Value
                            : "not found!"
                    }).ToList(),
                    DeliveryTypes = product.ProductDeliveries.Select(x => new ProductDeliveryTypes
                    {
                        DeliveryTypeName = x.DeliveryType.ToString(),
                        CityName = product.ProductDeliveries.Where(y => y.DeliveryType == x.DeliveryType).Select(
                                h =>
                                    cityList.Where(u => u.Id == (h.CityId.HasValue ? h.CityId.Value : Guid.Empty)).FirstOrDefault() != null
                                        ? cityList.Where(u => u.Id == (h.CityId.HasValue ? h.CityId.Value : Guid.Empty)).FirstOrDefault().Name
                                        : "not found!").Distinct().ToList()
                    }).GroupBy(y => y.DeliveryTypeName, (k, g) => new { Key = k, Value = g.FirstOrDefault() })
                        .Select(y => y.Value).ToList(),
                    Categories = product.ProductCategories.Select(x => new ProductCategorys
                    {
                        CategoryName = categories.FirstOrDefault(c => c.Id == x.CategoryId) != null
                            ? categories.FirstOrDefault(c => c.Id == x.CategoryId).Name
                            : "not found!"
                    }).ToList(),
                    Images = product.ProductImages.Select(x => new ProductImages
                    {
                        ImageUrl = x.Url
                    }).ToList(),
                    ProductGroups = productGroups,
                    SalePrice = product.ProductSellers.FirstOrDefault() != null
                        ? product.ProductSellers.FirstOrDefault().SalePrice
                        : 0,
                    ListPrice = product.ProductSellers.FirstOrDefault() != null
                        ? product.ProductSellers.FirstOrDefault().ListPrice
                        : 0,
                    VatRate = product.VatRate,
                    StockCode = product.ProductSellers.FirstOrDefault() != null
                        ? product.ProductSellers.FirstOrDefault().StockCode
                        : string.Empty,
                    StockCount = product.ProductSellers.FirstOrDefault() != null
                        ? product.ProductSellers.FirstOrDefault().StockCount
                        : 0
                });
            }

            return new ResponseBase<List<SellerProducts>>() { Data = productsDetailBySellerId, Success = true };
        }

        public ResponseBase<GetProductDelivery> MapToGetProductDeliveryListQueryResult(List<Product> products,
            List<Communicator.BackOffice.Model.CategoryCompanyInstallmentResponse> categoryCompanyInstallments)
        {
            var productsDeliveries = new List<ProductDeliveries>();
            foreach (var product in products)
            {
                productsDeliveries.Add(new ProductDeliveries
                {
                    ProductId = product.Id,
                    SellerId = product.ProductDeliveries.FirstOrDefault().SellerId,
                    DeliveryOptions = product.ProductDeliveries.GroupBy(y => y.DeliveryId).Select(x =>
                        new DeliveryOption
                        {
                            DeliveryType = x.FirstOrDefault().DeliveryType,
                            CityId = product.ProductDeliveries
                                .Where(z => z.DeliveryId == x.FirstOrDefault().DeliveryId &&
                                            z.DeliveryType == x.FirstOrDefault().DeliveryType).Select(a =>
                                    new DeliveryCity
                                    {
                                        CityId = a.CityId
                                    }).ToList(),
                            DeliveryId = x.FirstOrDefault().DeliveryId
                        }).ToList(),
                    UseOverdraftInstallment =
                        categoryCompanyInstallments.Where(x =>
                            (x.CategoryId == product.ProductCategories.FirstOrDefault().CategoryId || x.CategoryId == null) &&
                            x.SellerId == product.ProductDeliveries.FirstOrDefault().SellerId).FirstOrDefault() != null
                            ? categoryCompanyInstallments
                                .Where(x => (x.CategoryId == product.ProductCategories.FirstOrDefault().CategoryId || x.CategoryId == null) &&
                                            x.SellerId == product.ProductDeliveries.FirstOrDefault().SellerId)
                                .FirstOrDefault().UseOverdraftInstallment
                            : false,
                    OverdraftInstallmentCount =
                        categoryCompanyInstallments.Where(x =>
                            (x.CategoryId == product.ProductCategories.FirstOrDefault().CategoryId || x.CategoryId == null) &&
                            x.SellerId == product.ProductDeliveries.FirstOrDefault().SellerId).FirstOrDefault() != null
                            ? categoryCompanyInstallments
                                .Where(x => (x.CategoryId == product.ProductCategories.FirstOrDefault().CategoryId || x.CategoryId == null) &&
                                            x.SellerId == product.ProductDeliveries.FirstOrDefault().SellerId)
                                .FirstOrDefault().OverdraftInstallmentCount
                            : 0
                });
            }

            return new ResponseBase<GetProductDelivery>()
            {
                Data = new GetProductDelivery
                {
                    ProductDeliveries = productsDeliveries
                },

                Success = true
            };
        }

        public ResponseBase<GetProductListForSellerQueryResult> MapToGetProductsForSellerQueryResult(
            List<Product> products, List<Category> categories, List<ProductVariantGroup> productVariants)

        {
            var productsDetailBySellerId = new List<SellerProductInfo>();
            foreach (var product in products)
            {
                productsDetailBySellerId.Add(new SellerProductInfo
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    DisplayName = product.DisplayName,
                    Code = product.Code,
                    BrandName = product.Brand.Name,
                    PriorityRank = product.PriorityRank,
                    State = (product.ProductSellers.FirstOrDefault() != null
                        ? product.ProductSellers.FirstOrDefault().StockCount
                        : 0) > 0
                        ? "Satışta Olan"
                        : "Stoğu olmayan",
                    CategoryName = product.ProductCategories.Count > 0
                        ? product.ProductCategories.Select(x => new ProductCategorys
                        {
                            CategoryName = categories.FirstOrDefault(c =>
                                c.Id == x.CategoryId && c.Type == CategoryTypeEnum.MainCategory) != null
                                ? categories.FirstOrDefault(c =>
                                    c.Id == x.CategoryId && c.Type == CategoryTypeEnum.MainCategory).Name
                                : "not found!"
                        }).FirstOrDefault().CategoryName
                        : "not found!",
                    //IsVariantable = product.ProductAttributes.FirstOrDefault() != null
                    //    ? product.ProductAttributes.FirstOrDefault().IsVariantable ? "Varyantlı" : "Varyantsiz"
                    //    : "Varyantsiz",
                    IsVariantable = productVariants.FirstOrDefault(x => x.ProductId == product.Id) != null
                    ? productVariants.FirstOrDefault(x => x.ProductId == product.Id).AttributeName : "Varyantsiz",
                    StockCode = product.ProductSellers.FirstOrDefault() != null
                        ? product.ProductSellers.FirstOrDefault().StockCode
                        : string.Empty,
                    StockCount = product.ProductSellers.FirstOrDefault() != null
                        ? product.ProductSellers.FirstOrDefault().StockCount
                        : 0,
                    ListPrice = new Price(product.ProductSellers.FirstOrDefault().ListPrice),
                    SalePrice = new Price(product.ProductSellers.FirstOrDefault().SalePrice),
                    CreatedDate = product.CreatedDate,
                    ModifiedDate = product.ModifiedDate.GetValueOrDefault(),
                    GroupCode = product.ProductGroups.FirstOrDefault() != null
                    ? product.ProductGroups.FirstOrDefault().GroupCode
                    : string.Empty
                });
            }

            var sellerProductQueryResult = new GetProductListForSellerQueryResult
            {
                Products = productsDetailBySellerId
            };
            return new ResponseBase<GetProductListForSellerQueryResult>
            { Data = sellerProductQueryResult, Success = true };
        }

        public ResponseBase<GetProductSearchNameOrCodeQueryResult> MapToGetSearchProductQueryResult(
            List<Product> products,
            List<Category> categories, List<Attribute> attributes, List<AttributeValue> attributeValues)

        {
            var productSearchList = new List<ProductSearchList>();
            foreach (var product in products)
            {
                productSearchList.Add(new ProductSearchList
                {
                    ProductId = product.Id,
                    SellerId = product.ProductSellers.FirstOrDefault().SellerId,
                    Name = product.Name,
                    DisplayName = product.DisplayName,
                    Code = product.Code,
                    BrandName = product.Brand.Name,
                    VatRate = product.VatRate,
                    CategoryName = product.ProductCategories.Count > 0
                        ? product.ProductCategories.Select(x => new ProductCategorys
                        {
                            CategoryName = categories.FirstOrDefault(c =>
                                c.Id == x.CategoryId && c.Type == CategoryTypeEnum.MainCategory) != null
                                ? categories.FirstOrDefault(c =>
                                    c.Id == x.CategoryId && c.Type == CategoryTypeEnum.MainCategory).Name
                                : "not found!"
                        }).FirstOrDefault().CategoryName
                        : "not found!",
                    Attributes = product.ProductAttributes.Select(x => new ProductSearchAttributes
                    {
                        AttributeName = attributes.FirstOrDefault(a => a.Id == x.AttributeId) != null
                            ? attributes.FirstOrDefault(a => a.Id == x.AttributeId).Name
                            : "not found!",
                        AttributeValue = attributeValues.FirstOrDefault(a => a.Id == x.AttributeValueId) != null
                            ? attributeValues.FirstOrDefault(a => a.Id == x.AttributeValueId).Value
                            : "not found!"
                    }).ToList(),
                    ImageUrl = product.ProductImages.FirstOrDefault().Url != null
                        ? product.ProductImages.FirstOrDefault().Url
                        : "not found"
                });
            }

            var productSearchQueryResult = new GetProductSearchNameOrCodeQueryResult
            {
                Products = productSearchList,
            };

            return new ResponseBase<GetProductSearchNameOrCodeQueryResult>
            { Data = productSearchQueryResult, Success = true };
        }

        public ResponseBase<GetProductDetailToCreate> MapToGetProductDetailToCreateQueryResult(Product product,
            List<Category> categories, List<Attribute> attributes, List<AttributeValue> attributeValues)
        {
            decimal desi;
            var getProductDetailToCreate = new GetProductDetailToCreate()
            {
                Name = product.Name,
                DisplayName = product.DisplayName,
                Description = product.Description,
                Code = product.Code,
                BrandId = product.BrandId,
                Desi = decimal.TryParse(product.Desi.ToString().Replace(".", ","), out desi) ? desi : 0,
                GroupCode = product.ProductGroups.FirstOrDefault().GroupCode,
                StockCode = "",
                VatRate = product.VatRate,
                ListPrice = 0,
                SalePrice = 0,
                InstallmentCount = 0,
                CategoryId = categories.FirstOrDefault().Id,
                Attributes = product.ProductAttributes.Select(x => new CreateProductAttribute
                {
                    AttributeId = attributes.FirstOrDefault().Id,
                    AttributeValueId = attributeValues.FirstOrDefault().Id
                }).ToList(),
                Images = product.ProductImages.Select(x => new CreateProductImage
                {
                    ImageUrl = x.Url
                }).ToList(),
                //Deliveries = product.ProductDeliveries.GroupBy(y => y.DeliveryId).Select(x => new CreateProductDelivery()
                //{
                //    DeliveryType = x.FirstOrDefault().DeliveryType != null ? x.FirstOrDefault().DeliveryType : default,
                //    DeliveryId = x.FirstOrDefault().DeliveryId != null ? x.FirstOrDefault().DeliveryId : default,
                //    CityList = product.ProductDeliveries.Where(z => z.DeliveryId == x.FirstOrDefault().DeliveryId && z.DeliveryType == x.FirstOrDefault().DeliveryType)
                //        .Select(a => new CreateDeliveryCity
                //        {
                //            CityId = a.CityId.Value
                //        }).ToList(),
                //}).ToList()
            };

            return new ResponseBase<GetProductDetailToCreate>() { Data = getProductDetailToCreate, Success = true };
        }

        public ResponseBase<FavoriteProductDto> MapToCreateAndUpdateFavoriteProductCommandResult(
            FavoriteProduct product)
        {
            return new()
            {
                Data = new FavoriteProductDto
                {
                    Id = product.Id,
                    CustomerId = product.CustomerId,
                    ProductId = product.ProductId,
                    IsActive = product.IsActive
                },
                Success = true
            };
        }

        public ResponseBase<FavoriteProductList> MapToGetFavoriteProductListQueryResult(
            List<ProductFavorite> productList, int count, List<Guid> variantedProducts, List<CategoryIdAndNameforProducts> categoryList)
        {
            var favoriteProductList = new List<FavoriteProducts>();

            foreach (var product in productList)
            {
                var productPrice = product.Product.ProductSellers.OrderBy(u => u.SalePrice).FirstOrDefault();
                if (productPrice == null)
                    continue;
                var info = categoryList.Where(p => product.Product.ProductCategories.Select(o => o.CategoryId).Contains(p.Id)).FirstOrDefault();
                if (info == null)
                    count--;
                else
                {
                    favoriteProductList.Add(new FavoriteProducts
                    {
                        ProductId = product.Product.Id,
                        Name = product.Product.Name,
                        SellerId = productPrice.SellerId,
                        SeoUrl = product.Product.SeoName + "-p-" + product.Product.Code,
                        DisplayName = product.Product.DisplayName,
                        ImageUrl = product.Product.ProductImages.Count > 0
                        ? product.Product.ProductImages.OrderByDescending(u => u.IsDefault).FirstOrDefault().Url
                        : null,
                        Id = product.FavoriteProductId,
                        BrandDto = new BrandDto
                        {
                            Id = product.Product.Brand.Id,
                            Name = product.Product.Brand.Name
                        },
                        ListPrice = new Price(productPrice.ListPrice),
                        SalePrice = new Price(productPrice.SalePrice),
                        IsVariantable = variantedProducts.Contains(product.Product.Id),
                        CategoryInfo = info
                    });
                }
            }

            return new()
            {
                Data = new FavoriteProductList { List = favoriteProductList, TotalCount = count },
                Success = true
            };
        }

        public ResponseBase<GetProductDetailResponse> MapToGetProductDetailBySellerIdQueryResult(Product product,
            string sellerName, Brand brand, List<Category> categories, List<CategoryAttribute> categoryAttributes,
            List<System.Attribute> attributes, List<AttributeValue> attributeValues,
            List<List<ProductVariantGroup>> productGroups, List<OtherSellers> otherSellers,
            DeliveryOptions deliveryOptions, List<InstallmentTable> installmentTable)
        {
            throw new NotImplementedException();
        }

        public ResponseBase<List<SellerProducts>> MapToGetProductsDetailBySellerIdQueryResult(List<Product> product,
            List<System.Attribute> attributes, Brand brand, List<Category> categories,
            List<AttributeValue> attributeValues, List<ProductGroups> productGroups)
        {
            throw new NotImplementedException();
        }

        public ResponseBase<GetProductSearchNameOrCodeQueryResult> MapToGetSearchProductQueryResult(
            List<Product> products, List<Category> categories, List<System.Attribute> attributes,
            List<AttributeValue> attributeValues)
        {
            throw new NotImplementedException();
        }

        public ResponseBase<GetProductDetailToCreate> MapToGetProductDetailToCreateQueryResult(Product product,
            List<Category> categories, List<System.Attribute> attributes, List<AttributeValue> attributeValues)
        {
            throw new NotImplementedException();
        }
    }
}