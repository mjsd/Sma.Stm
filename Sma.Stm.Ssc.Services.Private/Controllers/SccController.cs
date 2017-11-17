using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Sma.Stm.Ssc;
using Newtonsoft.Json;
using Sma.Stm.EventBus.Abstractions;
using Sma.Stm.Common.Swagger;

namespace Sma.Stm.Ssc.Services.Private.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SccController : Controller
    {
        private readonly IEventBus _eventBus;
        private readonly SeaSwimConnectorPrivateService _seaSwimConnectorPrivateService;

        public SccController(SeaSwimConnectorPrivateService seaSwimConnectorPrivateService,
             IEventBus eventBus)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _seaSwimConnectorPrivateService = seaSwimConnectorPrivateService ?? throw new ArgumentNullException(nameof(seaSwimConnectorPrivateService));
        }

        /// <summary>
        /// callService
        /// </summary>
        /// <remarks>Facilitate the communication between services. The function will call a generic web service part of the stm infrastructure, checking the certificates authentications.</remarks>
        /// <param name="request">Contain the data needed by the callService function to execute the request.</param>
        /// <response code="200">Response container with response from called service</response>
        /// <response code="201">Created</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Unexpected error</response>
        [HttpPost]
        [Route("callService")]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [SwaggerRequestContentType(requestType: "application/json", Exclusive = true)]
        [SwaggerResponse(200, type: typeof(CallServiceResponseObj))]
        public virtual IActionResult CallService([FromBody]CallServiceRequestObj request)
        {
            try
            {
                var response = _seaSwimConnectorPrivateService.CallService(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// findIdentities
        /// </summary>
        /// <remarks>Facilitate the communication with the identity service in order to discover the organizations part of the STM infrastructure.</remarks>
        /// <param name="request">request</param>
        /// <response code="200">findIdentities response</response>
        /// <response code="201">Created</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Unexpected error</response>
        [HttpGet]
        [Route("findIdentities")]
        [SwaggerResponse(200, type: typeof(FindIdentitiesResponseObj))]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        public virtual IActionResult FindIdentities()
        {
            try
            {
                var response = _seaSwimConnectorPrivateService.FindIdentities();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// findServices
        /// </summary>
        /// <remarks>Facilitate the communication with the service registry, in order to discover the services of the STM infrastructure.</remarks>
        /// <param name="request">request</param>
        /// <response code="200">findServices response</response>
        /// <response code="201">Created</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Unexpected error</response>
        [HttpPost]
        [Route("findServices")]
        [SwaggerResponse(200, type: typeof(FindServicesResponseObj))]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [SwaggerRequestContentType(requestType: "application/json", Exclusive = true)]
        public virtual IActionResult FindServices([FromBody]FindServicesRequestObj request)
        {
            try
            {
                var response = _seaSwimConnectorPrivateService.FindServices(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
