using Catalog.Domain;
using Framework.Core.Logging;
using Framework.Core.Model;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;

namespace Catalog.Api.Controllers
{
    [Route("error")]
    public class ErrorController : ControllerBase
    {
        private readonly IAppLogger _appLogger;

        public ErrorController(IAppLogger appLogger)
        {
            _appLogger = appLogger;
        }

        [HttpPost]
        [HttpGet]
        [HttpPut]
        public IActionResult Index()
        {
            try
            {
                var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
                if (exceptionFeature != null)
                {
                    var exception = exceptionFeature.Error;
                    _appLogger.Exception(exception, MethodBase.GetCurrentMethod());
                }

                var responseObject = CreateResponse();

                return BadRequest(responseObject);
            }
            catch (Exception e)
            {
                _appLogger.Exception(e, MethodBase.GetCurrentMethod());
                var responseObject = CreateResponse();
                return Ok(responseObject);
            }
        }

        private static ResponseBase CreateResponse()
        {
            var responseObject = new ResponseBase
            {
                Success = false,
                MessageCode = ApplicationMessage.InvalidParameter,
                Message = ApplicationMessage.InvalidParameter.Message(),
                UserMessage = ApplicationMessage.InvalidParameter.UserMessage(),
            };
            return responseObject;
        }
    }
}