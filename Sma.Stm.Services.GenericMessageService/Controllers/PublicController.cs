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
        private readonly SeaSwimInstanceContextService _seaSwimInstanceContextService;
        private readonly GenericMessageDbContext _dbContext;
        private readonly SeaSwimIdentityService _seaSwimIdentityService;

        public PublicController(IEventBus eventBus,
            IHostingEnvironment hostingEnvironment,
            ILogger<PublicController> logger,
            IConfiguration configuration,
            SeaSwimInstanceContextService seaSwimInstanceContextService,
            GenericMessageDbContext dbCOntext,
            SeaSwimIdentityService seaSwimIdentityService)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _seaSwimInstanceContextService = seaSwimInstanceContextService ?? throw new ArgumentNullException(nameof(seaSwimInstanceContextService));
            _dbContext = dbCOntext ?? throw new ArgumentNullException(nameof(dbCOntext));
            _seaSwimIdentityService = seaSwimIdentityService ?? throw new ArgumentNullException(nameof(seaSwimIdentityService));
        }

        [HttpGet("message")]
        public async Task<IActionResult> Get([FromQuery]string dataId, [FromQuery]string status)
        {
            try
            {
                var query = _dbContext.PublishedMessages.Where(x => x.Id > 0);
                if (!string.IsNullOrEmpty(dataId))
                {
                    query = query.Where(x => x.DataId == dataId);
                }

                if (!string.IsNullOrEmpty(status))
                {
                    query = query.Where(x => x.Status.ToLower() == status.ToLower());
                }

                var items = await query.ToListAsync();

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
                else if (items.Count() == 0)
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("message/{dataId}")]
        public async Task<IActionResult> Get([FromQuery]string dataId)
        {
            try
            {
                if (!AuthorizationService.CheckAuthentication(_seaSwimInstanceContextService.CallerOrgId, dataId))
                {
                    return Unauthorized();
                }

                var item = await _dbContext.PublishedMessages.FirstOrDefaultAsync(x => x.DataId == dataId);

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

                var uploadedMessage = new UploadedMessage
                {
                    Content = message,
                    DataId = GetDataId(message),
                    FromOrgId = _seaSwimInstanceContextService.CallerOrgId,
                    FromServiceId = _seaSwimInstanceContextService.CallerServiceId,
                    ReceiveTime = DateTime.UtcNow,
                    SendAcknowledgement = deliveryAckEndPoint == null ? false : true,
                    Fetched = false
                };

                await _dbContext.UploadedMessages.AddAsync(uploadedMessage);

                await _dbContext.SaveChangesAsync();

                var @event = new MessageUploadedIntegrationEvent
                {
                };

                _eventBus.Publish(@event);

                var orgName = _seaSwimIdentityService.GetIdentityName(_seaSwimInstanceContextService.CallerOrgId);

                var @notificationEvent = new NotificationIntegrationEvent
                {
                    FromOrgId = _seaSwimInstanceContextService.CallerOrgId,
                    FromOrgName = orgName,
                    FromServiceId = _seaSwimInstanceContextService.CallerServiceId,
                    NotificationCreatedAt = DateTime.UtcNow,
                    NotificationType = EnumNotificationType.MESSAGE_WAITING,
                    Subject = "New voyageplan uploaded.",
                    NotificationSource = EnumNotificationSource.VIS,
                };
                _eventBus.Publish(@notificationEvent);

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
            var orgName = _seaSwimIdentityService.GetIdentityName(_seaSwimInstanceContextService.CallerOrgId);

            var @event = new NotificationIntegrationEvent
            {
                FromOrgId = _seaSwimInstanceContextService.CallerOrgId,
                FromOrgName = orgName,
                FromServiceId = _seaSwimInstanceContextService.CallerServiceId,
                NotificationCreatedAt = DateTime.UtcNow,
                NotificationType = EnumNotificationType.ACKNOWLEDGEMENT_RECEIVED,
                Subject = "Acknowledgement",
                NotificationSource = EnumNotificationSource.VIS,
                Body = string.Format("Acknowledgement of message delivery for message with id {0} recieved from {1}, {2}", deliveryAck.ReferenceId, deliveryAck.FromName, deliveryAck.FromId)
            };
            _eventBus.Publish(@event);

            return Ok();
        }
    }
}