using Sma.Stm.EventBus.Abstractions;
using Sma.Stm.Services.SubscriptionService.IntegrationEvents.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sma.Stm.Services.SubscriptionService.IntegrationEvents.EventHandling
{
    public class MessagePublishedIntegrationEventHandler : IIntegrationEventHandler<MessagePublishedIntegrationEvent>
    {
        public MessagePublishedIntegrationEventHandler()
        {

        }

        public async Task Handle(MessagePublishedIntegrationEvent @event)
        {
            await Task.FromResult(0);
        }
    }
}