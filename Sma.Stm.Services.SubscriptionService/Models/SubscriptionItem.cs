namespace Sma.Stm.Services.SubscriptionService.Models
{
    public class SubscriptionItem
    {
        public int Id { get; set; }

        public string OrgId { get; set; }

        public string ServiceId { get; set; }

        public string DataId { get; set; }

        public string CallbackEndpoint { get; set; }
    }
}
