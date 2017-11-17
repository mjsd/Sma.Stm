using Sma.Stm.EventBus.Abstractions;
using Sma.Stm.EventBus.Events;
using Sma.Stm.Services.SubscriptionService.DataAccess;
using Sma.Stm.Services.SubscriptionService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Sma.Stm.Services.SubscriptionService.IntegrationEvents.EventHandling
{
    public class MessagePublishedIntegrationEventHandler : IIntegrationEventHandler<MessagePublishedIntegrationEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly SubscriptionDbContext _dbContext;

        public MessagePublishedIntegrationEventHandler(IEventBus eventBus,
            SubscriptionDbContext dbContext)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task Handle(MessagePublishedIntegrationEvent @event)
        {
            try
            {
                var subscribers = await _dbContext.Subscriptions.Where(x => x.DataId == @event.DataId).ToListAsync();
                foreach (var subscriber in subscribers)
                {
                    var newEvent = new SendMessageIntegrationEven
                    {
                        Body = @event.Content,
                        ContentType = "text/xml",
                        Url = new Uri(subscriber.CallbackEndpoint)
                    };

                    _eventBus.Publish(newEvent);
                }
            }
            catch (Exception)
            {

            }
        }
    }
}