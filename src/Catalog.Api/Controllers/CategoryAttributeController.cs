using Catalog.ApiContract.Request.Command.CategoryAttributeCommands;
using Catalog.ApiContract.Request.Command.CategoryCommands;
using Catalog.ApiContract.Response.Command.CategoryAttributeCommands;
using Catalog.ApiContract.Response.Command.CategoryCommands;
using Framework.Core.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Api.Controllers
{

    [Route("categoryAttribute")]
    public class CategoryAttributeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryAttributeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<List<CreateCategoryAttributeResult>>))]
        public async Task<IActionResult> Create([FromBody] AddCategoryAttributeCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPut("update")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<List<UpdateCategoryAttributeResult>>))]
        public async Task<IActionResult> Update([FromBody] UpdateCategoryAttributeCommand request)
        {
            var updateCategoryQueryResult = await _mediator.Send(request);
            return Ok(updateCategoryQueryResult);
        }

        [HttpPost("delete")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<List<DeleteCategoryAttributeResult>>))]
        public async Task<IActionResult> Delete([FromBody] DeleteCategoryAttributeCommand request)
        {
            var deleteQueryResult = await _mediator.Send(request);
            return Ok(deleteQueryResult);
        }
    }
}