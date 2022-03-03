using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Request.Command.AttributeCommands;
using Catalog.ApiContract.Request.Query.AttributeQueries;
using Catalog.ApiContract.Response.Command.AttributeCommands;
using Catalog.ApiContract.Response.Query.AttributeQueries;
using Catalog.ApplicationService.Handler.Command.AttributeCommands;
using Framework.Core.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Api.Controllers
{
    [Route("attribute")]
    [Produces("application/json")]
    public class AttributeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AttributeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<AttributeDto>))]
        public async Task<IActionResult> Create([FromBody] CreateAttributeCommand request)
        {
            var createAttributeResult = await _mediator.Send(request);
            return Ok(createAttributeResult);
        }

        [HttpPost("updateAttribute")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<AttributeDto>))]
        public async Task<IActionResult> UpdateAttribute([FromBody] UpdateAttributeCommand request)
        {
            var updateAttributeResult = await _mediator.Send(request);
            return Ok(updateAttributeResult);
        }

        [HttpPost("createAttributeValue")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<CreateAttributeValue>))]
        public async Task<IActionResult> CreateAttributeValue([FromBody] CreateAttributeValueCommand request)
        {
            var createAttributeValueResult = await _mediator.Send(request);
            return Ok(createAttributeValueResult);
        }

        [HttpGet("getAttributeValue")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<List<AttributeValueDto>>))]
        public async Task<IActionResult> GetAttributeValue(GetAttributeValueQuery request)
        {
            var getAttributeValueResult = await _mediator.Send(request);
            return Ok(getAttributeValueResult);
        }

        [HttpPut("updateAttributeValue")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<AttributeValueDto>))]
        public async Task<IActionResult> UpdateAttributeValue([FromBody] UpdateAttributeValueCommand request)
        {
            var updateAttributeValueResult = await _mediator.Send(request);
            return Ok(updateAttributeValueResult);
        }

        [HttpDelete("deleteAttributeValue")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<AttributeValueDto>))]
        public async Task<IActionResult> DeleteAttributeValue(DeleteAttributeValueCommand request)
        {
            var deleteAttributeValueResult = await _mediator.Send(request);
            return Ok(deleteAttributeValueResult);
        }
        [HttpGet("getAttributeValueId")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetAttributeValueId>))]
        public async Task<IActionResult> GetAttributeValueId([FromQuery] GetAttributeValueIdQuery request)
        {
            var getAttributeValueResult = await _mediator.Send(request);
            return Ok(getAttributeValueResult);
        }

        [HttpPost("getAttributeAndValueWithCategory")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetAttributeAndValueQueryResult>))]
        public async Task<IActionResult> GetAttributeAndValueWithCategory([FromBody] GetAttributeAndValueWithCategoryQuery request)
        {
            var res = await _mediator.Send(request);
            return Ok(res);
        }

        [HttpPost("getAttributeAndValues")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetAllAttributeNameWithValues>))]
        public async Task<IActionResult> GetAttributeAndValues([FromBody] GetAllAttributeNameWithValuesQuery request)
        {
            var res = await _mediator.Send(request);
            return Ok(res);
        }

        [HttpPost("bulkInsertAttributeAttributeValueAndMap")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<object>))]
        public async Task<IActionResult> bulkInsertAttributeAttributeValueAndMap(IFormFile file)
        {
            var request = new BulkInsertAttributeAttributeValueAndMapCommand { File = file };
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}