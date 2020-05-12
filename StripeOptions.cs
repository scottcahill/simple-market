namespace SmWeb.Models
{
    public class StripeOptions
    {
        public string PubKey { get; set; }
        public string SecKey { get; set; }
        public string SecWebHook { get; set; }
        public string SubscriptPlanId { get; set; }
    }
}
