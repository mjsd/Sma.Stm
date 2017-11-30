using System.Collections.Generic;
using System.Threading.Tasks;
using Sma.Stm.EventBus.Abstractions;
using Sma.Stm.EventBus.Events;
using Sma.Stm.Services.MessageSenderService.Services;
using Sma.Stm.Ssc.Contract;

namespace Sma.Stm.Services.MessageSenderService.IntegrationEvents.EventHandling
{
    public class SendMessageIntegrationEvenHandler : IIntegrationEventHandler<SendMessageIntegrationEven>
    {
        public SendMessageIntegrationEvenHandler()
        {
        }

        public async Task Handle(SendMessageIntegrationEven @event)
        {
            List<Header> headers = null;
            if (!string.IsNullOrEmpty(@event.ContentType))
            {
                headers = new List<Header>
                {
                    new Header { Key = "Content-type", Value = @event.ContentType}
                };
            }

            SeaSwimService.CallService(@event.Body, @event.Url.ToString(), @event.HttpMethod, headers);
            await Task.FromResult(0);
        }
    }
}