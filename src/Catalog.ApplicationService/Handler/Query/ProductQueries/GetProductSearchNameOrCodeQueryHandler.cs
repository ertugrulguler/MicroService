using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.ApplicationService.Assembler;
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
    public class GetProductSearchNameOrCodeQueryHandler : IRequestHandler<GetProductSearchQuery, ResponseBase<GetProductSearchNameOrCodeQueryResult>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductAssembler _productAssembler;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAttributeRepository _attributeRepository;
        private readonly IAttributeValueRepository _attributeValueRepository;

        public GetProductSearchNameOrCodeQueryHandler(IProductRepository productRepository, IProductAssembler productAssembler,
              ICategoryRepository categoryRepository, IAttributeValueRepository attributeValueRepository, IAttributeRepository attributeRepository)
        {
            _productRepository = productRepository;
            _productAssembler = productAssembler;
            _categoryRepository = categoryRepository;
            _attributeValueRepository = attributeValueRepository;
            _attributeRepository = attributeRepository;
        }

        public async Task<ResponseBase<GetProductSearchNameOrCodeQueryResult>> Handle(GetProductSearchQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetProductSearchNameOrCode(request.Code, request.Name, new PagerInput(request.Page.Value, request.Size.Value));
            if (products == null || products.Count == 0)
                throw new BusinessRuleException(ApplicationMessage.ProductNotFound,
                ApplicationMessage.ProductNotFound.Message(),
                ApplicationMessage.ProductNotFound.UserMessage());

            var productAttributes = new List<Domain.AttributeAggregate.Attribute>();
            var productAttributeValues = new List<AttributeValue>();
            var productCategories = new List<Category>();

            foreach (var product in products)
            {
                var attributes = await _attributeRepository.FilterByAsync(z => product.ProductAttributes.Select(xx => xx.AttributeId).Contains(z.Id));
                productAttributes.AddRange(attributes);

                var attributeValues = await _attributeValueRepository.FilterByAsync(x => product.ProductAttributes.Select(pa => pa.AttributeValueId).Contains(x.Id));
                productAttributeValues.AddRange(attributeValues);

                var categories = await _categoryRepository.FilterByAsync(z => product.ProductCategories.Select(xx => xx.CategoryId).Contains(z.Id) && z.Type == Domain.Enums.CategoryTypeEnum.MainCategory);
                productCategories.AddRange(categories);
            }

            return _productAssembler.MapToGetSearchProductQueryResult(products, productCategories, productAttributes, productAttributeValues);

        }
    }
}
