using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.ApplicationService.Assembler;
using Catalog.ApplicationService.Communicator.Parameter;
using Catalog.Domain.AttributeAggregate;
using Catalog.Domain.BrandAggregate;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.ProductAggregate;
using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Attribute = Catalog.Domain.AttributeAggregate.Attribute;

namespace Catalog.ApplicationService.Handler.Query.ProductQueries
{
    public class GetProductsDetailBySellerIdQueryHandler : IRequestHandler<GetProductsDetailBySellerIdQuery, ResponseBase<List<SellerProducts>>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductAssembler _productAssembler;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IAttributeRepository _attributeRepository;
        private readonly IAttributeValueRepository _attributeValueRepository;
        private readonly IProductGroupRepository _productGroupRepository;
        private readonly IProductAttributeRepository _productAttributeRepository;
        private readonly IProductGroupVariantRepository _productGroupVariantRepository;
        private readonly IProductDeliveryRepository _productDeliveryRepository;
        private readonly IParameterCommunicator _parameterCommunicator;


        public GetProductsDetailBySellerIdQueryHandler(IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IProductAssembler productAssembler,
            IBrandRepository brandRepository,
            IAttributeRepository attributeRepository,
            IAttributeValueRepository attributeValueRepository, IProductGroupRepository productGroupRepository, IProductAttributeRepository productAttributeRepository, IProductGroupVariantRepository productGroupVariantRepository, IProductDeliveryRepository productDeliveryRepository, IParameterCommunicator parameterCommunicator)
        {
            _productRepository = productRepository;
            _productAssembler = productAssembler;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _attributeRepository = attributeRepository;
            _attributeValueRepository = attributeValueRepository;
            _productGroupRepository = productGroupRepository;
            _productAttributeRepository = productAttributeRepository;
            _productGroupVariantRepository = productGroupVariantRepository;
            _productDeliveryRepository = productDeliveryRepository;
            _parameterCommunicator = parameterCommunicator;
        }

        public async Task<ResponseBase<List<SellerProducts>>> Handle(GetProductsDetailBySellerIdQuery request, CancellationToken cancellationToken)
        {
            var products = new List<Product>();
            var attributes = new List<Attribute>();
            var attributeValues = new List<AttributeValue>();
            var categories = new List<Category>();
            var groups = new List<ProductGroups>();
            var groupVariants = new List<ProductGroupVariant>();


            if (!string.IsNullOrWhiteSpace(request.Code))
            {
                var productId = await _productRepository.FindByAsync(p => p.Code == request.Code);
                products.Add(await _productRepository.GetProductByProductSellerId(productId.Id, request.SellerId));
            }
            else
                products = await _productRepository.GetProductsDetailBySellerId
                (request.SellerId, new PagerInput(request.Page, request.Size));

            if (products == null || products.Count <= 0)
                return new ResponseBase<List<SellerProducts>>()
                {
                    Data = new List<SellerProducts>(),
                    Success = true
                };
            var brand = await _brandRepository.FindByAsync(z => z.Id == products.FirstOrDefault().BrandId);
            var cityList = (await _parameterCommunicator.GetCities()).Data;

            foreach (var product in products)
            {
                var productAttributes = await _attributeRepository.FilterByAsync(z => product.ProductAttributes.Select(xx => xx.AttributeId).Contains(z.Id));
                attributes.AddRange(productAttributes);

                var productAttributeValues = await _attributeValueRepository.FilterByAsync(x => product.ProductAttributes.Select(pa => pa.AttributeValueId).Contains(x.Id));
                attributeValues.AddRange(productAttributeValues);

                var productCategories = await _categoryRepository.FilterByAsync(z => product.ProductCategories.Select(xx => xx.CategoryId).Contains(z.Id));
                categories.AddRange(productCategories);

                var productGroups = await _productGroupRepository.FilterByAsync(x => product.ProductGroups.Select(pg => pg.GroupCode).Contains(x.GroupCode));
                foreach (var productGroup in productGroups)
                {
                    var productGroupVariants = await _productGroupVariantRepository.FilterByAsync(x => x.ProductGroupCode == productGroup.GroupCode);
                    groupVariants.AddRange(productGroupVariants);
                }

                var variantProductAttributes = await _productAttributeRepository.FilterByAsync(x =>
                    groupVariants.Select(pgv => pgv.AttributeId).Contains(x.AttributeId) &&
                    productGroups.Select(pg => pg.ProductId).Contains(x.ProductId));

                foreach (var variantProductAttribute in variantProductAttributes)
                {
                    if (variantProductAttribute.ProductId == products.FirstOrDefault().Id)
                        continue;

                    groups.Add(new ProductGroups
                    {
                        ProductId = variantProductAttribute.ProductId,
                        AttributeName = _attributeRepository.GetByIdAsync(variantProductAttribute.AttributeId).Result.Name,
                        AttributeValue = _attributeValueRepository.GetByIdAsync(variantProductAttribute.AttributeValueId).Result.Value
                    });
                }
            }

            return _productAssembler.MapToGetProductsDetailBySellerIdQueryResult(products, attributes, brand, categories, attributeValues, groups, cityList);

        }
    }
}
