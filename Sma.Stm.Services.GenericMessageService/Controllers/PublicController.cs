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
using Sma.Stm.Services.GenericMessageService.Services;
using Sma.Stm.Ssc;
using Sma.Stm.EventBus.Events;

namespace Sma.Stm.Services.GenericMessageService.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PublicController : MessageControllerBase
    {
        private readonly IEventBus _eventBus;
        private readonly DocumentDbRepository<PublishedMessage> _publishedMessageRepository;
        private readonly DocumentDbRepository<UploadedMessage> _uploadedMessageRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<PublicController> _logger;
        private readonly SeaSwimInstanceContextService _seaSwimInstanceContextService;

        public PublicController(DocumentDbRepository<PublishedMessage> publishedMessageRepository,
            DocumentDbRepository<UploadedMessage> uploadedMessageRepository,
            IEventBus eventBus,
            IHostingEnvironment hostingEnvironment,
            ILogger<PublicController> logger,
            SeaSwimInstanceContextService seaSwimInstanceContextService)
        {
            _publishedMessageRepository = publishedMessageRepository ?? throw new ArgumentNullException(nameof(publishedMessageRepository));
            _uploadedMessageRepository = uploadedMessageRepository ?? throw new ArgumentNullException(nameof(uploadedMessageRepository));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _seaSwimInstanceContextService = seaSwimInstanceContextService ?? throw new ArgumentNullException(nameof(seaSwimInstanceContextService));
        }

        [HttpGet("Message")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var items = await _publishedMessageRepository.GetItemsAsync(x => x != null);
                var response = new List<PublishedMessage>();

                foreach (var item in items)
                {
                    if (AuthorizationService.CheckAuthentication(_seaSwimInstanceContextService.CallerOrgId, item.Id))
                    {
                        response.Add(item);
                    }
                }

                if (items.Count() > 0 && response.Count() == 0)
                    return Unauthorized();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("Message/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                if (!AuthorizationService.CheckAuthentication(_seaSwimInstanceContextService.CallerOrgId, id))
                {
                    return Unauthorized();
                }

                var item = await _publishedMessageRepository.GetItemAsync(id);

                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("Message")]
        public async Task<IActionResult> Post([FromBody]UploadedMessage message)
        {
            try
            {
                // Validate(message.Content, _hostingEnvironment);

                await _uploadedMessageRepository.CreateItemAsync(message);

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
    }
}