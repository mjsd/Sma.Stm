using System;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Sma.Stm.Services.AuthorizationService.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class IdentityDescriptionObject :  IEquatable<IdentityDescriptionObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityDescriptionObject" /> class.
        /// </summary>
        public IdentityDescriptionObject()
        {
        }

        /// <summary>
        /// identity in urn format according to ID registry
        /// </summary>
        /// <value>identity in urn format according to ID registry</value>
        [DataMember(Name="identityId")]
        public string IdentityId { get; set; }

        /// <summary>
        /// Identity shortname in ID registry
        /// </summary>
        /// <value>Identity shortname in ID registry</value>
        [DataMember(Name="identityName")]
        public string IdentityName { get; set; }


        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class IdentityDescriptionObject {\n");
            sb.Append("  IdentityId: ").Append(IdentityId).Append("\n");
            sb.Append("  IdentityName: ").Append(IdentityName).Append("\n");
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
            return Equals((IdentityDescriptionObject)obj);
        }

        /// <summary>
        /// Returns true if IdentityDescriptionObject instances are equal
        /// </summary>
        /// <param name="other">Instance of IdentityDescriptionObject to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(IdentityDescriptionObject other)
        {

            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    this.IdentityId == other.IdentityId ||
                    this.IdentityId != null &&
                    this.IdentityId.Equals(other.IdentityId)
                ) && 
                (
                    this.IdentityName == other.IdentityName ||
                    this.IdentityName != null &&
                    this.IdentityName.Equals(other.IdentityName)
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
                    if (this.IdentityId != null)
                    hash = hash * 59 + this.IdentityId.GetHashCode();
                    if (this.IdentityName != null)
                    hash = hash * 59 + this.IdentityName.GetHashCode();
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
        public static bool operator ==(IdentityDescriptionObject left, IdentityDescriptionObject right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(IdentityDescriptionObject left, IdentityDescriptionObject right)
        {
            return !Equals(left, right);
        }

        #endregion Operators

    }
}
