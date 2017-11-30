using Sma.Stm.EventBus.Abstractions;
using Sma.Stm.EventBus.Events;
using Sma.Stm.Services.SubscriptionService.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sma.Stm.Common;
using Sma.Stm.Common.Web;

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
                        ContentType = Constants.ContentTypeTextXml,
                        HttpMethod = "POST",
                        Url = new Uri(WebRequestHelper.CombineUrl(subscriber.CallbackEndpoint, "/voyagePlans"))
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