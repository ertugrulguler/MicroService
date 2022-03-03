using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.ApiContract.Response.Query.ProductQueries;
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

namespace Catalog.ApplicationService.Handler.Query.ProductQueries
{
    public class GetProductsSearchOptimizationQueryHandler : IRequestHandler<GetProductsSearchOptimizationQuery, ResponseBase<GetProductsSearchOptimizationQueryResult>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IAttributeRepository _attributeRepository;
        private readonly IAttributeValueRepository _attributeValueRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;

        public GetProductsSearchOptimizationQueryHandler(IProductRepository productRepository, IAttributeRepository attributeRepository, IAttributeValueRepository attributeValueRepository, ICategoryRepository categoryRepository, IBrandRepository brandRepository)
        {
            _productRepository = productRepository;
            _attributeRepository = attributeRepository;
            _attributeValueRepository = attributeValueRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
        }

        public async Task<ResponseBase<GetProductsSearchOptimizationQueryResult>> Handle(GetProductsSearchOptimizationQuery request, CancellationToken cancellationToken)
        {
            var productList = new List<SearchOptimizationProductList>();

            var pagedList = await _productRepository.GetProductsSearchOptimizationByPagingAsync(new PagerInput(1, 1000), request.CreatedDate);

            GetProductsSearchOptimizationQueryResult productSearchQueryResult;
            if (pagedList.Count < 1)
            {
                productSearchQueryResult = new GetProductsSearchOptimizationQueryResult
                {
                    Next = false,
                    SearchOptimizationProductList = null,
                    LastDateTime = DateTime.Now
                };
            }

            else
            {
                var attributeIds = pagedList.SelectMany(x => x.ProductAttributes.Select(pa => pa.AttributeId));
                var attributeValueIds = pagedList.SelectMany(x => x.ProductAttributes.Select(pav => pav.AttributeValueId));
                var categoryIds = pagedList.SelectMany(x => x.ProductCategories.Select(pc => pc.CategoryId));
                var brandIds = pagedList.Select(p => p.BrandId);
                var attributes = await _attributeRepository.FilterByAsync(a => attributeIds.Contains(a.Id));
                var attributeValues = await _attributeValueRepository.FilterByAsync(av => attributeValueIds.Contains(av.Id));
                var categories = await _categoryRepository.FilterByAsync(c => categoryIds.Contains(c.Id));
                var brand = await _brandRepository.FilterByAsync(b => brandIds.Contains(b.Id));

                productList.AddRange(pagedList.Select(product => new SearchOptimizationProductList
                {
                    Code = product.Code,
                    Name = product.Name,
                    DisplayName = product.DisplayName,
                    Description = product.Description,
                    BrandId = product.BrandId,
                    BrandName = brand.FirstOrDefault(b => b.Id == product.BrandId)?.Name,
                    GroupCode = product.ProductGroups.FirstOrDefault(x => x.ProductId == product.Id)?.GroupCode,
                    Categories = product.ProductCategories.Select(pc => new SearchOptimizationProductCategories
                    { CategoryId = categories.FirstOrDefault(c => c.Id == pc.CategoryId)?.Id, CategoryName = categories.FirstOrDefault(c => c.Id == pc.CategoryId)?.Name }).ToList(),
                    Attributes = product.ProductAttributes.Select(pa => new SearchOptimizationProductAttributes
                    { AttributeName = attributes.FirstOrDefault(a => a.Id == pa.AttributeId)?.Name, AttributeValue = attributeValues.FirstOrDefault(a => a.Id == pa.AttributeValueId)?.Value }).ToList()
                }));

                productSearchQueryResult = new GetProductsSearchOptimizationQueryResult
                {
                    Next = pagedList.TotalCount > 1000,
                    SearchOptimizationProductList = productList,
                    LastDateTime = pagedList.Last().ModifiedDate.Value
                };
            }

            return new ResponseBase<GetProductsSearchOptimizationQueryResult>
            {
                Data = productSearchQueryResult,
                Success = true,
            };
        }
    }
}
