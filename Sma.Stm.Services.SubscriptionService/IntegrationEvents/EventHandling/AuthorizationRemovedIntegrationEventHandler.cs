using Sma.Stm.EventBus.Abstractions;
using Sma.Stm.EventBus.Events;
using Sma.Stm.Services.SubscriptionService.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Sma.Stm.Services.SubscriptionService.IntegrationEvents.EventHandling
{
    public class AuthorizationRemovedIntegrationEventHandler : IIntegrationEventHandler<AuthorizationRemovedIntegrationEvent>
    {
        private readonly SubscriptionDbContext _dbContext;

        public AuthorizationRemovedIntegrationEventHandler(SubscriptionDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task Handle(AuthorizationRemovedIntegrationEvent @event)
        {
            var items = await _dbContext.Subscriptions.Where(x => x.OrgId == @event.OrgId
                && x.DataId == @event.DataId).ToListAsync();

            foreach (var item in items)
            {
                _dbContext.Remove(item);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}