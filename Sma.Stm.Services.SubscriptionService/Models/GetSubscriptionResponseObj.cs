﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Sma.Stm.Services.SubscriptionService.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class GetSubscriptionResponseObj
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string DataId { get; set; }
    }
}