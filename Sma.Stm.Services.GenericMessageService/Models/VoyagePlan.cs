using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Sma.Stm.Services.GenericMessageService.Models
{
    /// <summary>
    /// A voyage plan in RTZ XML format
    /// </summary>
    [DataContract]
    public partial class VoyagePlan :  IEquatable<VoyagePlan>
    {
        /// <summary>
        /// 
        /// </summary>
        public VoyagePlan()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VoyagePlan" /> class.
        /// </summary>
        /// <param name="Route">Gets or Sets Route.</param>
        public VoyagePlan(string Route = null)
        {
            this.Route = Route;
        }

        /// <summary>
        /// A voyage plan in RTZ XML format
        /// </summary>
        /// <value>Route</value>
        [DataMember(Name="route")]
        public string Route { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VoyagePlan {\n");
            sb.Append("  Route: ").Append(Route).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((VoyagePlan)obj);
        }

        /// <summary>
        /// Returns true if VoyagePlan instances are equal
        /// </summary>
        /// <param name="other">Instance of VoyagePlan to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VoyagePlan other)
        {

            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    this.Route == other.Route ||
                    this.Route != null &&
                    this.Route.Equals(other.Route)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            // credit: http://stackoverflow.com/a/263416/677735
            unchecked // Overflow is fine, just wrap
            {
                int hash = 41;
                // Suitable nullity checks etc, of course :)
                    if (this.Route != null)
                    hash = hash * 59 + this.Route.GetHashCode();
                return hash;
            }
        }

        #region Operators

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(VoyagePlan left, VoyagePlan right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(VoyagePlan left, VoyagePlan right)
        {
            return !Equals(left, right);
        }

        #endregion Operators

    }
}
