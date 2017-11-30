using System;

namespace Sma.Stm.EventBus.Events
{
    public class IntegrationEvent
    {
        protected IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        public Guid Id  { get; }
        public DateTime CreationDate { get; }
    }
}
