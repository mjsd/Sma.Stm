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
using Sma.Stm.EventBus.Events;
using Sma.Stm.Services.GenericMessageService.DataAccess;
using Microsoft.EntityFrameworkCore;
using Sma.Stm.Common.Swagger;
using Newtonsoft.Json;
using Sma.Stm.Common;
using Microsoft.Extensions.Configuration;

namespace Sma.Stm.Services.GenericMessageService.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PrivateController : MessageControllerBase
    {
        private readonly IEventBus _eventBus;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<PrivateController> _logger;
        private readonly GenericMessageDbContext _dbContext;

        public PrivateController(GenericMessageDbContext dbCOntext,
            IEventBus eventBus,
            IHostingEnvironment hostingEnvironment,
            IConfiguration configuration,
            ILogger<PrivateController> logger)
        {
            _dbContext = dbCOntext ?? throw new ArgumentNullException(nameof(dbCOntext));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpGet("uploadedMessage")]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        public async Task<IActionResult> GetUploaded([FromQuery] int? limitQuery = null)
        {
            try
            {
                var result = new MessageEnvelope
                {
                    Messages = new List<Message>(),
                    NumberOfMessages = 0,
                    RemainingNumberOfMessages = 0
                };

                var items = new List<UploadedMessage>();
                if (limitQuery == null)
                {
                    items = await _dbContext.UploadedMessages
                        .Where(x => !x.Fetched).OrderBy(x => x.ReceiveTime).ToListAsync();
                }
                else
                {
                    items = await _dbContext.UploadedMessages
                        .Where(x => !x.Fetched).OrderBy(x => x.ReceiveTime).Take(limitQuery.Value).ToListAsync();
                }

                foreach(var item in items)
                {
                    result.Messages.Add(new Message
                    {
                        Id = item.DataId,
                        FromOrgId = item.FromOrgId,
                        FromServiceId = item.FromServiceId,
                        MessageType = "RTZ",
                        CallbackEndpoint = "http://test.se",
                        FromOrgName = "Missing",
                        ReceivedAt = item.ReceiveTime,
                        StmMessage = new StmMessage
                        {
                            Message = item.Content,
                        },
                    });

                    item.Fetched = true;
                    _dbContext.UploadedMessages.Update(item);

                    if (item.SendAcknowledgement)
                    {
                        var ack = new DeliveryAck
                        {
                            AckResult = "Ok",
                            FromId = "",
                            FromName = "",
                            Id = Guid.NewGuid().ToString(),
                            ReferenceId = item.DataId,
                            TimeOfDelivery = DateTime.UtcNow,
                            ToId = "",
                            ToName = ""
                        };

                        var newEvent = new SendMessageIntegrationEven
                        {
                            Body = JsonConvert.SerializeObject(ack),
                            ContentType = Constants.CONTENT_TYPE_APPLICATION_JSON,
                            Url = new Uri("https://161.54.241.174:8080/acknowledgement"),
                            HttpMethod = "POST",
                            SenderOrgId = "",
                            SenderServiceId = "",
                            TargetServiceId = item.FromServiceId
                        };

                        _eventBus.Publish(newEvent);
                    }
                }

                _dbContext.SaveChanges();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("publishedMessage")]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        public async Task<IActionResult> GetPublished()
        {
            try
            {
                var items = await _dbContext.PublishedMessages.ToListAsync();

                var response = new List<PublishedMessageContract>();
                foreach (var item in items)
                {
                    response.Add(new PublishedMessageContract
                    {
                        Message = item.Content,
                        MessageID = item.DataId,
                        MessageLastUpdateTime = DateTime.UtcNow,
                        MessageStatus = 7,
                        MessageType = "RTZ",
                        PublishTime = item.PublishTime
                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("publishedMessage")]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [SwaggerRequestContentType(requestType: "text/xml", Exclusive = true)]
        public async Task<IActionResult> PublishMessage([FromQuery]string dataId,
            [FromQuery]string messageType,
            [FromBody]string message)
        {
            try
            {
                Validate(message, _hostingEnvironment);

                var existing = await _dbContext.PublishedMessages.FirstOrDefaultAsync(x => x.DataId == dataId);
                if (existing == null)
                {
                    await _dbContext.PublishedMessages.AddAsync(new PublishedMessage
                    {
                        Content = message,
                        DataId = dataId,
                        Status = GetStatus(message),
                        PublishTime = DateTime.UtcNow
                    });
                }
                else
                {
                    existing.Content = message;
                    existing.PublishTime = DateTime.UtcNow;
                    _dbContext.Update(existing);
                }

                _dbContext.SaveChanges();

                var @event = new MessagePublishedIntegrationEvent
                {
                    DataId = dataId,
                    Content = message
                };
                _eventBus.Publish(@event);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete("publishedMessage")]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [SwaggerRequestContentType(requestType: "application/json", Exclusive = true)]
        public async Task<IActionResult> DeletePublished([FromQuery]string dataId)
        {
            try
            {
                var existing = await _dbContext.PublishedMessages.FirstOrDefaultAsync(x => x.DataId == dataId);
                if (existing != null)
                {
                    _dbContext.PublishedMessages.Remove(existing);
                }

                _dbContext.SaveChanges();

                var @event = new MessageDeletedIntegrationEvent
                {
                    DataId = dataId
                };
                _eventBus.Publish(@event);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}