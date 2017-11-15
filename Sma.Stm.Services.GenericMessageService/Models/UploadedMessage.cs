using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace Sma.Stm.Services.GenericMessageService.Models
{
    public class UploadedMessage
    {
        public int Id { get; set; }
        public string DataId { get; set; }
        public string Content { get; set; }
        public string FromOrgId { get; set; }
        public string FromServiceId { get; set; }
        public DateTime ReceiveTime { get; set; }
        public bool Fetched { get; set; }
    }
}
