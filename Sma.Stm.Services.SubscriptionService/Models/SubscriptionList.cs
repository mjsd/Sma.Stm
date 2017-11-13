using Sma.Stm.Common.DocumentDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sma.Stm.Services.SubscriptionService.Models
{
    public class SubscriptionList : DocumentBase
    {
        public List<SubscriptionItem> Subscriptions { get; set; }
    }
}