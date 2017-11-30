using System;
using System.Runtime.Serialization;

namespace Sma.Stm.Services.SubscriptionService.Models
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SubscriptionObject : IEquatable<SubscriptionObject>
    {
        /// <summary>
        /// 
        /// </summary>
        public SubscriptionObject()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "identityId")]
        public string IdentityId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "identityName")]
        public string IdentityName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "endpointURL")]
        public Uri EndpointUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(SubscriptionObject other)
        {
            return true;
        }
    }
}
