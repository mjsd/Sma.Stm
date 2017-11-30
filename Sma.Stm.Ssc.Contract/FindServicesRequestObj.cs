/*
 * STM Voyage Information Service STM Onboard API
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: v2
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

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
    public partial class FindServicesRequestObj :  IEquatable<FindServicesRequestObj>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindServicesRequestObj" /> class.
        /// </summary>
        /// <param name="filter">Filter.</param>
        /// <param name="page">Page.</param>
        /// <param name="pageSize">PageSize.</param>
        public FindServicesRequestObj(FindServicesRequestObjFilter filter = null, long? page = null, long? pageSize = null)
        {
            this.Filter = filter;
            this.Page = page;
            this.PageSize = pageSize;
            
        }

        /// <summary>
        /// Gets or Sets Filter
        /// </summary>
        [DataMember(Name="filter")]
        public FindServicesRequestObjFilter Filter { get; set; }

        /// <summary>
        /// Gets or Sets Page
        /// </summary>
        [DataMember(Name="page")]
        public long? Page { get; set; }

        /// <summary>
        /// Gets or Sets PageSize
        /// </summary>
        [DataMember(Name="pageSize")]
        public long? PageSize { get; set; }


        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class FindServicesRequestObj {\n");
            sb.Append("  Filter: ").Append(Filter).Append("\n");
            sb.Append("  Page: ").Append(Page).Append("\n");
            sb.Append("  PageSize: ").Append(PageSize).Append("\n");
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
            return Equals((FindServicesRequestObj)obj);
        }

        /// <summary>
        /// Returns true if FindServicesRequestObj instances are equal
        /// </summary>
        /// <param name="other">Instance of FindServicesRequestObj to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(FindServicesRequestObj other)
        {

            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    this.Filter == other.Filter ||
                    this.Filter != null &&
                    this.Filter.Equals(other.Filter)
                ) && 
                (
                    this.Page == other.Page ||
                    this.Page != null &&
                    this.Page.Equals(other.Page)
                ) && 
                (
                    this.PageSize == other.PageSize ||
                    this.PageSize != null &&
                    this.PageSize.Equals(other.PageSize)
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
                    if (this.Filter != null)
                    hash = hash * 59 + this.Filter.GetHashCode();
                    if (this.Page != null)
                    hash = hash * 59 + this.Page.GetHashCode();
                    if (this.PageSize != null)
                    hash = hash * 59 + this.PageSize.GetHashCode();
                return hash;
            }
        }

        #region Operators

        public static bool operator ==(FindServicesRequestObj left, FindServicesRequestObj right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FindServicesRequestObj left, FindServicesRequestObj right)
        {
            return !Equals(left, right);
        }

        #endregion Operators

    }
}
