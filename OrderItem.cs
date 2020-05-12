using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmWeb.Models
{
    public class OrderItem
    {

        [Key]
        [JsonProperty(PropertyName = "orderItemId")]
        public string OrderItemId { get; set; }

        [JsonProperty(PropertyName = "vendorId")]
        public string VendorId { get; set; }

        [JsonProperty(PropertyName = "productId")]
        public string ProductId;

        [JsonProperty(PropertyName = "name")]
        public string Name;

        [JsonProperty(PropertyName = "price")]
        public decimal Price;

        [JsonProperty(PropertyName = "lineTotal")]
        public decimal LineTotal;

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "imgPath")]
        public string ImgPath { get; set; }

        [JsonProperty(PropertyName = "taxAmt")]
        public decimal TaxAmt { get; set; }

        [JsonProperty(PropertyName = "smFee")]
        public Decimal SmFee { get; set; }

        [JsonProperty(PropertyName = "stripeFee")]
        public Decimal StripeFee { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public Int16 Quantity;

        [JsonProperty(PropertyName = "promiseDays")]
        public System.Int16 PromiseDays { get; set; }

        [JsonProperty(PropertyName = "dateDeliveryPromise")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime DateDeliveryPromise { get; set; }

        // store requested new delivery date here and only
        // update DateDeliveryPromise if buyer agrees to change
        [JsonProperty(PropertyName = "dateDeliverydefered")]
        public DateTime DateDeliveryDefered { get; set; }

        [JsonProperty(PropertyName = "dateShipped")]
        public DateTime DateShipped { get; set; }

        [JsonProperty(PropertyName = "carrier")]
        public string Carrier;

        [JsonProperty(PropertyName = "trackingNumber")]
        public string TrackingNumber;

        // what are the states here???  where are they listed??
        [JsonProperty(PropertyName = "orderItemState")]
        public string OrderItemState { get; set; }
    }
}
