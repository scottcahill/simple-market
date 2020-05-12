using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmWeb.Models
{
    public class Transaction
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "tranType")]
        public string TranType { get; set; }

        [JsonProperty(PropertyName = "orderSetId")]
        public string OrderSetId { get; set; }

        [JsonProperty(PropertyName = "sourceMemberId")]
        public string SourceMemberId { get; set; }

        [JsonProperty(PropertyName = "dateCreated")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime DateCreated { get; set; }

        [JsonProperty(PropertyName = "totalAmount")]
        public Decimal TotalAmount { get; set; }

        // NOTE: used for 'charge' transaction [instead of carrying entire OrderItem]
        [JsonProperty(PropertyName = "refOrderItemId")]
        public string RefOrderItemId { get; set; }

        [JsonProperty(PropertyName = "orderItem")]
        public OrderItem OrderItem { get; set; }

        [JsonProperty(PropertyName = "tranState")]
        public TranStateElement TranState { get; set; }

        [JsonProperty(PropertyName = "cancelReason")]
        public string CancelReason { get; set; }

        // Charge data -----------------
        [JsonProperty(PropertyName = "chargeId")]
        public string ChargeId { get; set; }

        [JsonProperty(PropertyName = "chargeStatus")]
        public string ChargeStatus { get; set; }

        [JsonProperty(PropertyName = "balanceTranId")]
        public string BalanceTranId { get; set; }

        [JsonProperty(PropertyName = "transferId")]
        public string TransferId { get; set; }

        [JsonProperty(PropertyName = "transferDestinationId")]
        public string TransferDestinationId { get; set; }

        [JsonProperty(PropertyName = "transferGroupId")]
        public string TransferGroupId { get; set; }
        // Charge data --------------- end
    }
}
