using System;
using Newtonsoft.Json;

namespace SmWeb.Models
{
    public class StripeVendorSubscription
    {
        //  sm Vendorship 14.95 yearly subscription Product/Plan
        [JsonProperty(PropertyName = "serviceProductId")]
        public string ServiceProductId { get; set; }

        [JsonProperty(PropertyName = "vendorshipPlanId")]
        public string VendorshipPlanId { get; set; }
    }
}
