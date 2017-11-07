using Sma.Stm.Common.DocumentDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace Sma.Stm.Services.GenericMessageService.Models
{
    public class PublishedMessage : DocumentBase
    {
        public XmlDocument Content { get; set; }

        public DateTime PublishTime { get; set; }
    }
}