using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.ApplicationService.Assembler;
using Catalog.ApplicationService.Handler.Services;
using Catalog.Domain;
using Catalog.Domain.AttributeAggregate;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;
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
    public class GetProductVariantsQueryHandler : IRequestHandler<GetProductVariantsQuery, ResponseBase<GetProductVariantsResponse>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductVariantService _productVariantService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAttributeRepository _attributeRepository;
        private readonly IAttributeValueRepository _attributeValueRepository;
        private readonly IProductSellerRepository _productSellerRepository;

        private readonly IProductAssembler _productAssembler;

        public GetProductVariantsQueryHandler(IProductRepository productRepository,
            IProductVariantService productVariantService,
            ICategoryRepository categoryRepository,
            IAttributeRepository attributeRepository,
            IAttributeValueRepository attributeValueRepository,
            IProductSellerRepository productSellerRepository,
            IProductAssembler productAssembler)
        {
            _productRepository = productRepository;
            _productVariantService = productVariantService;
            _categoryRepository = categoryRepository;
            _attributeRepository = attributeRepository;
            _attributeValueRepository = attributeValueRepository;
            _productSellerRepository = productSellerRepository;
            _productAssembler = productAssembler;
        }

        public async Task<ResponseBase<GetProductVariantsResponse>> Handle(GetProductVariantsQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductByProductSellerId(request.ProductId, request.SellerId);

            if (product == null)
                throw new BusinessRuleException(ApplicationMessage.ProductNotFound,
                 ApplicationMessage.ProductNotFound.Message(),
                 ApplicationMessage.ProductNotFound.UserMessage());

            if (product.ProductSellers.FirstOrDefault() == null)
                throw new BusinessRuleException(ApplicationMessage.InvalidId,
                 ApplicationMessage.InvalidId.Message(),
                 ApplicationMessage.InvalidId.UserMessage());

            var productSeller = product.ProductSellers.FirstOrDefault();


            #region Variant
            var groupedProductGroups = new List<List<ProductVariantGroup>>();
            var categoriesT = await _categoryRepository.FilterByAsync(c => product.ProductCategories.Select(pc => pc.CategoryId).Contains(c.Id));
            var catId = categoriesT.FirstOrDefault(x => x.Type == CategoryTypeEnum.MainCategory).Id;
            var variantGroups = await _productVariantService.GetProductWithVariants(product.Id, catId);

            if (variantGroups.Count > 0)
                groupedProductGroups = await ArrangeVariants(variantGroups);
            #endregion



            return _productAssembler.MapToGetProductVariantsQueryResult(product, productSeller, groupedProductGroups);
        }

        private async Task<List<List<ProductVariantGroup>>> ArrangeVariants(List<List<VariantGroup>> variants)
        {
            var groupedVariants = new List<List<ProductVariantGroup>>();

            var attributeIdsList = variants.SelectMany(x => x.Select(ss => ss.AttributeId));
            var attributeValueIdsList = variants.SelectMany(x => x.Select(ss => ss.AttributeValueId));
            var productIdsList = variants.SelectMany(x => x.Select(p => p.ProductId));

            var attributes = await _attributeRepository.FilterByAsync(a => attributeIdsList.Contains(a.Id));
            var attributeValues = await _attributeValueRepository.FilterByAsync(av => attributeValueIdsList.Contains(av.Id));
            var variantSellers = await _productSellerRepository.FilterByAsync(ps => productIdsList.Contains(ps.ProductId));

            for (int i = 0; i < variants.Count; i++)
            {
                var groupVariant = new List<ProductVariantGroup>();
                foreach (var item in variants[i])
                {
                    var seller = variantSellers.Where(x => x.ProductId == item.ProductId).OrderBy(o => o.SalePrice).FirstOrDefault();

                    groupVariant.Add(new ProductVariantGroup
                    {
                        ProductId = item.ProductId,
                        ProductSellerId = seller.Id,
                        SellerId = seller.SellerId,
                        AttributeName = attributes.FirstOrDefault(a => a.Id == item.AttributeId).DisplayName,
                        AttributeValue = attributeValues.FirstOrDefault(av => av.Id == item.AttributeValueId).Value,
                        OrderByAttributeValue = attributeValues.FirstOrDefault(av => av.Id == item.AttributeValueId).Order,
                        IsOpen = seller.StockCount > 0,
                        IsSelected = item.IsSelected
                    });
                }
                var orderedGroupVariant = groupVariant.OrderBy(gv => gv.OrderByAttributeValue).ToList();
                groupedVariants.Add(orderedGroupVariant);
            }

            return groupedVariants;
        }

    }
}
