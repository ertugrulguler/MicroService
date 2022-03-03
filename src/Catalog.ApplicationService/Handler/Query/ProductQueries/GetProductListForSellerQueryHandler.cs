using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.ApiContract.Response;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.ApplicationService.Assembler;
using Catalog.ApplicationService.Handler.Services;
using Catalog.Domain;
using Catalog.Domain.AttributeAggregate;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.ProductAggregate;

using Framework.Core.Model;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.ProductQueries
{
    public class GetProductListForSellerQueryHandler : IRequestHandler<GetProductListForSellerQuery, ResponseBase<GetProductListForSellerQueryResult>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductSellerRepository _productSellerRepository;
        private readonly IProductAssembler _productAssembler;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductVariantService _productVariantService;
        private readonly IAttributeValueRepository _attributeValueRepository;

        public GetProductListForSellerQueryHandler(IProductRepository productRepository, IProductSellerRepository productSellerRepository,
            ICategoryRepository categoryRepository,
            IProductAssembler productAssembler, IProductVariantService productVariantService, IAttributeValueRepository attributeValueRepository)
        {
            _productRepository = productRepository;
            _productAssembler = productAssembler;
            _categoryRepository = categoryRepository;
            _productSellerRepository = productSellerRepository;
            _productVariantService = productVariantService;
            _attributeValueRepository = attributeValueRepository;
        }

        public async Task<ResponseBase<GetProductListForSellerQueryResult>> Handle(GetProductListForSellerQuery request, CancellationToken cancellationToken)
        {
            var productList = await _productSellerRepository.FilterByAsync(p => p.SellerId == request.SellerId);
            if (productList == null)
                throw new BusinessRuleException(ApplicationMessage.EmptySellerProducts,
                ApplicationMessage.EmptySellerProducts.Message(),
                ApplicationMessage.EmptySellerProducts.UserMessage());

            List<Guid> productIdList = new List<Guid>();

            foreach (var item in productList)
            {
                if ((request.State == Domain.Enums.ProductFilterForSeller.InSale && item.StockCount > 0)
                    || (request.State == Domain.Enums.ProductFilterForSeller.NonStock && item.StockCount == 0)
                    || (request.State == Domain.Enums.ProductFilterForSeller.All))
                {
                    productIdList.Add(item.ProductId);
                }
            }

            var products = new List<Product>();

            if (request.Page.HasValue && request.Page.Value > 0 && request.Size.HasValue && request.Size.Value > 0)
                products = await _productRepository.GetProductListForSellerWithPaging
                (productIdList, request.SellerId, request.Code, request.ProductName, request.BrandName, request.OrderByDate, request.GroupCode, new PagerInput(request.Page.Value, request.Size.Value));
            else
                products = await _productRepository.GetProductListForSeller
                (productIdList, request.SellerId, request.Code, request.ProductName, request.BrandName, request.OrderByDate, request.GroupCode);

            if (products == null)
                throw new BusinessRuleException(ApplicationMessage.EmptySellerProducts,
                ApplicationMessage.EmptySellerProducts.Message(),
                ApplicationMessage.EmptySellerProducts.UserMessage());

            //if (request.State != Domain.Enums.ProductFilterForSeller.All)
            //{
            //    products = products.Skip((request.Page.Value - 1) * request.Size.Value)
            //       .Take(request.Size.Value).ToList();
            //}

            List<Guid> categoryIdList = new List<Guid>();
            foreach (var productCategory in products)
            {
                categoryIdList.AddRange(productCategory.ProductCategories.Select(xx => xx.CategoryId));
            }
            var productCategories = await _categoryRepository.FilterByAsync(a => categoryIdList.Contains(a.Id) && a.Type == Domain.Enums.CategoryTypeEnum.MainCategory);

            var variantGroups = await _productVariantService.GetProductWithVariantsList(products.Select(x => x.Id).ToList());
            var arrangedVariants = await ArrangeVariants(variantGroups);

            var result = _productAssembler.MapToGetProductsForSellerQueryResult(products, productCategories, arrangedVariants);

            return new ResponseBase<GetProductListForSellerQueryResult>
            {
                Data = new GetProductListForSellerQueryResult
                {
                    Products = result.Data.Products,
                    PageResponse = new PageResponse(productIdList.Count, new(request.Page.Value, request.Size.Value))
                },
                Success = true
            };

        }
        private async Task<List<ProductVariantGroup>> ArrangeVariants(List<VariantGroup> variants)
        {
            var groupedVariants = new List<ProductVariantGroup>();

            var attributeIdsList = variants.Select(ss => ss.AttributeValueId);

            var attributes = await _attributeValueRepository.FilterByAsync(a => attributeIdsList.Contains(a.Id));

            var productList = variants.GroupBy(s => s.ProductId, (k, g) => new
            {
                Key = k,
                Value = g

            }).ToList();

            foreach (var item in productList)
            {
                var attributeName = string.Empty;
                foreach (var value in item.Value)
                {
                    var attId = attributes.FirstOrDefault(a => a.Id == value.AttributeValueId);
                    if (attId != null)
                        attributeName += attId.Value + ",";
                }

                groupedVariants.Add(new ProductVariantGroup
                {
                    ProductId = item.Key,
                    AttributeName = attributeName.TrimEnd(',')
                });
            }

            return groupedVariants;
        }
    }
}
