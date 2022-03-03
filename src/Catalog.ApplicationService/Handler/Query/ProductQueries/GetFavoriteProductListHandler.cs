using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.ApplicationService.Assembler;
using Catalog.ApplicationService.Handler.Services;
using Catalog.Domain.CategoryAggregate.ServiceModel;
using Catalog.Domain.ProductAggregate;
using Catalog.Domain.ProductAggregate.ServiceModels;

using Framework.Core.Authorization;
using Framework.Core.Model;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.ProductQueries
{
    public class GetFavoriteProductListHandler : IRequestHandler<GetFavoriteProductListQuery, ResponseBase<FavoriteProductList>>
    {
        private readonly IFavoriteProductRepository _favoriteProductRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductAssembler _productAssembler;
        private readonly IIdentityContext _identityContext;
        private readonly IProductVariantService _productVariantService;
        private readonly ICategoryService _categoryService;


        public GetFavoriteProductListHandler(IProductAssembler productAssembler, IProductRepository productRepository, IFavoriteProductRepository favoriteProductRepository, IIdentityContext identityContext, IProductVariantService productVariantService, ICategoryService categoryService)
        {
            _productRepository = productRepository;
            _productAssembler = productAssembler;
            _favoriteProductRepository = favoriteProductRepository;
            _identityContext = identityContext;
            _productVariantService = productVariantService;
            _categoryService = categoryService;
        }

        public async Task<ResponseBase<FavoriteProductList>> Handle(GetFavoriteProductListQuery request, CancellationToken cancellationToken)
        {
            var list = await _favoriteProductRepository.GetFavoriteProductsByCustomerId(_identityContext.GetUserInfo().Id, request.PagerInput);
            var products = new List<ProductFavorite>();
            var variantableList = new List<Guid>();
            if (list.TotalCount <= 0)
                return _productAssembler.MapToGetFavoriteProductListQueryResult(products, list.TotalCount, variantableList, new List<CategoryIdAndNameforProducts>());
            else
            {
                var categoryList = await _categoryService.GetCagetoriesIdAndNameProducts(list.List.Select(o => o.ProductId).ToList());
                foreach (var item in list.List)
                {
                    var productItem = await _productRepository.GetFavoriteProductsByProductId(item.ProductId, item.Id);
                    if (productItem.Product != null)
                    {
                        products.Add(productItem);
                    }
                    else
                        list.TotalCount--;
                }
                variantableList = await _productVariantService.VariantableProductIds(products.Select(u => u.Product).ToList());
                return _productAssembler.MapToGetFavoriteProductListQueryResult(products, list.TotalCount, variantableList, categoryList);
            }
        }
    }
}
