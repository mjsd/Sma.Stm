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
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Sma.Stm.Ssc
{
    /// <summary>
    /// A technical way to describe aspects if a service.The Xml should validate against a XSD from a SpecificationTemplate.
    /// </summary>
    [DataContract]
    public partial class Xml :  IEquatable<Xml>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Xml" /> class.
        /// </summary>
        /// <param name="Comment">Comment.</param>
        /// <param name="Content">Content.</param>
        /// <param name="ContentContentType">ContentContentType.</param>
        /// <param name="Id">Id.</param>
        /// <param name="Name">Name.</param>
        public Xml(string Comment = null, string Content = null, string ContentContentType = null, long? Id = null, string Name = null)
        {
            this.Comment = Comment;
            this.Content = Content;
            this.ContentContentType = ContentContentType;
            this.Id = Id;
            this.Name = Name;
            
        }

        /// <summary>
        /// Gets or Sets Comment
        /// </summary>
        [DataMember(Name="comment")]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or Sets Content
        /// </summary>
        [DataMember(Name="content")]
        public string Content { get; set; }

        /// <summary>
        /// Gets or Sets ContentContentType
        /// </summary>
        [DataMember(Name="contentContentType")]
        public string ContentContentType { get; set; }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id")]
        public long? Id { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name="name")]
        public string Name { get; set; }


        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Xml {\n");
            sb.Append("  Comment: ").Append(Comment).Append("\n");
            sb.Append("  Content: ").Append(Content).Append("\n");
            sb.Append("  ContentContentType: ").Append(ContentContentType).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
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
            return Equals((Xml)obj);
        }

        /// <summary>
        /// Returns true if Xml instances are equal
        /// </summary>
        /// <param name="other">Instance of Xml to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Xml other)
        {

            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    this.Comment == other.Comment ||
                    this.Comment != null &&
                    this.Comment.Equals(other.Comment)
                ) && 
                (
                    this.Content == other.Content ||
                    this.Content != null &&
                    this.Content.Equals(other.Content)
                ) && 
                (
                    this.ContentContentType == other.ContentContentType ||
                    this.ContentContentType != null &&
                    this.ContentContentType.Equals(other.ContentContentType)
                ) && 
                (
                    this.Id == other.Id ||
                    this.Id != null &&
                    this.Id.Equals(other.Id)
                ) && 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
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
                    if (this.Comment != null)
                    hash = hash * 59 + this.Comment.GetHashCode();
                    if (this.Content != null)
                    hash = hash * 59 + this.Content.GetHashCode();
                    if (this.ContentContentType != null)
                    hash = hash * 59 + this.ContentContentType.GetHashCode();
                    if (this.Id != null)
                    hash = hash * 59 + this.Id.GetHashCode();
                    if (this.Name != null)
                    hash = hash * 59 + this.Name.GetHashCode();
                return hash;
            }
        }

        #region Operators

        public static bool operator ==(Xml left, Xml right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Xml left, Xml right)
        {
            return !Equals(left, right);
        }

        #endregion Operators

    }
}
