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
using Sma.Stm.Services.GenericMessageService.IntegrationEvents.Events;
using System.Xml;
using System.Text;
using Microsoft.AspNetCore.Hosting;

namespace Sma.Stm.Services.GenericMessageService.Controllers
{
    [Route("api/[controller]")]
    public class UploadedMessageController : MessageControllerBase
    {
        private readonly IEventBus _eventBus;
        private readonly DocumentDbRepository<UploadedMessage> _uploadedMessageRepository;
        private readonly IHostingEnvironment _hostingEnvironment;

        public UploadedMessageController(DocumentDbRepository<UploadedMessage> upladedMessageRepository,
            IEventBus eventBus,
            IHostingEnvironment hostingEnvironment)
        {
            _uploadedMessageRepository = upladedMessageRepository ?? throw new ArgumentNullException(nameof(upladedMessageRepository));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var item = await _uploadedMessageRepository.GetItemAsync(id);
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]UploadedMessage message)
        {
            try
            {
                Validate(message.Content, _hostingEnvironment);
                await _uploadedMessageRepository.CreateItemAsync(message);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            { 
                await _uploadedMessageRepository.DeleteItemAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}