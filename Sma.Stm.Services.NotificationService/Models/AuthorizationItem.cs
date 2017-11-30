using System;

namespace Sma.Stm.Services.NotificationService.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public string Body { get; set; }

        public string FromOrgId { get; set; }

        public string FromOrgName { get; set; }

        public string FromServiceId { get; set; }

        public DateTime NotificationCreatedAt { get; set; }

        public EnumNotificationType NotificationType { get; set; }

        public DateTime ReceivedAt { get; set; }

        public string Subject { get; set; }

        public EnumNotificationSource NotificationSource { get; set; }

        public bool Fetched { get; set; }
    }

    public enum EnumNotificationType : int
    {
        MessageWaiting = 1,
        UnauthorizedRequest = 2,
        AcknowledgementReceived = 3,
        ErrorMessage = 4

    }

    public enum EnumNotificationSource
    {
        Vis = 1,
        Spis = 2,
    }
}