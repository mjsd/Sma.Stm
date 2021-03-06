﻿using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace Sma.Stm.Services.GenericMessageService.Models
{
    /// <summary>
    /// Acknowledgement message that incoming (uploaded) message has been delivered to consumer
    /// </summary>
    [DataContract]
    public partial class DeliveryAck : IEquatable<DeliveryAck>
    {
        /// <summary>
        /// Acknowledgement message that incoming (uploaded) message has been delivered to consumer
        /// </summary>
        public DeliveryAck()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryAck" /> class.
        /// </summary>
        /// <param name="id">Id for the ACK (required).</param>
        /// <param name="referenceId">Reference to delivered message (URN) (required).</param>
        /// <param name="timeOfDelivery">Time of delivery (required).</param>
        /// <param name="fromId">Identity of source (sender) of message that have been delivered (URN) (required).</param>
        /// <param name="fromName">Friendly name of sender (required).</param>
        /// <param name="toId">Identity of target (recipient) of message delivery (URN) (required).</param>
        /// <param name="toName">Friendly name of recipient (required).</param>
        /// <param name="ackResult">AckResult (required).</param>
        public DeliveryAck(string id = null, string referenceId = null, DateTime? timeOfDelivery = null, string fromId = null, string fromName = null, string toId = null, string toName = null, string ackResult = null)
        {
            // to ensure "Id" is required (not null)
            if (id == null)
            {
                throw new InvalidDataException("Id is a required property for DeliveryAck and cannot be null");
            }
            else
            {
                this.Id = id;
            }
            // to ensure "ReferenceId" is required (not null)
            if (referenceId == null)
            {
                throw new InvalidDataException("ReferenceId is a required property for DeliveryAck and cannot be null");
            }
            else
            {
                this.ReferenceId = referenceId;
            }
            // to ensure "TimeOfDelivery" is required (not null)
            if (timeOfDelivery == null)
            {
                throw new InvalidDataException("TimeOfDelivery is a required property for DeliveryAck and cannot be null");
            }
            else
            {
                this.TimeOfDelivery = timeOfDelivery;
            }
            // to ensure "FromId" is required (not null)
            if (fromId == null)
            {
                throw new InvalidDataException("FromId is a required property for DeliveryAck and cannot be null");
            }
            else
            {
                this.FromId = fromId;
            }
            // to ensure "FromName" is required (not null)
            if (fromName == null)
            {
                throw new InvalidDataException("FromName is a required property for DeliveryAck and cannot be null");
            }
            else
            {
                this.FromName = fromName;
            }
            // to ensure "ToId" is required (not null)
            if (toId == null)
            {
                throw new InvalidDataException("ToId is a required property for DeliveryAck and cannot be null");
            }
            else
            {
                this.ToId = toId;
            }
            // to ensure "ToName" is required (not null)
            if (toName == null)
            {
                throw new InvalidDataException("ToName is a required property for DeliveryAck and cannot be null");
            }
            else
            {
                this.ToName = toName;
            }
            // to ensure "AckResult" is required (not null)
            if (ackResult == null)
            {
                throw new InvalidDataException("AckResult is a required property for DeliveryAck and cannot be null");
            }
            else
            {
                this.AckResult = ackResult;
            }

        }

        /// <summary>
        /// Acknowledgement ID
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Reference ID such as a UVID, TXT id or area message id
        /// </summary>
        [DataMember(Name = "referenceId")]
        public string ReferenceId { get; set; }

        /// <summary>
        /// Time of Delivery of message to consumer
        /// </summary>
        [DataMember(Name = "timeOfDelivery")]
        public DateTime? TimeOfDelivery { get; set; }

        /// <summary>
        /// Identity O (organisation) of the message sender in MRN format
        /// </summary>
        [DataMember(Name = "fromId")]
        public string FromId { get; set; }

        /// <summary>
        /// "Identity O (organisation) of the message sender in full name
        /// </summary>
        [DataMember(Name = "fromName")]
        public string FromName { get; set; }

        /// <summary>
        /// Identity O (organisation) of the message receiver in MRN format
        /// </summary>
        [DataMember(Name = "toId")]
        public string ToId { get; set; }

        /// <summary>
        /// IIdentity O (organisation) of the message receiver in full name
        /// </summary>
        [DataMember(Name = "toName")]
        public string ToName { get; set; }

        /// <summary>
        /// Descriptive acknowledgement message
        /// </summary>
        [DataMember(Name = "ackResult")]
        public string AckResult { get; set; }


        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class DeliveryAck {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  ReferenceId: ").Append(ReferenceId).Append("\n");
            sb.Append("  TimeOfDelivery: ").Append(TimeOfDelivery).Append("\n");
            sb.Append("  FromId: ").Append(FromId).Append("\n");
            sb.Append("  FromName: ").Append(FromName).Append("\n");
            sb.Append("  ToId: ").Append(ToId).Append("\n");
            sb.Append("  ToName: ").Append(ToName).Append("\n");
            sb.Append("  AckResult: ").Append(AckResult).Append("\n");
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
            return Equals((DeliveryAck)obj);
        }

        /// <summary>
        /// Returns true if DeliveryAck instances are equal
        /// </summary>
        /// <param name="other">Instance of DeliveryAck to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(DeliveryAck other)
        {

            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return
                (
                    this.Id == other.Id ||
                    this.Id != null &&
                    this.Id.Equals(other.Id)
                ) &&
                (
                    this.ReferenceId == other.ReferenceId ||
                    this.ReferenceId != null &&
                    this.ReferenceId.Equals(other.ReferenceId)
                ) &&
                (
                    this.TimeOfDelivery == other.TimeOfDelivery ||
                    this.TimeOfDelivery != null &&
                    this.TimeOfDelivery.Equals(other.TimeOfDelivery)
                ) &&
                (
                    this.FromId == other.FromId ||
                    this.FromId != null &&
                    this.FromId.Equals(other.FromId)
                ) &&
                (
                    this.FromName == other.FromName ||
                    this.FromName != null &&
                    this.FromName.Equals(other.FromName)
                ) &&
                (
                    this.ToId == other.ToId ||
                    this.ToId != null &&
                    this.ToId.Equals(other.ToId)
                ) &&
                (
                    this.ToName == other.ToName ||
                    this.ToName != null &&
                    this.ToName.Equals(other.ToName)
                ) &&
                (
                    this.AckResult == other.AckResult ||
                    this.AckResult != null &&
                    this.AckResult.Equals(other.AckResult)
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
                if (this.Id != null)
                    hash = hash * 59 + this.Id.GetHashCode();
                if (this.ReferenceId != null)
                    hash = hash * 59 + this.ReferenceId.GetHashCode();
                if (this.TimeOfDelivery != null)
                    hash = hash * 59 + this.TimeOfDelivery.GetHashCode();
                if (this.FromId != null)
                    hash = hash * 59 + this.FromId.GetHashCode();
                if (this.FromName != null)
                    hash = hash * 59 + this.FromName.GetHashCode();
                if (this.ToId != null)
                    hash = hash * 59 + this.ToId.GetHashCode();
                if (this.ToName != null)
                    hash = hash * 59 + this.ToName.GetHashCode();
                if (this.AckResult != null)
                    hash = hash * 59 + this.AckResult.GetHashCode();
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
        public static bool operator ==(DeliveryAck left, DeliveryAck right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(DeliveryAck left, DeliveryAck right)
        {
            return !Equals(left, right);
        }

        #endregion Operators

    }

}
