using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sma.Stm.Common.DocumentDb;
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

namespace Sma.Stm.Services.GenericMessageService.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PrivateController : MessageControllerBase
    {
        private readonly IEventBus _eventBus;
        private readonly DocumentDbRepository<PublishedMessage> _publishedMessageRepository;
        private readonly DocumentDbRepository<UploadedMessage> _uploadedMessageRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<PrivateController> _logger;

        public PrivateController(DocumentDbRepository<PublishedMessage> publishedMessageRepository,
            DocumentDbRepository<UploadedMessage> uploadedMessageRepository,
            IEventBus eventBus,
            IHostingEnvironment hostingEnvironment,
            ILogger<PrivateController> logger)
        {
            _publishedMessageRepository = publishedMessageRepository ?? throw new ArgumentNullException(nameof(publishedMessageRepository));
            _uploadedMessageRepository = uploadedMessageRepository ?? throw new ArgumentNullException(nameof(uploadedMessageRepository));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("UploadedMessage")]
        public async Task<IActionResult> GetUploaded()
        {
            try
            {
                var items = await _uploadedMessageRepository.GetItemsAsync(x => x != null);
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
                var items = await _publishedMessageRepository.GetItemsAsync(x => x != null);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("PublishedMessage/{id}")]
        public async Task<IActionResult> GetPublished(string id)
        {
            try
            {
                var item = await _publishedMessageRepository.GetItemAsync(id);
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

                var existing = await _publishedMessageRepository.GetItemAsync(message.Id);
                if (existing == null)
                {
                    await _publishedMessageRepository.CreateItemAsync(message);
                }
                else
                {
                    await _publishedMessageRepository.UpdateItemAsync(message.Id, message);
                }

                var @event = new MessagePublishedIntegrationEvent
                {
                    DataId = message.Id,
                    Content = message.Content.OuterXml
                };
                _eventBus.Publish(@event);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete("PublishedMessage/{id}")]
        public async Task<IActionResult> DeletePublished(string id)
        {
            try
            {
                await _publishedMessageRepository.DeleteItemAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}