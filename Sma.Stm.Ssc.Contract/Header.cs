using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Sma.Stm.Ssc.Contract
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class Header :  IEquatable<Header>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Header" /> class.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public Header(string key = null, string value = null)
        {
            this.Key = key;
            this.Value = value;
            
        }

        /// <summary>
        /// Gets or Sets Key
        /// </summary>
        [DataMember(Name="key")]
        public string Key { get; set; }

        /// <summary>
        /// Gets or Sets Value
        /// </summary>
        [DataMember(Name="value")]
        public string Value { get; set; }


        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Header {\n");
            sb.Append("  Key: ").Append(Key).Append("\n");
            sb.Append("  Value: ").Append(Value).Append("\n");
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
            return Equals((Header)obj);
        }

        /// <summary>
        /// Returns true if Header instances are equal
        /// </summary>
        /// <param name="other">Instance of Header to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Header other)
        {

            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    this.Key == other.Key ||
                    this.Key != null &&
                    this.Key.Equals(other.Key)
                ) && 
                (
                    this.Value == other.Value ||
                    this.Value != null &&
                    this.Value.Equals(other.Value)
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
                    if (this.Key != null)
                    hash = hash * 59 + this.Key.GetHashCode();
                    if (this.Value != null)
                    hash = hash * 59 + this.Value.GetHashCode();
                return hash;
            }
        }

        #region Operators

        public static bool operator ==(Header left, Header right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Header left, Header right)
        {
            return !Equals(left, right);
        }

        #endregion Operators

    }
}
