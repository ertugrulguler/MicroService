using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Request.Command.ProductCommands;
using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.ApiContract.Response.Command.ProductCommands;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.Domain.ProductAggregate;
using Framework.Core.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Api.Controllers
{
    [Produces("application/json")]
    [Route("product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("createOrUpdateProduct")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<ProductDto>))]
        public async Task<IActionResult> Create(CreateProductCommand request)
        {
            var healtCheckQueryResult = await _mediator.Send(request);
            return Ok(healtCheckQueryResult);
        }

        [HttpDelete("delete")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<ProductDto>))]
        public async Task<IActionResult> Delete(DeleteProductCommand request)
        {
            var deleteQueryResult = await _mediator.Send(request);
            return Ok(deleteQueryResult);
        }

        [HttpGet("getProduct")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<ProductDetail>))]
        public async Task<IActionResult> GetProduct([FromQuery] GetProductQuery request)
        {
            var productQueryResult = await _mediator.Send(request);
            return Ok(productQueryResult);
        }

        [HttpPost("getProductListAndFilter")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<ProductListAndFilterQueryResult>))]
        public async Task<IActionResult> GetProductListAndFilter(GetProductListAndFilterQuery request)
        {
            var productListQueryResult = await _mediator.Send(request);
            return Ok(productListQueryResult);
        }

        [HttpPost("getProductRelationControl")]
        public async Task<IActionResult> GetProductRelationControl([FromBody] GetProductRelationControlQuery request)
        {
            var productCheck = await _mediator.Send(request);
            return Ok(productCheck);
        }

        [HttpPost("getProductDetail")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetProductDetailResponse>))]
        public async Task<IActionResult> GetProductDetailBySellerId([FromBody] GetProductDetailQuery request)
        {
            var productDetailResult = await _mediator.Send(request);
            return Ok(productDetailResult);
        }

        [HttpPost("getProductVariants")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetProductVariantsResponse>))]
        public async Task<IActionResult> GetProductVariants([FromBody] GetProductVariantsQuery request)
        {
            var productVariantsResult = await _mediator.Send(request);
            return Ok(productVariantsResult);
        }


        [HttpGet("getProductsDetailBySellerId")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<List<SellerProducts>>))]
        public async Task<IActionResult> GetProductsDetailBySellerId(
            [FromQuery] GetProductsDetailBySellerIdQuery request)

        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("updatePriceControl")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<ProductSellerDto>))]
        public async Task<IActionResult> UpdatePriceControl(UpdatePriceControlCommand request)
        {
            var updatePriceControlQueryResult = await _mediator.Send(request);
            return Ok(updatePriceControlQueryResult);
        }

        [HttpPost("productBarcodeControl")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<object>))]
        public async Task<IActionResult> ProductBarcodeControl(ProductBarcodeControlQuery request)
        {
            var productQueryResult = await _mediator.Send(request);
            return Ok(productQueryResult);
        }

        [HttpPost("getProductIdByCode")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<object>))]
        public async Task<IActionResult> ProductIdByCode(GetProductIdByCodeQuery request)
        {
            var productIdByCode = await _mediator.Send(request);
            return Ok(productIdByCode);
        }

        [HttpGet("getSellerProductCountDetails")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<List<GetSellerProductCountsDetail>>))]
        public async Task<IActionResult> GetSellerProductCountDetails([FromQuery] GetSellerProductCountDetailsQuery request)
        {
            var getSellerProductCountDetails = await _mediator.Send(request);
            return Ok(getSellerProductCountDetails);
        }

        [HttpPost("getProductDeliveryList")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetProductDelivery>))]
        public async Task<IActionResult> GetProductDeliveryList(GetProductDeliveryListQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("getProductListForSeller")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<List<SellerProductInfo>>))]
        public async Task<IActionResult> GetProductListForSeller([FromQuery] GetProductListForSellerQuery request)
        {
            var productList = await _mediator.Send(request);
            return Ok(productList);
        }

        [HttpPost("getProductSearchList")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<List<ProductSearchList>>))]
        public async Task<IActionResult> GetProductSearch(GetProductSearchQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        /// <summary>
        /// Catalogda ki bir ürünü, baska bir seller'a create edip, onaya gonder.
        /// </summary>>
        [HttpPost("getProductDetailToCreate")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetProductDetailToCreate>))]
        public async Task<IActionResult> GetProductDetailToCreate(GetProductDetailToCreateQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("updateProductStockCount")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<UpdatePriceControlResult>))]
        public async Task<IActionResult> UpdateProductStockCount(UpdateProductStockCountCommand request)
        {
            var updateProductStockCountResult = await _mediator.Send(request);
            return Ok(updateProductStockCountResult);
        }

        [HttpPost("updatePrice")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<UpdatePriceControlResult>))]
        public async Task<IActionResult> UpdatePrice(UpdatePriceCommand request)
        {
            var updatePriceResult = await _mediator.Send(request);
            return Ok(updatePriceResult);
        }

        [HttpPost("getProductsSearchOptimization")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetProductsSearchOptimizationQueryResult>))]
        public async Task<IActionResult> GetProductsSearchOptimization(GetProductsSearchOptimizationQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("productTransferToVirtualCategory")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<object>))]
        public async Task<IActionResult> ProductTransferToVirtualCategory(ProductTransferToVirtualCategoryCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("addAndUpdateFavoriteProduct")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(ResponseBase<FavoriteProductDto>))]
        public async Task<IActionResult> AddAndUpdateFavoriteProduct(AddAndUpdateFavoriteProductCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
        [HttpPost("getFavoriteProductList")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(ResponseBase<FavoriteProductList>))]
        public async Task<IActionResult> GetFavoriteProductList(GetFavoriteProductListQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
        [HttpPost("getProductListAndFilterBySeller")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetProductListAndFilterBySellerQueryResult>))]
        public async Task<IActionResult> GetProductListAndFilterBySeller(GetProductListAndFilterBySellerQuery request)
        {
            var productListQueryResult = await _mediator.Send(request);
            return Ok(productListQueryResult);
        }

        [HttpPost("productOneChannelCreate")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<object>))]
        public async Task<IActionResult> ProductOneChannelCreate([FromBody] CreateProductOneChannelCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("getDefaultProductImage")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetDefaultProductImage>))]
        public async Task<IActionResult> GetDefaultProductImage([FromQuery] GetDefaultProductImageQuery request)
        {
            var productImage = await _mediator.Send(request);
            return Ok(productImage);
        }

        [HttpGet("productGroups")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<List<string>>))]
        public async Task<IActionResult> GetProductGroups([FromQuery] GetProductGroupsQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("productsByGroupCode")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetProductsByGroupCode>))]
        public async Task<IActionResult> GetProductsByGroupCode([FromQuery] GetProductsByGroupCodeQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("getProductsFilter")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetProductsFilterQueryResult>))]
        public async Task<IActionResult> GetProductsFilter(GetProductsFilterQuery request)
        {
            var productListQueryResult = await _mediator.Send(request);
            return Ok(productListQueryResult);
        }

        /// <summary>
        /// Virtual Category added products.
        /// </summary>>
        /// <param name="ProductCode"></param>
        [HttpPost("addProductVirtualCategory")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<AddProductVirtualCategoryResult>))]
        public async Task<IActionResult> AddProductVirtualCategory(AddProductVirtualCategoryCommand req)
        {
            var result = await _mediator.Send(req);
            return Ok(result);
        }
        [HttpPost("getProductPriceAndStock")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetProductPriceAndStockQueryResult>))]
        public async Task<IActionResult> GetProductPriceAndStock(GetProductPriceAndStockQuery request)
        {
            var productListQueryResult = await _mediator.Send(request);
            return Ok(productListQueryResult);
        }
        [HttpPost("getProductVariantListForSeller")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<List<ProductVariantGroup>>))]
        public async Task<IActionResult> GetProductVariantListForSeller(GetProductVariantListForSellerQuery request)
        {
            var productList = await _mediator.Send(request);
            return Ok(productList);
        }

        [HttpPost("getProductSeoUrl")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetProductSeoUrlResponse>))]
        public async Task<IActionResult> GetProductSeoUrl(GetProductSeoUrlQuery request)
        {
            var productList = await _mediator.Send(request);
            return Ok(productList);
        }

        [HttpPost("getProductSeoUrlList")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetProductSeoUrlListResponse>))]
        public async Task<IActionResult> GetProductSeoUrlList(GetProductSeoUrlListQuery request)
        {
            var productList = await _mediator.Send(request);
            return Ok(productList);
        }

        [HttpGet("returnable/{id}")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<bool>))]
        public async Task<IActionResult> ProductReturnable(System.Guid id)
        {
            var result = await _mediator.Send(new GetProductReturnableQuery { Id = id });
            return Ok(result);
        }
        [HttpPost("getProductSellerDetailToBanner")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<ProductSeller>))]
        public async Task<IActionResult> GetProductSellerDetailToBanner(GetProductSellerDetailToBannerQuery request)
        {
            var productSellerQueryResult = await _mediator.Send(request);
            return Ok(productSellerQueryResult);
        }
        [HttpPost("createXmlWithProducts")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<string>))]
        public async Task<IActionResult> CreateXmlWithProducts(CreateXmlWithProductsCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("getProductListFilter")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetProductListFilterQueryResponse>))]
        public async Task<IActionResult> GetProductListFilter(GetProductListFilterQueryRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}