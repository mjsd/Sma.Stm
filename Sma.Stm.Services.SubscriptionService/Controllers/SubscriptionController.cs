using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sma.Stm.EventBus.Abstractions;
using Sma.Stm.EventBus.Events;

namespace Sma.Stm.Services.SubscriptionService.Controllers
{
    [Route("/api/[controller]")]
    public class SubscriptionController : Controller
    {
        private readonly IEventBus _eventBus;

        public SubscriptionController(IEventBus eventBus)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            var @event = new NewSubscriptionIntegrationEvent();
            _eventBus.Publish(@event);

            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
