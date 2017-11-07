using Sma.Stm.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sma.Stm.Services.AuthorizationServiceService.IntegrationEvents.Events
{
    public class AuthorizationRemovedIntegrationEvent : IntegrationEvent
    {
        public string OrgId { get; set; }
        public string DataId { get; set; }
    }
}