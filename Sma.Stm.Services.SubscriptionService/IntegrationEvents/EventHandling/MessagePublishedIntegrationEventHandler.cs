using Sma.Stm.Common.DocumentDb;
using Sma.Stm.EventBus.Abstractions;
using Sma.Stm.EventBus.Events;
using Sma.Stm.Services.SubscriptionService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sma.Stm.Services.SubscriptionService.IntegrationEvents.EventHandling
{
    public class MessagePublishedIntegrationEventHandler : IIntegrationEventHandler<MessagePublishedIntegrationEvent>
    {
        private readonly DocumentDbRepository<SubscriptionList> _subscriptionRepository;
        private readonly IEventBus _eventBus;

        public MessagePublishedIntegrationEventHandler(DocumentDbRepository<SubscriptionList> subscriptionRepository,
            IEventBus eventBus)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _subscriptionRepository = subscriptionRepository ?? throw new ArgumentNullException(nameof(subscriptionRepository));
        }

        public async Task Handle(MessagePublishedIntegrationEvent @event)
        {
            try
            {
                //var subscribers = await _subscriptionRepository.GetItemsAsync(x => x != null);
                //foreach (var subscriber in subscribers)
                //{
                    var newEvent = new SendMessageIntegrationEven
                    {
                        Body = @event.Content,
                        ContentType = "text/xml",
                        Url = new Uri("http://mjsd.se")
                    };

                    _eventBus.Publish(newEvent);
                //}
            }
            catch (Exception ex)
            {

            }

            await Task.FromResult(0);
        }
    }
}