using Catalog.ApiContract.Request.Query.SearchQueries;
using Catalog.ApiContract.Response.Query.SearchQueries;
using Framework.Core.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Catalog.Api.Controllers
{
    [Produces("application/json")]
    [Route("search")]
    [ApiController]
    public class SearchController : ControllerBase
    {

        private readonly IMediator _mediator;

        public SearchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{query}")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(ResponseBase<DidYouMeanDetail>))]
        public async Task<IActionResult> DidYouMean(string query)
        {
            if (query.Length < 3)
            {
                return BadRequest();
            }
            var searchResult = await _mediator.Send(new DidYouMeanQuery() { Message = query });
            return Ok(searchResult);
        }
        [HttpGet("getSeoSearchValue")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetSeoSearchValue>))]
        public async Task<IActionResult> GetSeoSearchValue([FromQuery] GetSeoSearchValueQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}
