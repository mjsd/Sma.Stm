using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sma.Stm.Services.GenericMessageService.Models;
using System.Net;
using Sma.Stm.Common.Xml;
using System.Xml.Schema;
using System.IO;
using Sma.Stm.EventBus.Abstractions;
using Sma.Stm.Services.GenericMessageService.IntegrationEvents.EventHandling;
using System.Xml;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Sma.Stm.Services.GenericMessageService.Services;
using Sma.Stm.Ssc;
using Sma.Stm.EventBus.Events;
using Microsoft.EntityFrameworkCore;
using Sma.Stm.Services.GenericMessageService.DataAccess;
using Sma.Stm.Common.Swagger;
using Microsoft.Extensions.Configuration;

namespace Sma.Stm.Services.GenericMessageService.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PublicController : MessageControllerBase
    {
        private readonly IEventBus _eventBus;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<PublicController> _logger;
        private readonly IConfiguration _configuration;
        private readonly SeaSwimInstanceContextService _seaSwimInstanceContextService;
        private readonly GenericMessageDbContext _dbCOntext;

        public PublicController(IEventBus eventBus,
            IHostingEnvironment hostingEnvironment,
            ILogger<PublicController> logger,
            IConfiguration configuration,
            SeaSwimInstanceContextService seaSwimInstanceContextService,
            GenericMessageDbContext dbCOntext)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _seaSwimInstanceContextService = seaSwimInstanceContextService ?? throw new ArgumentNullException(nameof(seaSwimInstanceContextService));
            _dbCOntext = dbCOntext ?? throw new ArgumentNullException(nameof(dbCOntext));
        }

        [HttpGet("message")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var items = await _dbCOntext.PublishedMessages.ToListAsync();
                var response = new GetVoyagePlanResponse
                {
                    LastInteractionTime = DateTime.UtcNow,
                    VoyagePlans = new List<VoyagePlan>()
                };

                foreach (var item in items)
                {
                    if (AuthorizationService.CheckAuthentication(_seaSwimInstanceContextService.CallerOrgId, item.DataId))
                    {
                        response.VoyagePlans.Add(new VoyagePlan
                        {
                            Route = item.Content
                        });
                    }
                }

                if (items.Count() > 0 && response.VoyagePlans.Count() == 0)
                    return Unauthorized();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("message/{dataId}")]
        public async Task<IActionResult> Get(string dataId)
        {
            try
            {
                if (!AuthorizationService.CheckAuthentication(_seaSwimInstanceContextService.CallerOrgId, dataId))
                {
                    return Unauthorized();
                }

                var item = await _dbCOntext.PublishedMessages.FirstOrDefaultAsync(x => x.DataId == dataId);

                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("message")]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [SwaggerRequestContentType(requestType: "text/xml", Exclusive = true)]
        public async Task<IActionResult> Post([FromBody]string message, [FromQuery]string deliveryAckEndPoint = null)
        {
            try
            {
                Validate(message, _hostingEnvironment);

                var dataIdXPath = _configuration.GetValue<string>("DataIdXPath");
                var parser = new XmlParser(message);
                parser.SetNamespaces(_configuration.GetValue<string>("Namespaces"));

                var uploadedMessage = new UploadedMessage
                {
                    Content = message,
                    DataId = parser.GetValue(dataIdXPath),
                    FromOrgId = _seaSwimInstanceContextService.CallerOrgId,
                    FromServiceId = _seaSwimInstanceContextService.CallerServiceId,
                    ReceiveTime = DateTime.UtcNow,
                    SendAcknowledgement = deliveryAckEndPoint == null ? false : true,
                    Fetched = false
                };

                await _dbCOntext.UploadedMessages.AddAsync(uploadedMessage);

                await _dbCOntext.SaveChangesAsync();

                var @event = new MessageUploadedIntegrationEvent
                {
                };
                _eventBus.Publish(@event);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("message/acknowledgement")]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [SwaggerRequestContentType(requestType: "application/json", Exclusive = true)]
        public async virtual Task<IActionResult> Acknowledgement([FromBody]DeliveryAck deliveryAck)
        {
            return Ok();
        }
    }
}