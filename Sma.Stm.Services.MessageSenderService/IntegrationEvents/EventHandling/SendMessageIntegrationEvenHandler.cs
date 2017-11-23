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
            List<Ssc.Header> headers = null;
            if (!string.IsNullOrEmpty(@event.ContentType))
            {
                headers = new List<Ssc.Header>
                {
                    new Ssc.Header { Key = "Content-type", Value = @event.ContentType}
                };
            }

            Services.SeaSwimService.CallService(@event.Body, @event.Url.ToString(), @event.HttpMethod, headers);
            await Task.FromResult(0);
        }
    }
}