namespace Sma.Stm.EventBus.Events
{
    public class AuthorizationRemovedIntegrationEvent : IntegrationEvent
    {
        public string OrgId { get; set; }
        public string DataId { get; set; }
    }
}