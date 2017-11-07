using Sma.Stm.EventBus.Abstractions;
using Sma.Stm.Services.SubscriptionService.IntegrationEvents.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sma.Stm.Services.SubscriptionService.IntegrationEvents.EventHandling
{
    public class AuthorizationRemovedIntegrationEventHandler : IIntegrationEventHandler<AuthorizationRemovedIntegrationEvent>
    {
        public AuthorizationRemovedIntegrationEventHandler()
        {

        }

        public async Task Handle(AuthorizationRemovedIntegrationEvent @event)
        {
            await Task.FromResult(0);
        }
    }
}