using Sma.Stm.EventBus.Abstractions;
using Sma.Stm.EventBus.Events;
using Sma.Stm.Services.GenericMessageService.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;
using Sma.Stm.Common;
using Sma.Stm.Common.Web;

namespace Sma.Stm.Services.GenericMessageService.IntegrationEvents.EventHandling
{
    public class NewSubscriptionIntegrationEventHandler : IIntegrationEventHandler<NewSubscriptionIntegrationEvent>
    {
        private readonly GenericMessageDbContext _dbContext;
        private readonly IEventBus _eventBus;

        public NewSubscriptionIntegrationEventHandler(IEventBus eventBus, 
            GenericMessageDbContext dbCOntext)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _dbContext = dbCOntext ?? throw new ArgumentNullException(nameof(dbCOntext));
        }

        public async Task Handle(NewSubscriptionIntegrationEvent @event)
        {
            var message = _dbContext.PublishedMessages.FirstOrDefault(x => x.DataId == @event.DataId);

            if (message != null)
            {
                var newEvent = new SendMessageIntegrationEven
                {
                    Body = message.Content,
                    ContentType = Constants.ContentTypeTextXml,
                    HttpMethod = "POST",
                    Url = new Uri(WebRequestHelper.CombineUrl(@event.CallbackEndpoint, "voyagePlans"))
                };

                _eventBus.Publish(newEvent);
            }

            await Task.FromResult(0);
        }
    }
}