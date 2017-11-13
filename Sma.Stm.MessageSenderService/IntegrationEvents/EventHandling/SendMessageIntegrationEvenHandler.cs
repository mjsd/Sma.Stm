using Sma.Stm.Common.Web;
using Sma.Stm.EventBus.Abstractions;
using Sma.Stm.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Sma.Stm.Services.GenericMessageService.IntegrationEvents.EventHandling
{
    public class SendMessageIntegrationEvenHandler : IIntegrationEventHandler<SendMessageIntegrationEven>
    {
        public SendMessageIntegrationEvenHandler()
        {

        }

        public async Task Handle(SendMessageIntegrationEven @event)
        {
            var headers = new WebHeaderCollection();
            if (!string.IsNullOrEmpty(@event.ContentType))
            {
                headers.Add("Content-type", @event.ContentType);
            }

            var result = WebRequestHelper.Post(@event.Url.ToString(), @event.Body, headers);
            await Task.FromResult(0);
        }
    }
}