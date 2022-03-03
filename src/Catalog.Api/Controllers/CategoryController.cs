using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Request.Command.CategoryCommands;
using Catalog.ApiContract.Request.Query;
using Catalog.ApiContract.Request.Query.CategoryQueries;
using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.ApiContract.Response.Command.CategoryCommands;
using Catalog.ApiContract.Response.Query.CategoryQueries;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Framework.Core.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Api.Controllers
{

    [Route("category")]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoryController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet("getActiveCategoryByCategoryId")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetActiveCategoryByCategoryIdResult>))]
        public async Task<IActionResult> GetActiveCategoryByCategoryId(GetActiveCategoryByCategoryId request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);

        }
        [HttpPost("create")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<CreateCategory>))]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand request)
        {
            var requestHeader = _httpContextAccessor.HttpContext.Request.Headers["ahmet"];
            //request.Ahmet = requestHeader;
            var healtCheckQueryResult = await _mediator.Send(request);
            return Ok(healtCheckQueryResult);
        }

        [HttpPut("update")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<CategoryDto>))]
        public async Task<IActionResult> Update([FromBody] UpdateCategoryCommand request)
        {
            var updateCategoryQueryResult = await _mediator.Send(request);
            return Ok(updateCategoryQueryResult);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteCategoryCommand request)
        {
            var deleteQueryResult = await _mediator.Send(request);
            return Ok(deleteQueryResult);
        }

        [HttpPost("addAttribute")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<CategoryAttributeDto>))]
        public async Task<IActionResult> AddAttribute([FromBody] CreateCategoryAttributeCommand request)
        {
            var healtCheckQueryResult = await _mediator.Send(request);
            return Ok(healtCheckQueryResult);
        }

        [HttpGet("getCategoryWithAttributes")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetCategoryWithAttributes>))]
        public async Task<IActionResult> GetCategoryWithAttributes(GetCategoryWithAttributesQuery request)
        {
            var healtCheckQueryResult = await _mediator.Send(request);
            return Ok(healtCheckQueryResult);
        }

        [HttpGet("healthcheck")]
        public async Task<IActionResult> HealthCheck()
        {
            var healtCheckQueryResult = await _mediator.Send(new HealthCheckQuery());
            return Ok(healtCheckQueryResult);
        }

        [HttpGet("getCategoryTree")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<CategoryTree>))]
        public async Task<IActionResult> GetCategoryTree(GetCategoryTreeQuery request)
        {
            var getCategoryTreeQueryResult = await _mediator.Send(request);
            return Ok(getCategoryTreeQueryResult);
        }

        [HttpPost("getAllCategoryNames")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<List<CategoryIdAndName>>))]
        public async Task<IActionResult> GetAllCategoryNames([FromBody] GetAllCategoriesQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("getCategoriesForMobile")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<CategoryResultForMobile>))]
        public async Task<IActionResult> GetCategoriesForMobile(GetCategoriesForMobileQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("createCategoryInstallment")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<CategoryInstallmentDto>))]
        public async Task<IActionResult> CreateCategoryInstallment(CreateCategoryInstallmentCommand request)
        {
            var healtCheckQueryResult = await _mediator.Send(request);
            return Ok(healtCheckQueryResult);
        }

        [HttpPost("updateCategoryInstallment")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<CategoryInstallmentDto>))]
        public async Task<IActionResult> UpdateCategoryInstallment(UpdateCategoryInstallmentCommand request)
        {
            var healtCheckQueryResult = await _mediator.Send(request);
            return Ok(healtCheckQueryResult);
        }

        [HttpPost("deleteCategoryInstallment")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<CategoryInstallmentDto>))]
        public async Task<IActionResult> DeleteCategoryInstallment(DeleteCategoryInstallmentCommand request)
        {
            var healtCheckQueryResult = await _mediator.Send(request);
            return Ok(healtCheckQueryResult);
        }
        [HttpGet("getCategoryInstallment")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<CategoryInstallmentDto>))]
        public async Task<IActionResult> GetCategoryInstallment(GetCategoryInstallmentQuery request)
        {
            var healtCheckQueryResult = await _mediator.Send(request);
            return Ok(healtCheckQueryResult);
        }
        [HttpGet("categoryIsExist")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<CategoryIsExistQueryResult>))]
        public async Task<IActionResult> CategoryIsExist(CategoryIsExistQuery request)
        {
            var categoryExistQueryResult = await _mediator.Send(request);
            return Ok(categoryExistQueryResult);
        }

        [HttpGet("checkCategoryIsExistOrLeaf")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<object>))]
        public async Task<IActionResult> CheckCategoryIsExistOrLeaf(CheckCategoryIsExistOrLeafQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
        [HttpGet("getCategoryTreeForSeller")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<CategoryResultForMobile>))]
        public async Task<IActionResult> GetCategoryTreeForSeller(GetCategoryTreeForSellerQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
        [HttpGet("getCategoryAndImageBySellerId")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<List<GetCategoryAndImageBySellerIdResult>>))]
        public async Task<IActionResult> GetCategoryAndImageBySellerId(GetCategoryAndImageBySellerIdQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
        [HttpPost("getIdNumberControl")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetIdNumberControlQueryResult>))]
        public async Task<IActionResult> GetCategoryIdNumberControl([FromBody] GetIdNumberControlQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
        [HttpPost("getCategoryTreeSearchOptimization")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetCategoryTreeSearchOptimizationQueryResult>))]
        public async Task<IActionResult> GetCategoryTreeSearchOptimization([FromBody] GetCategoryTreeSearchOptimizationQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("getCategoryById")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetCategoriesResult>))]
        public async Task<IActionResult> GetCategories(GetCategoriesQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("calculateLeafPaths")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<object>))]
        public async Task<IActionResult> CalculateLeafPaths()
        {
            var result = await _mediator.Send(new CalculateLeafPathsCommand());
            return Ok(result);
        }

        [HttpPost("calculateHasProducts")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<object>))]
        public async Task<IActionResult> CalculateHasProducts()
        {
            var result = await _mediator.Send(new CalculateHasProductsCommand());
            return Ok(result);
        }

        [HttpPost("bulkInsertCategoryCategoryAttributeAndMap")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<object>))]
        public async Task<IActionResult> BulkInsertCategoryCategoryAttributeAndMap([FromForm] IFormFile file)
        {
            var request = new BulkInsertCategoryCategoryAttributeAndMapCommand { File = file };
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("createVirtualCategory")]
        [ProducesResponseType(200, Type = typeof(CreateVirtualCategoryResult))]
        public async Task<IActionResult> CreateVirtualCategory([FromBody] CreateVirtualCategoryCommand req)
        {
            var result = await _mediator.Send(req);
            return Ok(result);
        }

        [HttpPost("getCategoriesByName")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetCategoriesByNameQueryResult>))]
        public async Task<IActionResult> GetCategoriesByName([FromBody] GetCategoriesByNameQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("getCategoryParentsNames")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<List<GetCategoryParentsNamesResult>>))]
        public async Task<IActionResult> GetCategoryParentsNames([FromBody] GetCategoryParentsNamesQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
        [HttpGet("getSeoCategories")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetSeoCategoriesResult>))]
        public async Task<IActionResult> GetSeoCategories([FromQuery] GetSeoCategoriesQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("getCategoryGroups")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetCategoryGroupsResponse>))]
        public async Task<IActionResult> GetCategoryGroups([FromQuery] GetCategoryGroupsQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}