namespace Sma.Stm.EventBus.Events
{
    public class MessageDeletedIntegrationEvent : IntegrationEvent
    {
        public string DataId { get; set; }
    }
}