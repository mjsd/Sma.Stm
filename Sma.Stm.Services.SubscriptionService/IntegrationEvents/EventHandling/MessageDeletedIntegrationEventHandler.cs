using Sma.Stm.EventBus.Abstractions;
using Sma.Stm.EventBus.Events;
using Sma.Stm.Services.SubscriptionService.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Sma.Stm.Services.SubscriptionService.IntegrationEvents.EventHandling
{
    public class MessageDeletedIntegrationEventHandler : IIntegrationEventHandler<MessageDeletedIntegrationEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly SubscriptionDbContext _dbContext;

        public MessageDeletedIntegrationEventHandler(IEventBus eventBus,
            SubscriptionDbContext dbContext)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task Handle(MessageDeletedIntegrationEvent @event)
        {
            try
            {
                var subscribers = await _dbContext.Subscriptions.Where(x => x.DataId == @event.DataId).ToListAsync();
                _dbContext.Subscriptions.RemoveRange(subscribers);

                _dbContext.SaveChanges();
            }
            catch (Exception)
            {

            }
        }
    }
}