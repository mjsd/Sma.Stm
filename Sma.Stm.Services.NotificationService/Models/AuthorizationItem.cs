using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        MESSAGE_WAITING = 1,
        UNAUTHORIZED_REQUEST = 2,
        ACKNOWLEDGEMENT_RECEIVED = 3,
        ERROR_MESSAGE = 4

    }

    public enum EnumNotificationSource
    {
        VIS = 1,
        SPIS = 2,
    }
}