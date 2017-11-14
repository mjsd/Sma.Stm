using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace Sma.Stm.Services.GenericMessageService.Models
{
    public class PublishedMessage
    {
        public int Id { get; set; }

        public string DataId { get; set; }

        public string Content { get; set; }

        public DateTime PublishTime { get; set; }
    }
}