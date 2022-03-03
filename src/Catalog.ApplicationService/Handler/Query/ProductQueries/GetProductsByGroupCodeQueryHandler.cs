using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.ApiContract.Response.Query.ProductQueries;
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
    public class GetProductsByGroupCodeQueryHandler : IRequestHandler<GetProductsByGroupCodeQuery, ResponseBase<GetProductsByGroupCode>>
    {
        private readonly IProductGroupRepository _productGroupRepository;
        private readonly IProductRepository _productRepository;
        private readonly IAttributeRepository _attributeRepository;
        private readonly IAttributeValueRepository _attributeValueRepository;
        private readonly ICategoryAttributeRepository _categoryAttributeRepository;

        public GetProductsByGroupCodeQueryHandler(IProductGroupRepository productGroupRepository,
            IProductRepository productRepository,
            IAttributeRepository attributeRepository,
            IAttributeValueRepository attributeValueRepository,
            ICategoryAttributeRepository categoryAttributeRepository)
        {
            _productGroupRepository = productGroupRepository;
            _productRepository = productRepository;
            _attributeRepository = attributeRepository;
            _attributeValueRepository = attributeValueRepository;
            _categoryAttributeRepository = categoryAttributeRepository;
        }

        public async Task<ResponseBase<GetProductsByGroupCode>> Handle(GetProductsByGroupCodeQuery request, CancellationToken cancellationToken)
        {
            var productGroups = await _productGroupRepository.FilterByAsync(x => x.GroupCode == request.GroupCode);
            var products = await _productRepository.FilterEagerLoading(x => productGroups.Select(pg => pg.ProductId).Contains(x.Id), null, "ProductAttributes,ProductCategories");

            if (products == null || products.Count == 0)
                return new ResponseBase<GetProductsByGroupCode> { Data = null, Success = false };

            var returnList = new List<ProductByGroupCode>();

            var attributeIds = products.SelectMany(x => x.ProductAttributes.Select(pa => pa.AttributeId));
            var attributeValueIds = products.SelectMany(x => x.ProductAttributes.Select(pav => pav.AttributeValueId));
            var categoryIds = products.SelectMany(x => x.ProductCategories.Select(pc => pc.CategoryId));

            var attributes = await _attributeRepository.FilterByAsync(x => attributeIds.Contains(x.Id));
            var attributeValues = await _attributeValueRepository.FilterByAsync(x => attributeValueIds.Contains(x.Id));

            var categoryAttributes = await _categoryAttributeRepository.FilterByAsync(z => categoryIds.Contains(z.CategoryId)
            && attributes.Select(aa => aa.Id).Contains(z.AttributeId) && z.IsVariantable);

            foreach (var product in products)
            {
                var productByGroupCode = new ProductByGroupCode
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    DisplayName = product.DisplayName,
                    Code = product.Code,
                    GroupCode = request.GroupCode,
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
                    }).ToList()
                };

                productByGroupCode.Attributes.RemoveAll(x => x.IsVariantable == false);
                returnList.Add(productByGroupCode);
            }

            return new ResponseBase<GetProductsByGroupCode>
            { Data = new GetProductsByGroupCode { ProductsByGroupCode = returnList }, Success = true };

        }
    }
}
