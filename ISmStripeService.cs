using System.Collections.Generic;
using System.Threading.Tasks;
using SmWeb.Models;
using SmWeb.ViewModels;
using Stripe;

namespace SmWeb.Services
{
    public interface ISmStripeService
    {
        // Administration
        Task<SmAdminVm> GetAdminInfo();

        Stripe.Product CreateProductTypeService(string ProductName);

        Plan CreatePlan(ProdPlanVm vm);

        Stripe.Product GetProduct(string ProductId);

        Task<bool> CancelSubscription(string SubscriptionId);
        //  end

        Token GetCCToken(string CCNum, string CvcNum, int? exYear, int? exMonth);

        //Task<Member> AddBankToAccount(Member Member, Token token);

        Task<Account> AddBankToAccount(Member Member, string BusinessName, string BankAccount, string RoutingNumber);

        Task<Member> CreateStripeSharedCustomer(Member Member, Token Token);

        Task<IEnumerable<BalanceTransaction>> GetBalanceHistoryFor(string VendorsSKey);

        Task<Balance> GetBalanceFor(string AccountId);

        void UpdateAdminInfo(StripeVendorSubscription vm);

        Task<Subscription> SubscribeMemberToVendorshipPlan(string VendorshipPlanId, string CustomerId);

        Task<ResultAndMessage> CreateContribution(ContributeVm vm);

        Task<Customer> CreateMember(string MemberEmail, string stripeToken);

        Task<Member> CreateStripeAccountOnPlatform(Member Member, string BusinessName);

        ResultAndMessage CreditCardPurchase(string vendorCustId, decimal purchaseAmount, decimal smFee, string stripeToken);

        Task<Account> DeleteAccount(string AccountId);

        // Charges
        // first trial .......
        Charge DestinationCharge(SmChargeInfo Charge);

        Charge SeparateCharge(SmChargeInfo vm, string TransferGroup);

        Transfer TransferFromSmToVendor(SmChargeInfo Charge);
        // end first trial

        ResultAndMessage ChargeAccountToAccount(string BuyerCustId, string VendorAcctId, decimal purchaseAmount, decimal smFee);
        ResultAndMessage ChargeCustomerToAccount(string BuyerCustId, string VendorAcctId, string StripeKey, decimal purchaseAmount, decimal smFee);
       // ResultAndMessage CreateCharge(ChargeInfo data);

        Task<bool> CaptureAuthorizedCharge(string stripeAuthChargeId, int amount);

        Task<bool> AuthorizeCharge(string vendorAccount, string stripeBuyerId, int amount, int platformFees);

    }
}