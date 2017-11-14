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
            ILogger<PrivateController> logger)
        {
            _dbContext = dbCOntext ?? throw new ArgumentNullException(nameof(dbCOntext));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("UploadedMessage")]
        public async Task<IActionResult> GetUploaded()
        {
            try
            {
                var items = await _dbContext.UploadedMessages.ToListAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("PublishedMessage")]
        public async Task<IActionResult> GetPublished()
        {
            _logger.LogDebug("Get published messages");

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

        [HttpGet("PublishedMessage/{id}")]
        public async Task<IActionResult> GetPublished(string dataId)
        {
            try
            {
                var item = await _dbContext.UploadedMessages.FirstOrDefaultAsync(x => x.DataId == dataId);
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("PublishedMessage")]
        public async Task<IActionResult> PostPublished([FromBody]PublishedMessage message)
        {
            try
            {
                // Validate(message.Content, _hostingEnvironment);

                var existing = await _dbContext.PublishedMessages.FirstOrDefaultAsync(x => x.DataId == message.DataId);
                if (existing == null)
                {
                    await _dbContext.AddAsync(message);
                }
                else
                {
                    existing.Content = message.Content;
                    existing.PublishTime = message.PublishTime;
                    _dbContext.Update(existing);
                }

                _dbContext.SaveChanges();

                var @event = new MessagePublishedIntegrationEvent
                {
                    DataId = message.DataId,
                    Content = message.Content
                };
                _eventBus.Publish(@event);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete("PublishedMessage/{dataId}")]
        public async Task<IActionResult> DeletePublished(string dataId)
        {
            try
            {
                var existing = await _dbContext.PublishedMessages.FirstOrDefaultAsync(x => x.DataId == dataId);
                if (existing != null)
                {
                    _dbContext.PublishedMessages.Remove(existing);
                }

                _dbContext.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}