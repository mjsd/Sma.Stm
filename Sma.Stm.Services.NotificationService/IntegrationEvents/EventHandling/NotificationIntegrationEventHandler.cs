using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sma.Stm.Common;
using Sma.Stm.Common.Web;
using Sma.Stm.EventBus.Abstractions;
using Sma.Stm.EventBus.Events;
using Sma.Stm.Services.AuthorizationService.DataAccess;
using Sma.Stm.Services.NotificationService.Models;

namespace Sma.Stm.Services.NotificationService.IntegrationEvents.EventHandling
{
    public class NotificationIntegrationEventHandler : IIntegrationEventHandler<NotificationIntegrationEvent>
    {
        private readonly NotificationDbContext _dbContext;
        private readonly IEventBus _eventBus;

        public NotificationIntegrationEventHandler(IEventBus eventBus,
            NotificationDbContext dbCOntext)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _dbContext = dbCOntext ?? throw new ArgumentNullException(nameof(dbCOntext));
        }

        public async Task Handle(NotificationIntegrationEvent @event)
        {
            var notification = new Notification
            {
                Body = @event.Body,
                FromOrgId = @event.FromOrgId,
                FromOrgName = @event.FromOrgName,
                FromServiceId = @event.FromServiceId,
                NotificationCreatedAt = @event.NotificationCreatedAt,
                NotificationSource = (NotificationService.Models.EnumNotificationSource)@event.NotificationSource,
                NotificationType = (NotificationService.Models.EnumNotificationType)@event.NotificationType,
                ReceivedAt = @event.ReceivedAt,
                Subject = @event.Subject,
                Fetched = false
            };

            var headers = new WebHeaderCollection
            {
                { HttpRequestHeader.ContentType, Constants.ContentTypeApplicationJson }
            };

            var subscribers = await _dbContext.Subscribers.ToListAsync();
            if (subscribers != null)
            {
                foreach (var subscriber in subscribers)
                {
                    var response = WebRequestHelper.Post(subscriber.NotificationEndpointUrl, JsonConvert.SerializeObject(notification), headers);
                    if (response.HttpStatusCode == HttpStatusCode.OK)
                    {
                        notification.Fetched = true;
                    }
                }
            }

            await _dbContext.Notifications.AddAsync(notification);
            await _dbContext.SaveChangesAsync();
        }
    }
}