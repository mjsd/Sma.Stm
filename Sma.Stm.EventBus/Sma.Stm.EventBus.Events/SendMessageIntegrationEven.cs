using System;

namespace Sma.Stm.EventBus.Events
{
    public class SendMessageIntegrationEven : IntegrationEvent
    {
        public string TargetServiceId { get; set; }
        public string SenderServiceId { get; set; }
        public string SenderOrgId { get; set; }
        public string Body { get; set; }
        public Uri Url { get; set; }
        public string HttpMethod { get; set; }
        public string ContentType { get; set; }
    }
}