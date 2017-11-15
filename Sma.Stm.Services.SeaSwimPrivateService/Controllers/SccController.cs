using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Sma.Stm.Ssc;
using Newtonsoft.Json;
using Sma.Stm.EventBus.Abstractions;

namespace Sma.Stm.Services.SeaSwimPrivateService.Controllers
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
        [SwaggerOperation("CallServiceUsingPOST")]
        [SwaggerResponse(200, type: typeof(CallServiceResponseObj))]
        public virtual IActionResult CallServiceUsingPOST([FromBody]CallServiceRequestObj request)
        {
            string exampleJson = null;

            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<CallServiceResponseObj>(exampleJson)
            : default(CallServiceResponseObj);
            return new ObjectResult(example);
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
        [SwaggerOperation("FindIdentities")]
        [SwaggerResponse(200, type: typeof(FindIdentitiesResponseObj))]
        public virtual IActionResult FindIdentitiesUsing()
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
        [SwaggerOperation("FindServices")]
        [SwaggerResponse(200, type: typeof(FindServicesResponseObj))]
        public virtual IActionResult FindServices([FromBody]FindServicesRequestObj request)
        {
            string exampleJson = null;

            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<FindServicesResponseObj>(exampleJson)
            : default(FindServicesResponseObj);
            return new ObjectResult(example);
        }
    }
}
