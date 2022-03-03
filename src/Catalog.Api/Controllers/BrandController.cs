using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Request.Command.BrandCommands;
using Catalog.ApiContract.Request.Query.BrandQueries;
using Catalog.ApiContract.Response.Command.BrandCommands;
using Catalog.ApiContract.Response.Query.BrandQueries;
using Framework.Core.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Api.Controllers
{

    [Produces("application/json")]
    [Route("brand")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BrandController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<CreateBrand>))]
        public async Task<IActionResult> Create([FromBody] CreateBrandCommand request)
        {
            var createBrandResult = await _mediator.Send(request);
            return Ok(createBrandResult);
        }

        [HttpGet("getBrands")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<List<BrandDto>>))]
        public async Task<IActionResult> GetBrands([FromQuery] GetBrandQuery request)
        {
            var brandQueryResult = await _mediator.Send(request);
            return Ok(brandQueryResult);
        }
        [HttpPost("getBrandsByName")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetBrandIdAndNameQuery>))]
        public async Task<IActionResult> GetBrandsByName([FromBody] GetBrandsByNameQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
        [HttpPut("update")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<BrandDto>))]
        public async Task<IActionResult> UpdateBrand([FromBody] UpdateBrandCommand request)
        {
            var updateBrandQueryResult = await _mediator.Send(request);
            return Ok(updateBrandQueryResult);
        }

        [HttpDelete("delete")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<BrandDto>))]
        public async Task<IActionResult> Delete([FromBody] DeleteBrandCommand request)
        {
            var deleteBrandQueryResult = await _mediator.Send(request);
            return Ok(deleteBrandQueryResult);
        }

        [HttpGet("exist")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<BrandExist>))]
        public async Task<IActionResult> Exist(BrandExistQuery request)
        {
            var brandExistQueryResult = await _mediator.Send(request);
            return Ok(brandExistQueryResult);
        }


        [HttpPost("getBrandsIdAndName")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<BrandDto>))]
        public async Task<IActionResult> GetBrandsIdAndName([FromBody] GetBrandsIdAndNameQuery request)
        {
            var brandQueryResult = await _mediator.Send(request);
            return Ok(brandQueryResult);
        }
        [HttpPost("getBrandsSearchOptimization")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetBrandsSearchOptimizationQueryResult>))]
        public async Task<IActionResult> GetBrandsSearchOptimization(GetBrandsSearchOptimizationQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
        [HttpGet("getSeoBrands")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetSeoBrands>))]
        public async Task<IActionResult> GetSeoBrands([FromQuery] GetSeoBrandsQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
        [HttpPost("getBrandsBySeoName")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetBrandIdAndSeoNameQuery>))]
        public async Task<IActionResult> GetBrandsBySeoName([FromBody] GetBrandsBySeoNameQuery request)
        {

            var result = await _mediator.Send(request);
            return Ok(result);
        }
        [HttpGet("getBrandsForBO")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<List<BrandDto>>))]
        public async Task<IActionResult> GetBrandsForBO([FromQuery] GetBrandsQueryForBO request)
        {
            var brandQueryResult = await _mediator.Send(request);
            return Ok(brandQueryResult);
        }
    }
}
