﻿using Sma.Stm.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sma.Stm.EventBus.Events
{
    public class MessageDeletedIntegrationEvent : IntegrationEvent
    {
        public string DataId { get; set; }
    }
}