using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Sma.Stm.Services.GenericMessageService.Models
{
    /// <summary>
    /// Response object from GET voyagePlans. Can contain 0 or several (0..*) voyage plans
    /// </summary>
    [DataContract]
    public partial class GetVoyagePlanResponse :  IEquatable<GetVoyagePlanResponse>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetVoyagePlanResponse" /> class.
        /// </summary>
        /// <param name="lastInteractionTime">Gets or Sets LastInteractionTime.</param>
        /// <param name="voyagePlans">Gets or Sets VoyagePlan.</param>
        public GetVoyagePlanResponse(DateTime? lastInteractionTime = null, List<VoyagePlan> voyagePlans = null)
        {
            this.LastInteractionTime = lastInteractionTime;
            this.VoyagePlans = voyagePlans;
        }

        /// <summary>
        /// Last interaction time with private application. Can indicate the current connectivity on private side of VIS
        /// </summary>
        /// <value>LastInteractionTime</value>
        [DataMember(Name="lastInteractionTime")]
        public DateTime? LastInteractionTime { get; set; }

        /// <summary>
        /// Array of voyage plans in RTZ XML format
        /// </summary>
        /// <value>VoyagePlans</value>
        [DataMember(Name="voyagePlans")]
        public List<VoyagePlan> VoyagePlans { get; set; }


        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class GetVoyagePlanResponse {\n");
            sb.Append("  LastInteractionTime: ").Append(LastInteractionTime).Append("\n");
            sb.Append("  VoyagePlan: ").Append(VoyagePlans).Append("\n");
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
            return Equals((GetVoyagePlanResponse)obj);
        }

        /// <summary>
        /// Returns true if GetVoyagePlanResponse instances are equal
        /// </summary>
        /// <param name="other">Instance of GetVoyagePlanResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(GetVoyagePlanResponse other)
        {

            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    this.LastInteractionTime == other.LastInteractionTime ||
                    this.LastInteractionTime != null &&
                    this.LastInteractionTime.Equals(other.LastInteractionTime)
                ) && 
                (
                    this.VoyagePlans == other.VoyagePlans ||
                    this.VoyagePlans != null &&
                    this.VoyagePlans.SequenceEqual(other.VoyagePlans)
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
                var hash = 41;
                // Suitable nullity checks etc, of course :)
                    if (this.LastInteractionTime != null)
                    hash = hash * 59 + this.LastInteractionTime.GetHashCode();
                    if (this.VoyagePlans != null)
                    hash = hash * 59 + this.VoyagePlans.GetHashCode();
                return hash;
            }
        }

        #region Operators

        /// <summary>
        /// Equality operator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(GetVoyagePlanResponse left, GetVoyagePlanResponse right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not equal operator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(GetVoyagePlanResponse left, GetVoyagePlanResponse right)
        {
            return !Equals(left, right);
        }

        #endregion Operators

    }
}
