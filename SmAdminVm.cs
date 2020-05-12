using SmWeb.Models;
using Stripe;
using System.Collections.Generic;

namespace SmWeb.ViewModels
{
    public class SmAdminVm
    {
        public StripeVendorSubscription VendorSubscription { get; set; }

        public Stripe.Product ServiceProduct { get; set; }

        public string ProductName { get; set; }

        public Balance Balance { get; set; }
        public decimal PendingBalance { get; set; }
        public decimal AvailableBalance { get; set; }

        public IEnumerable<Member> Members { get; set; }
        public bool HasMembers { get; set; }

        public IEnumerable<Stripe.Product> Products { get; set; }
        public bool HasProducts { get; set; }

        public IEnumerable<Plan> Plans { get; set; }
        public bool HasPlans { get; set; }

        public IEnumerable<Account> Accounts { get; set; }
        public bool HasAccounts { get; set; }

        public IEnumerable<Customer> Customers { get; set; }
        public bool HasCustomers { get; set; }

        public IEnumerable<Subscription> Subscriptions { get; set; }
        public bool HasSubscriptions { get; set; }

        public IEnumerable<Charge> Charges { get; set; }
        public bool HasCharges { get; set; }

        public IEnumerable<BalanceTransaction> BalanceHistory { get; set; }
        public bool HasBalance { get; set; }
    }
}
