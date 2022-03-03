using Catalog.ApiContract.Request.Query.BrandQueries;
using Catalog.ApiContract.Response.Query.BrandQueries;
using Catalog.Domain.BrandAggregate;
using Catalog.Domain.ProductAggregate;
using Framework.Core.Model;
using MediatR;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.BrandQueries
{
    public class GetBrandsSearchOptimizationQueryHandler : IRequestHandler<GetBrandsSearchOptimizationQuery, ResponseBase<GetBrandsSearchOptimizationQueryResult>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IProductRepository _productRepository;

        public GetBrandsSearchOptimizationQueryHandler(IBrandRepository brandRepository, IProductRepository productRepository)
        {
            _brandRepository = brandRepository;
            _productRepository = productRepository;
        }

        public async Task<ResponseBase<GetBrandsSearchOptimizationQueryResult>> Handle(GetBrandsSearchOptimizationQuery request, CancellationToken cancellationToken)
        {
            var productBrandByGroupBy = await _productRepository.GetProductsBrandSearchOptimization(request.CreatedDate);
            var brands = productBrandByGroupBy.BrandList;

            //  var product = await _productRepository.GetProductsSearchOptimization(request.CreatedDate);
            //var productBrands = product.Select(x => x.BrandId).Distinct();
            var count = productBrandByGroupBy.Count;
            //var productBrandFilter = productBrands.Take(1000).ToList();
            var lastDateTime = productBrandByGroupBy.LastDateTime;

            var brandFilter = new List<SearchOptimizationBrandList>();
            GetBrandsSearchOptimizationQueryResult brandSearchQueryResult;


            if (!brands.Any())
            {
                brandSearchQueryResult = new GetBrandsSearchOptimizationQueryResult
                {
                    Next = false,
                    SearchOptimizationBrandList = null,
                    LastDateTime = DateTime.Now
                };
            }

            else
            {
                brandFilter.AddRange(brands.Select(brand => new SearchOptimizationBrandList { Id = brand.BrandId, Name = brand.BrandName }));

                //var brand = await _brandRepository.FilterByAsync(b => productBrandFilter.Contains(b.Id));
                //foreach (var productBrand in brand)
                //{
                //    brandFilter.Add(new SearchOptimizationBrandList
                //    {
                //        Id = productBrand.Id,
                //        Name = productBrand.Name
                //    });

                //}

                brandSearchQueryResult = new GetBrandsSearchOptimizationQueryResult
                {
                    Next = count > 1000 ? true : false,
                    SearchOptimizationBrandList = brandFilter,
                    LastDateTime = lastDateTime
                };
            }
            return new ResponseBase<GetBrandsSearchOptimizationQueryResult>
            {
                Data = brandSearchQueryResult,
                Success = true,
            };
        }
    }
}
