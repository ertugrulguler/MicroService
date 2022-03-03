using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.ApplicationService.Handler.Services;
using Catalog.Domain.AttributeAggregate;

using Framework.Core.Model;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.ProductQueries
{
    public class GetProductVariantListForSellerQueryHandler : IRequestHandler<GetProductVariantListForSellerQuery, ResponseBase<List<ProductVariantGroup>>>
    {
        private readonly IProductVariantService _productVariantService;
        private readonly IAttributeValueRepository _attributeValueRepository;

        public GetProductVariantListForSellerQueryHandler(IProductVariantService productVariantService,
            IAttributeValueRepository attributeValueRepository)
        {
            _productVariantService = productVariantService;
            _attributeValueRepository = attributeValueRepository;
        }

        public async Task<ResponseBase<List<ProductVariantGroup>>> Handle(GetProductVariantListForSellerQuery request, CancellationToken cancellationToken)
        {
            var variantGroups = await _productVariantService.GetProductWithVariantsList(request.ProductIdList);
            var arrangedVariants = await ArrangeVariants(variantGroups);

            return new ResponseBase<List<ProductVariantGroup>>
            { Data = arrangedVariants, Success = true };

        }
        private async Task<List<ProductVariantGroup>> ArrangeVariants(List<VariantGroup> variants)
        {
            var groupedVariants = new List<ProductVariantGroup>();

            var attributeIdsList = variants.Select(ss => ss.AttributeId);

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
                    var attId = attributes.FirstOrDefault(a => a.Id == value.AttributeId);
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
