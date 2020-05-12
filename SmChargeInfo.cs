using SmWeb.Models;
namespace SmWeb.ViewModels
{
    public class SmChargeInfo
    {
        public string BuyerStripeCustId { get; set; }
        public string BuyerStripeAcctId { get; set; }
        public string VendorStripeAcctId { get; set; }
        public string OrderItemId { get; set; }  // passed to Stripe Transfer_group
        public string ChargeId { get; set; }
        public string BuyersEmail { get; set; }
        public MemberType BuyerMemberType { get; set; }
        public MemberType VendorMemberType { get; set; }

        public Transaction Tran { get; set; }

        public long? TotalAmount { get; set; }
        public long? SmFee { get; set; }
        //  Looks like stripe extracts it's own fee
        //  we don't need to
        //public long? StripeFee { get; set; }

    }
}
