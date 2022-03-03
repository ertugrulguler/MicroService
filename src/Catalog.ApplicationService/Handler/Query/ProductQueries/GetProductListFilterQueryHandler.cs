using System;
using System.Collections.Generic;
using System.Linq;
using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Framework.Core.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Catalog.ApplicationService.Communicator.Campaign.Model;
using Catalog.ApplicationService.Communicator.Merchant;
using Catalog.ApplicationService.Communicator.Merchant.Model;
using Catalog.ApplicationService.Handler.Services;
using Catalog.Domain.BrandAggregate;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;
using Catalog.Domain.ProductAggregate;
using Framework.Core.Helpers.Abstract;
using Framework.Core.Model.Enums;
using Microsoft.Extensions.Configuration;

namespace Catalog.ApplicationService.Handler.Query.ProductQueries
{
    public class GetProductListFilterQueryHandler : IRequestHandler<GetProductListFilterQueryRequest, ResponseBase<GetProductListFilterQueryResponse>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IProductSellerRepository _productSellerRepository;
        private readonly IDiscountService _discountService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IConfiguration _configuration;
        private readonly IMerhantCommunicator _merchantCommunicator;
        private readonly IProductVariantService _productVariantService;
        private readonly ICustomerHelper _customerHelper;

        public GetProductListFilterQueryHandler(IProductRepository productRepository, IBrandRepository brandRepository, IProductSellerRepository productSellerRepository, IDiscountService discountService, ICategoryRepository categoryRepository, IConfiguration configuration, IMerhantCommunicator merchantCommunicator, IProductVariantService productVariantService, ICustomerHelper customerHelper)
        {
            _productRepository = productRepository;
            _brandRepository = brandRepository;
            _productSellerRepository = productSellerRepository;
            _discountService = discountService;
            _categoryRepository = categoryRepository;
            _configuration = configuration;
            _merchantCommunicator = merchantCommunicator;
            _productVariantService = productVariantService;
            _customerHelper = customerHelper;
        }


        public async Task<ResponseBase<GetProductListFilterQueryResponse>> Handle(GetProductListFilterQueryRequest request, CancellationToken cancellationToken)
        {
            var productChannelCode = _customerHelper.GetChannel();

            var products = await _productRepository.GetProductsByChannel(productChannelCode, request.CategoryId, request.BrandId);
            if (products.Count == 0)
                return new ResponseBase<GetProductListFilterQueryResponse>();

            var response = new ResponseBase<GetProductListFilterQueryResponse>() { Data = new GetProductListFilterQueryResponse() { Products = new List<FilterProduct>() } };
            var prods = new List<FilterProduct>();

            var productIds = products.Select(i => i.Id).ToList();

            var couponWithSellers = await _discountService.GetCouponSellers(productIds);
            if (couponWithSellers.Data.Count == 0)
                return new ResponseBase<GetProductListFilterQueryResponse>();

            var usedSellerId = new List<CouponProductWithSeller>();
            foreach (var coupon in couponWithSellers.Data)
            {

                var product = products.FirstOrDefault(i => i.Id == coupon.ProductId);

                var newProduct = new FilterProduct();
                var brand = await _brandRepository.FindByAsync(i => i.Id == product.BrandId);
                newProduct.BrandId = brand.Id;
                newProduct.BrandName = brand.Name;
                newProduct.Name = product.DisplayName;
                newProduct.Description = product.Description;
                newProduct.Code = product.Code;



                var seller = await _productSellerRepository.FindByAsync(i => i.ProductId == coupon.ProductId && i.SellerId == coupon.SellerId);

                if (seller == null)
                    continue;

                if (usedSellerId.Count > 0 && usedSellerId.Any(i => i.ProductId == coupon.ProductId && i.SellerId == coupon.SellerId))
                    continue;


                var sellerSeoName = await _merchantCommunicator.GetSellerDetailByIds(new GetSellerDetailByIdsRequest() { SellerId = new List<Guid>() { coupon.SellerId } });
                newProduct.Url = $"/iscep/{product.SeoName}-p-{product.Code}?magaza={sellerSeoName.Data[0].SellerSeoName}";

                product.ArrangeProductImagesBySellerId(seller.SellerId);

                newProduct.ListPrice = new Price(seller.ListPrice).ValueString;

                var discount = await _discountService.GetDiscountResult(seller.SellerId, seller.ProductId, seller.SalePrice, seller.ListPrice, productChannelCode);

                if (discount != null && discount.IsDiscounted)
                    newProduct.SalePrice = new Price(discount.DiscountedAmount).ValueString;

                else 
                    continue;

                newProduct.ImageList = product.ProductImages.Select(x => new SellerProductImage()
                {
                    ImageUrl = x.Url
                });

                newProduct.ProductId = product.Id;
                newProduct.SellerId = seller.SellerId;


                #region Variants
                var categories = await _categoryRepository.FilterByAsync(z => product.ProductCategories.Select(xx => xx.CategoryId).Contains(z.Id));

                if (categories == null)
                    continue;

                var productCategory = categories.FirstOrDefault(x => x.Type == CategoryTypeEnum.MainCategory);
                if (productCategory == null)
                    continue;

                var variantGroups = await _productVariantService.GetProductWithVariants(product.Id, productCategory.Id);

                if (variantGroups.Count > 0)
                {
                    var areThereVariants = await AreThereVariants(variantGroups, productChannelCode, seller.SellerId);
                    newProduct.IsVariantable = areThereVariants;
                }
                #endregion

                prods.Add(newProduct);
                usedSellerId.Add(new CouponProductWithSeller() { ProductId = newProduct.SellerId, SellerId = newProduct.SellerId });
            }

            response.Data.Products = prods;


            switch (request.ProductFilterType)
            {
                case ProductFilterType.Brand:
                    var brand = await _brandRepository.FindByAsync(i => i.Id == request.BrandId);
                    response.Data.Breadcrumb = "Marka > " + brand.Name;
                    break;
                case ProductFilterType.Category:
                    var category = await _categoryRepository.FindByAsync(i => i.Id == request.CategoryId);
                    response.Data.Breadcrumb = category.DisplayName;
                    break;
            }

            response.Success = true;
            return response;
        }



        public async Task<bool> AreThereVariants(List<List<VariantGroup>> variants, ChannelCode channel, Guid sellerId)
        {
            var returnValue = false;
            var productIdsList = variants.SelectMany(x => x.Select(p => p.ProductId));
            var variantSellers = await _productSellerRepository.FilterByAsync(ps => productIdsList.Contains(ps.ProductId));

            for (int i = 0; i < variants.Count; i++)
            {
                foreach (var item in variants[i])
                {
                    ProductSeller seller;
                    IEnumerable<ProductSeller> sellers;
                    if (channel == ChannelCode.IsCep)
                        sellers = variantSellers.Where(x => x.ProductId == item.ProductId && x.SellerId == sellerId);
                    else
                        sellers = variantSellers.Where(x => x.ProductId == item.ProductId);

                    if (sellers.Count() > 1 && sellers.Any(x => x.StockCount > 0))
                        seller = sellers.Where(x => x.StockCount > 0).OrderBy(o => o.SalePrice).FirstOrDefault();
                    else
                        seller = sellers.FirstOrDefault();

                    if (sellers.Count() > 1)
                    {
                        returnValue = true;
                        break;
                    }
                }
            }

            return returnValue;
        }
    }
}