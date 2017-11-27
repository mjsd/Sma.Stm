﻿using Sma.Stm.EventBus.Abstractions;
using Sma.Stm.EventBus.Events;
using Sma.Stm.Services.AuthorizationService.DataAccess;
using Sma.Stm.Services.AuthorizationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sma.Stm.Common;
using Sma.Stm.Common.Web;

namespace Sma.Stm.Services.AuthorizationService.IntegrationEvents.EventHandling
{
    public class MessageDeletedIntegrationEventHandler : IIntegrationEventHandler<MessageDeletedIntegrationEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly AuthorizationDbContext _dbContext;

        public MessageDeletedIntegrationEventHandler(IEventBus eventBus,
            AuthorizationDbContext dbContext)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task Handle(MessageDeletedIntegrationEvent @event)
        {
            try
            {
                var subscribers = await _dbContext.Authorizations.Where(x => x.DataId == @event.DataId).ToListAsync();
                _dbContext.Authorizations.RemoveRange(subscribers);

                _dbContext.SaveChanges();
            }
            catch (Exception)
            {

            }
        }
    }
}