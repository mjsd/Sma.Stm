using Sma.Stm.Common.DocumentDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace Sma.Stm.Services.GenericMessageService.Models
{
    public class UploadedMessage : DocumentBase
    {
        public XmlDocument Content { get; set; }
        public string FromOrgId { get; set; }
        public string FromServiceId { get; set; }
        public DateTime ReceiveTime { get; set; }
    }
}
