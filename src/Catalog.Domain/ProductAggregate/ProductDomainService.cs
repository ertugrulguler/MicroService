using Catalog.Domain.AttributeAggregate;
using Catalog.Domain.CategoryAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Domain.ProductAggregate
{
    public class ProductDomainService : IProductDomainService
    {
        private readonly IProductGroupRepository _productGroupRepository;
        private readonly ICategoryAttributeRepository _categoryAttributeRepository;
        private readonly IProductGroupVariantRepository _productGroupVariantRepository;
        private readonly IAttributeRepository _attributeRepository;


        public ProductDomainService(
            IProductGroupRepository productGroupRepository,
            ICategoryAttributeRepository categoryAttributeRepository,
            IProductGroupVariantRepository productGroupVariantRepository,
            IAttributeRepository attributeRepository)
        {
            _productGroupRepository = productGroupRepository;
            _categoryAttributeRepository = categoryAttributeRepository;
            _productGroupVariantRepository = productGroupVariantRepository;
            _attributeRepository = attributeRepository;
        }

        public async Task<List<ProductGroup>> SaveProductGroup(List<ProductGroup> productGroups, Guid productId)
        {
            if (productGroups == null || productGroups.FirstOrDefault() == null)
                return new List<ProductGroup>();

            var _productGroups = new List<ProductGroup>();
            var dbProductGroups = await _productGroupRepository.FilterByAsync(x => productGroups.Select(pg => pg.GroupCode).Contains(x.GroupCode));

            foreach (var productGroup in productGroups)
            {
                var firstProductGroup = dbProductGroups.FirstOrDefault(x => x.ProductId == productId);
                var secondProductGroup = dbProductGroups.FirstOrDefault(x => x.ProductId == productGroup.ProductId);

                if (firstProductGroup == null && !_productGroups.Select(x => x.ProductId).Contains(productId))
                {
                    var newFirstProductGroup = new ProductGroup(productId, productGroup.GroupCode);
                    _productGroups.Add(newFirstProductGroup);
                }

                if (secondProductGroup == null)
                {
                    var newSecondProductGroup = new ProductGroup(productGroup.ProductId, productGroup.GroupCode);
                    _productGroups.Add(newSecondProductGroup);
                }
            }

            return _productGroups;
        }

        public async Task<List<ProductGroupVariant>> SaveProductGroupVariant(string groupCode, Guid categoryId, IReadOnlyCollection<ProductAttribute> productAttribute)
        {
            var categoryAttributeIds = await _categoryAttributeRepository.FilterByAsync(ca => ca.CategoryId == categoryId && ca.IsVariantable);

            var variantableAttributes = productAttribute.Where(x => categoryAttributeIds.Select(ca => ca.AttributeId).Contains(x.AttributeId));

            if (!variantableAttributes.Any())
                return new List<ProductGroupVariant>();

            var productGroupVariantList = new List<ProductGroupVariant>();
            var groupAttributes = await _attributeRepository.FilterByAsync(x => variantableAttributes.Select(va => va.AttributeId).Contains(x.Id));
            var productGroupVariants = await _productGroupVariantRepository.FilterByAsync(x => x.ProductGroupCode.Contains(groupCode));

            foreach (var variantableAttribute in variantableAttributes)
            {
                var attributeName = groupAttributes.FirstOrDefault(x => x.Id == variantableAttribute.AttributeId).Name;

                if (!productGroupVariants.Any(x => x.ProductGroupCode == $"{groupCode}-{attributeName}"))
                    productGroupVariantList.Add(new ProductGroupVariant($"{groupCode}-{attributeName}", variantableAttribute.AttributeId));
            }

            return productGroupVariantList;
        }
    }
}