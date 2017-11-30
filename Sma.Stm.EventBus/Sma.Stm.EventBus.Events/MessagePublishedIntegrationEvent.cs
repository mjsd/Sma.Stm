namespace Sma.Stm.EventBus.Events
{
    public class MessagePublishedIntegrationEvent : IntegrationEvent
    {
        public string DataId { get; set; }
        public string Content { get; set; }
    }
}