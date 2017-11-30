namespace Sma.Stm.EventBus.Events
{
    public class NewSubscriptionIntegrationEvent : IntegrationEvent
    {
        public string OrgId { get; set; }

        public string ServiceId { get; set; }

        public string DataId { get; set; }

        public string CallbackEndpoint { get; set; }
    }
}