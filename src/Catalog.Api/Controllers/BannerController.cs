using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Request.Command.BannerCommands;
using Catalog.ApiContract.Request.Query.BannerQueries;
using Catalog.ApiContract.Response.Command.BannerCommands;
using Catalog.ApiContract.Response.Query.BannerQueries;
using Catalog.Domain.Enums;
using Framework.Core.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Api.Controllers
{
    [Produces("application/json")]
    [Route("banner")]
    public class BannerController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BannerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("createBanner")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<BannerDto>))]
        public async Task<IActionResult> CreateBanner([FromBody] CreateBannerCommand request)
        {
            var createBanner = await _mediator.Send(request);
            return Ok(createBanner);
        }

        [HttpDelete("deleteBanner")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<object>))]
        public async Task<IActionResult> DeleteBanner(DeleteBannerCommand request)
        {
            var deleteBanner = await _mediator.Send(request);
            return Ok(deleteBanner);
        }

        [HttpDelete("deleteBannerLocation")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<object>))]
        public async Task<IActionResult> DeleteBannerLocation(DeleteBannerLocationCommand request)
        {
            var deleteBannerLocation = await _mediator.Send(request);
            return Ok(deleteBannerLocation);
        }

        [HttpDelete("deleteBannerLocationById")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<object>))]
        public async Task<IActionResult> DeleteBannerLocationById([FromBody] DeleteBannerLocationByIdCommand request)
        {
            var deleteBannerLocationById = await _mediator.Send(request);
            return Ok(deleteBannerLocationById);
        }

        [HttpGet("getBanners")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<List<GetBannerList>>))]
        public async Task<IActionResult> GetBannerList(GetBannerQuery request)
        {
            Enum.TryParse(HttpContext.Request.Headers["X-ChannelCode"], out ProductChannelCode productChannelCode);
            request.ProductChannelCode = productChannelCode;
            var getBanners = await _mediator.Send(request);

            return Ok(getBanners);
        }

        [HttpPost("createBannerLocation")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<CreateBannerLocationResult>))]
        public async Task<IActionResult> CreateBannerLocation([FromBody] CreateBannerLocationCommand request)
        {
            var createBannerLocation = await _mediator.Send(request);
            return Ok(createBannerLocation);
        }

        [HttpPost("getBannersByVipSeller")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<List<GetBannersByVipSeller>>))]
        public async Task<IActionResult> GetBannersByVipSeller([FromBody] GetBannersByVipSellerQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("updateBanner")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<object>))]
        public async Task<IActionResult> UpdateBanner([FromBody] UpdateBannerCommand request)
        {
            var updateBanner = await _mediator.Send(request);
            return Ok(updateBanner);
        }

        [HttpPost("updateBannerLocation")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<object>))]
        public async Task<IActionResult> UpdateBannerLocation([FromBody] UpdateBannerLocationCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
        [HttpGet("getBanner")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<List<GetBannerList>>))]
        public async Task<IActionResult> GetBanner(GetBannerForBOQuery request)
        {
            var getBanners = await _mediator.Send(request);
            return Ok(getBanners);
        }
        [HttpGet("getBannerDetail")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetBannerById>))]
        public async Task<IActionResult> GetBannerDetail(GetBannerByIdQuery request)
        {
            var getBannerDetail = await _mediator.Send(request);
            return Ok(getBannerDetail);
        }
        [HttpGet("getBannersType")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<List<BannerTypeList>>))]
        public async Task<IActionResult> GetBannersType(GetBannersTypeQuery request)
        {
            var getBannersType = await _mediator.Send(request);
            return Ok(getBannersType);
        }
        [HttpGet("getBannerActionType")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<List<GetBannerActionTypeList>>))]
        public async Task<IActionResult> GetBannerActionType(GetBannersActionTypeQuery request)
        {
            var getBannerActionTypes = await _mediator.Send(request);
            return Ok(getBannerActionTypes);
        }
        [HttpGet("getBannerFilterType")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<List<GetBannerFilterTypeList>>))]
        public async Task<IActionResult> GetBannerFilterType(GetBannerFilterTypeQuery request)
        {
            var getBannerActionTypes = await _mediator.Send(request);
            return Ok(getBannerActionTypes);
        }
        [HttpGet("getBannerLocations")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<GetBannerLocationListForBO>))]
        public async Task<IActionResult> GetBannerLocations(GetBannerLocationsQuery request)
        {
            var getBannerActionTypes = await _mediator.Send(request);
            return Ok(getBannerActionTypes);
        }

        [HttpGet("getBannersByChannel")]
        [ProducesResponseType(200, Type = typeof(ResponseBase<List<GetBannerChannelQueryResponse>>))]
        public async Task<IActionResult> GetBannersByChannel(GetBannerChannelQuery request)
        {
            Enum.TryParse(HttpContext.Request.Headers["X-ChannelCode"], out ProductChannelCode productChannelCode);
            request.ProductChannelCode = productChannelCode;
            var getBanners = await _mediator.Send(request);

            return Ok(getBanners);
        }
    }
}
