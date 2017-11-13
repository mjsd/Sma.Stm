using Sma.Stm.EventBus.Abstractions;
using Sma.Stm.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sma.Stm.Services.GenericMessageService.IntegrationEvents.EventHandling
{
    public class NewSubscriptionIntegrationEventHandler : IIntegrationEventHandler<NewSubscriptionIntegrationEvent>
    {
        public NewSubscriptionIntegrationEventHandler()
        {

        }

        public async Task Handle(NewSubscriptionIntegrationEvent @event)
        {
            await Task.FromResult(0);
        }
    }
}