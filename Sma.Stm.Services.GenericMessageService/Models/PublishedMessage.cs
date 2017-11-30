using System;

namespace Sma.Stm.Services.GenericMessageService.Models
{
    public class PublishedMessage
    {
        public int Id { get; set; }

        public string DataId { get; set; }

        public string Status { get; set; }

        public string Content { get; set; }

        public DateTime PublishTime { get; set; }
    }
}