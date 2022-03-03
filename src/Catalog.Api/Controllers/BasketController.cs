using Catalog.ApiContract.Request.Command.BasketCommands;
using Catalog.ApiContract.Request.Query.BasketQueries;
using Catalog.ApiContract.Response.Query.BasketQueries;
using Framework.Core.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Api.Controllers
{
    [Produces("application/json")]
    [Route("basket")]
    [ApiController]
    public class BasketController : ControllerBase
    {

        private readonly IMediator _mediator;

        public BasketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("items")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<List<BasketDetail>>))]
        public async Task<IActionResult> Items(GetBasketItemsQuery request)
        {
            var basketItemsQueryResult = await _mediator.Send(request);
            return Ok(basketItemsQueryResult);
        }

        [HttpPost("stocks")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<List<StockDetail>>))]
        public async Task<IActionResult> Stocks(GetBasketItemsStocksQuery request)
        {
            var stockItemsQueryResult = await _mediator.Send(request);
            return Ok(stockItemsQueryResult);
        }

        [HttpPost("removeStocks")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<object>))]
        public async Task<IActionResult> RemoveStocks(RemoveStockCommand request)
        {
            var stockItemsRemoveQueryResult = await _mediator.Send(request);
            return Ok(stockItemsRemoveQueryResult);
        }

        [HttpPost("installments")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<List<BasketDetailInstallment>>))]
        public async Task<IActionResult> Installments(GetBasketItemsInstallmentCountQuery request)
        {
            var installmentsQueryResult = await _mediator.Send(request);
            return Ok(installmentsQueryResult);
        }

    }
}
