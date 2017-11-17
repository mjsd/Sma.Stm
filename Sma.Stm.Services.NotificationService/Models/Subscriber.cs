using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sma.Stm.Services.NotificationService.Models
{
    public class Subscriber
    {
        public int Id { get; set; }

        public string NotificationEndpointUrl { get; set; }
    }
}
