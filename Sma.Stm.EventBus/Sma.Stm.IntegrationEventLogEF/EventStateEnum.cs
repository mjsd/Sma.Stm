using System;
using System.Collections.Generic;
using System.Text;

namespace Sma.Stm.EventBus.Log
{
    public enum EventStateEnum
    {
        NotPublished = 0,
        Published = 1,
        PublishedFailed = 2
    }
}
