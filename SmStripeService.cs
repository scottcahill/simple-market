using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmWeb.ViewModels;
using SmWeb.Models;
using SmWeb.Utils;

namespace SmWeb.Services
{
    public class SmStripeService : ISmStripeService
    {
        private readonly IOptions<StripeOptions> options;

        public SmStripeService(IOptions<StripeOptions> options)
        {
            this.options = options;
        }

        public Token GetCCToken(string CCNum,
                                string CvcNum,
                                int? exYear,
                                int? exMonth)
        {
            var tokenOptions = new TokenCreateOptions()
            {
                Card = new CreditCardOptions()
                {
                    Number = CCNum,

                    ExpYear = exYear,
                    ExpMonth = exMonth,
                    Cvc = CvcNum,
                    Currency = "usd"
                }
            };

            string errMsg;
            var tokenService = new TokenService();
            Token token = null;
            try
            {
                token = tokenService.Create(tokenOptions);
            }
            catch (StripeException e)
            {
                switch (e.StripeError.ErrorType)
                {
                    case "card_error":
                        errMsg = $"card_error: {e.StripeError.Message}  Code: {e.StripeError.Code}";
                        break;
                    case "api_connection_error":
                        errMsg = $"api_connection_error: {e.StripeError.Message}  Code: {e.StripeError.Code}";
                        break;
                    case "api_error":
                        errMsg = $"api_error: {e.StripeError.Message}  Code: {e.StripeError.Code}";
                        break;
                    case "authentication_error":
                        errMsg = $"authentication_error: {e.StripeError.Message}  Code: {e.StripeError.Code}";
                        break;
                    case "invalid_request_error":
                        errMsg = $"invalid_request_error: {e.StripeError.Message}  Code: {e.StripeError.Code}";
                        break;
                    case "rate_limit_error":
                        errMsg = $"rate_limit_error: {e.StripeError.Message}  Code: {e.StripeError.Code}";
                        break;
                    case "validation_error":
                        errMsg = $"validation_error: {e.StripeError.Message}  Code: {e.StripeError.Code}";
                        break;
                    default:
                        errMsg = $"unknown error: {e.StripeError.Message}  Code: {e.StripeError.Code}";
                        break;
                }
            }
            return token;
        }


        // Charges -------------------------
        //-----------------------------------------------
        // Stripe : Separate charges and transfers  -----------------
        //        create charges on the platform account and then separately transfer funds 
        //        to the connected account.This approach is similar to creating destination charges, 
        //        but provides greater flexibility over the flow of funds at the cost of a more 
        //        complicated integration.
        //  Using this approach:
        //      Your platform account is responsible for the cost of the Stripe fees, refunds, 
        //      and chargebacks, handling these for the connected account
        //      The payment itself appears as a charge in your platform account, with a separate, 
        //      manual allocation to the connected account, which decreases your platform’s balance 
        //      and increases the connected account’s balance
        //      Funds from charges can be allocated to more than one connected account
        //      Platform fees are earned by allocating less than the entire charge amount to the connected account

        public Charge SeparateCharge(SmChargeInfo vm, string TransferGroup)
        {
            var chargeOptions = new ChargeCreateOptions()
            {
                Amount = vm.TotalAmount,
                ReceiptEmail=vm.BuyersEmail,
                Source = vm.BuyerStripeAcctId,
                TransferGroup = TransferGroup,
                Currency = "usd",
                Description = "Buyer charged: " + vm.BuyersEmail,
            };

            var chargeService = new ChargeService();
            Charge charge = chargeService.Create(chargeOptions);

            return charge;
        }

        public Charge DestinationCharge(SmChargeInfo vm)
        {
            //  Destination charge ----------------
            //  - platform is responsible for the cost of the Stripe fees, refunds, and chargebacks.
            var chargeOptions = new ChargeCreateOptions()
            {
                Amount = vm.TotalAmount,
                Customer = vm.BuyerStripeCustId,
                Destination = new ChargeDestinationCreateOptions()
                {
                    Account = vm.VendorStripeAcctId,
                    //  effectively - taking fees
                    Amount = vm.TotalAmount - vm.SmFee
                    // This amount becomes available on the connected account’s normal transfer schedule, 
                    // just like funds from regular Stripe charges.
                },
                Currency = "usd",
                Description = "Buyer charged: " + vm.BuyersEmail,
            };

            var chargeService = new ChargeService();
            Charge charge = chargeService.Create(chargeOptions);

            return charge;
        }

        public Transfer TransferFromSmToVendor(SmChargeInfo vm)
        {
            var options = new TransferCreateOptions
            {
                Amount = (vm.TotalAmount - vm.SmFee),
                Currency = "usd",
                Destination = vm.VendorStripeAcctId,
                SourceTransaction = vm.ChargeId
            };

            var service = new TransferService();
            return service.Create(options);
        }

        // end Charges


        //   step one in subscription process - create a type=service
        //   product [Sm Admin Dashboard ??]
        public Stripe.Product CreateProductTypeService(string ProductName)
        {
            var options = new ProductCreateOptions
            {
                Name = ProductName,
                Type = "service",
            };
            var service = new ProductService();
            return service.Create(options);
        }

        public Stripe.Product GetProduct(string ProductId)
        {
            var productService = new ProductService();
            return productService.Get(ProductId);
        }

        public async Task<IEnumerable<Account>> ListAccounts()
        {
            var accountService = new AccountService();
            StripeList<Account> accountItems =
                await accountService.ListAsync(
                                  new AccountListOptions()
                                  {
                                      Limit = 30
                                  }
                                );
            return accountItems;
        }


        public async Task<IEnumerable<Charge>> ListCharges()
        {
            var service = new ChargeService();
            var options = new ChargeListOptions
            {
                Limit = 3,
            };
            var charges = await service.ListAsync(options);
            return charges;
        }

        public async Task<Balance> GetBalanceFor(string connectedAcctId)
        {
            var service = new BalanceService();
            RequestOptions ro = new RequestOptions()
            {
                StripeAccount = connectedAcctId
            };
            return await service.GetAsync(ro);
        }

        public async Task<IEnumerable<BalanceTransaction>> GetBalanceHistoryFor(string VendorsSKey)
        {
            var service = new BalanceTransactionService();
            var transactions = await service.ListAsync(
               new BalanceTransactionListOptions
               {
                   Limit = 12,
               }
             );
            return transactions;
            //  Don't know how with latest Stripe updates

            //var balanceService = new BalanceService();
            //var opts= new RequestOptions() {  ApiKey=VendorsSKey, StripeConnectAccountId=}
            //StripeList<BalanceTransaction> balanceTransactions =
            //                await balanceService.GetAsync(opts, CancellationToken.None)

            //return balanceTransactions;
        }

        public async Task<IEnumerable<Customer>> ListCustomers()
        {
            var customerService = new CustomerService();
            StripeList<Customer> customerItems = await customerService.ListAsync(
              new CustomerListOptions()
              {
                  Limit = 30
              }
            );
            return customerItems;
        }

        public async Task<Account> DeleteAccount(string AccountId)
        {
            var accountService = new AccountService();
            return await accountService.DeleteAsync(AccountId);
        }

        public async Task<bool> CancelSubscription(string SubscriptionId)
        {
            SubscriptionCancelOptions opts =
                new SubscriptionCancelOptions() { };

            var subscriptionService = new SubscriptionService();
            var sub = await subscriptionService.CancelAsync(SubscriptionId, opts);

            return (sub.Status == "canceled");
        }

        public async Task<IEnumerable<Stripe.Product>> ListProducts()
        {
            var productService = new ProductService();
            StripeList<Stripe.Product> productItems =
                await productService.ListAsync(
                                      new ProductListOptions()
                                      {
                                          Limit = 3
                                      }
            );

            return productItems;
        }
        public async Task<IEnumerable<Plan>> ListPlans()
        {
            var planService = new PlanService();
            StripeList<Plan> planItems =
                await planService.ListAsync(
                                  new PlanListOptions()
                                  {
                                      Limit = 3
                                  }
                                );
            return planItems;
        }

        public async Task<IEnumerable<Subscription>> ListSubscriptions()
        {
            var subscriptionService = new SubscriptionService();
            StripeList<Subscription> response =
                await subscriptionService.ListAsync(new SubscriptionListOptions
                {
                    Limit = 30
                });
            return response;
        }

        //   step two  - create plan on product:"simple-market Vendor" [Sm Admin Dashboard ??]
        public Plan CreatePlan(ProdPlanVm vm)
        {
            var options = new PlanCreateOptions
            {
                Product = vm.ProductId,
                Nickname = vm.Nickname, //"sm vendorship USD",
                Interval = vm.Interval, //"yearly",
                Currency = "usd",
                Amount = vm.Amount
            };
            var service = new PlanService();
            return service.Create(options);
        }


        public async Task<Member> CreateStripeSharedCustomer(Member Member, Token Token)
        {
            // Platform key - customer created on platform
            var options = new CustomerCreateOptions
            {
                Email = Member.Email,
                Source = Token.Id,

            };
            var service = new CustomerService();
            Customer customer = await service.CreateAsync(options);
            if (customer == null)
                throw new Exception($"SmStripeService.CreateStripeCustomer fails for member id {Member.Id}");


            Member.StripeCustomerId = customer.Id;

            return Member;
        }

        public async Task<Subscription> SubscribeMemberToVendorshipPlan(string VendorshipPlanId,
                                                                        string CustomerId)
        {
            var items = new List<SubscriptionItemOptions>
            {
                new SubscriptionItemOptions {Plan = VendorshipPlanId}
            };

            var options = new SubscriptionCreateOptions
            {
                Items = items,
                Customer = CustomerId
            };

            var service = new SubscriptionService();
            Subscription subscription = await service.CreateAsync(options);

            return subscription;
        }


        public Token GetMemberChargeToken(string CustomerId, string CustomerAccountId)
        {
            //Making tokens
            //-------------------------------
            //  When you’re ready to create a charge on a connected account using a customer saved 
            //  on Sm platform account, create a new token for that purpose. 
            //  Need:
            //     - The Stripe account ID—something like acct_5Be23LrR9CajZbmp—of the connected account 
            //          for whom you’re making the charge
            //     - The customer ID—something like cus_WsV53JxavIqlrm—of the 
            //          customer(in your platform account) being charged
            //     - The card or bank account ID for that customer, if you want to charge a specific card 
            //          or bank account rather than the default

            // Set your secret key: remember to change this to your live secret key in production
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            

            var options = new TokenCreateOptions
            {
                Customer = CustomerId
            };
            var requestOptions = new RequestOptions
            {
                StripeAccount = CustomerAccountId
            };
            var service = new TokenService();
            return service.Create(options, requestOptions);

        }

        public Charge ExampleSaveCustomerToConnectedAccount(Token Token, string CustomerAccountId)
        {
            // You can save the customer to the connected account. 
            // This lets you charge customers, and create subscriptions for 
            // them(to a plan defined on the connected account).
            // To create a charge, save the customer to the connected account.

            //  transfer_group is required to connect parties to a charge [buyer and various vendors]
            //  sm OrderId may be ideal for this

            var options = new CustomerCreateOptions
            {
                Description = "Shared Customer",
                Source = Token.Id,

            };
            var requestOptions = new RequestOptions
            {

                // is this StripeConnectAccount the member or simple-market ??????
                StripeAccount = CustomerAccountId
            };
            var service = new CustomerService();
            Customer stripeCust = service.Create(options, requestOptions);

            // Then, use the customer ID(e.g., cus_M9YAwhWTkFhjmU) returned by the 
            // customers.create call to charge the customer.
            var options2 = new ChargeCreateOptions
            {
                Customer = stripeCust.Id,
                Amount = 999,
                Currency = "usd",
            };
            var requestOptions2 = new RequestOptions
            {
                StripeAccount = "{{CONNECTED_STRIPE_ACCOUNT_ID}}"
            };

            var service2 = new ChargeService();
            return service2.Create(options2, requestOptions2);
        }

        // Charge called only when vendor confirms product shipped
        //  - order transaction records one buyer purchase, may be multiple
        //     transactions as shopping cart supports multiple product purchase
        //     requests in one order. 
        public ResultAndMessage CreateCharge(SmChargeInfo data)
        {
            ResultAndMessage result = new ResultAndMessage();

            if (data.Tran.OrderItem.Price > 0)
            {
                //// no account balance monitoring until Mango

                // Testing
                if (data.BuyerMemberType == MemberType.Vendor)
                {
                    var chargeResult = ChargeAccountToAccount(data.BuyerStripeCustId,
                                                              data.VendorStripeAcctId,
                                                              data.Tran.TotalAmount,
                                                              data.Tran.OrderItem.SmFee);
                    if (!chargeResult.success)
                    {
                        result.message = $"SmTransService.ChargeAccount : Charge failed for Tran {data.Tran.Id}";
                        return result;
                    }
                    // credit tran.VendorId
                }
                if (data.BuyerMemberType == MemberType.Member)
                {
                    // TODO get Stripe key
                    string key = "adsfsdf";
                    var stripeResult = ChargeCustomerToAccount(data.BuyerStripeCustId,
                                                               data.VendorStripeAcctId,
                                                               key,
                                                               data.Tran.TotalAmount,
                                                               data.Tran.OrderItem.SmFee);
                    if (!stripeResult.success)
                    {
                        result.message += $"SmTransService.ChargeCard : Charge failed for Tran {data.Tran.Id}";
                        return stripeResult;
                    }
                }

                // interesting idea here of  tran history as a separate collection of records
                // tied by tranid to order tran.
                // Another approach might be to convert TranStateElement into an array of state-change entries

                //TranStatusHistory charge = new TranStatusHistory()
                //{
                //    DateCreated = DateTime.UtcNow,
                //    TranId = tran.TranId,
                //    tranEvent = OrderEventType.Charge,
                //    EventName = OrderEventType.Charge.ToString()

                //};
                //result.success = AddTranHistory(charge);
                return result;
            }
            return result;
        }

        public async Task<bool> AuthorizeCharge(string vendorAccount,
                                                string stripeBuyerId,
                                                int amount,
                                                int platformFees)
        {
            //  -- MUST BE CAPTURED WITHIN SEVEN DAYS FROM THIS AUTHORIZATION
            // charging (uncaptured) sm customer and routing payment, minus
            //  platform fees, to vendor
            var charges = new ChargeService();
            var charge = await charges.CreateAsync(new ChargeCreateOptions
            {
                Amount = amount,                            // product cost plus platform fees
                Currency = "usd",
                Description = "Example charge",
                Capture = false,                            // this false value is the 'authorize only' flag
                Customer = stripeBuyerId,
                Destination = new ChargeDestinationCreateOptions() { Account = vendorAccount, Amount = (amount - platformFees) }             //vendorID,
                                                                                                                                             // Destination Amount =    //s/b fee to vendor after subtracting sm fees
            });
            return (charge != null);
        }

        public async Task<bool> CaptureAuthorizedCharge(string stripeAuthChargeId, int amount)
        {
            var opts = new ChargeCaptureOptions() { Amount = amount };
            var chargeService = new ChargeService();
            Charge charge = await chargeService.CaptureAsync(stripeAuthChargeId, opts);
            return (charge != null);
        }

        public ResultAndMessage ChargeAccountToAccount(string BuyerCustId,
                                                       string VendorAcctId,
                                                       decimal purchaseAmount,
                                                       decimal smFee)
        {
            ResultAndMessage result = new ResultAndMessage();

            var charge = new ChargeCreateOptions
            {

                //  What is this property?
                //----------------------------------------
                //charge.OnBehalfOf = stripeAcctId;
                //-----------------------------------
                //   When you create destination charges on the platform account, 
                //  Stripe automatically:
                //     -Settles charges in the country of the specified account, 
                //        thereby minimizing declines
                //     -Uses the connected account’s currency for settlement, 
                //        often avoiding currency conversions
                //     -Uses the fee structure for the connected account’s country
                // This same functionality is not automatically provided when creating 
                //   separate charges and transfers, but you can replicate it using 
                //   the on_behalf_of attribute when creating the charge.
                //   Setting on_behalf_of defines the business of record for the charge 
                //   and funds will be settled using that account.

                //  For Custom Accounts, simplest route 
                //  create the charge on platform’s account
                //  destination parm - connected account that receives funds.
                Destination = new ChargeDestinationCreateOptions() { Account = VendorAcctId },

                // always set these properties
                Amount = purchaseAmount.ToPennies(),
                Currency = "usd",           

                //  two-step payments: you can first authorize a charge 
                //  on your customer’s card, then wait to settle (capture) 
                //  it later. When a charge is authorized, the funds are guaranteed 
                //  by the issuing bank and the amount held on the customer’s card 
                // for up to seven days. If the charge is not captured within this time, 
                // the authorization is canceled and funds released.
                // To authorize a payment without capturing it, make a charge request 
                // that also includes the capture parameter with a value of false.
                // This instructs Stripe to only authorize the amount 
                // on the customer’s card.

                //charge.Capture = false;

                // Here's second step if two-step is employed
                //var chargeService = new StripeChargeService();
                //charge = chargeService.Capture({ CHARGE_ID});

                // set this if you want to
                Description = "simple-market purchase",

                //charge.SourceTokenOrExistingSourceId = BuyerAcctId;

                // set this property if using a customer 
                // - this MUST be set if you are using an existing source!
                Customer = BuyerCustId,
                ApplicationFeeAmount = smFee.ToPennies()
            };

            //string secKey = _cntxt.smAdmin.Single(r => r.type == AdminRecType.secKey).strData;
            //if (string.IsNullOrEmpty(secKey))
            //{
            //    result.message = "failure to fetch stripe key.";
            //    return result;
            //}
            //StripeConfiguration.SetApiKey(secKey);

            var chargeService = new ChargeService();
            Charge stripeCharge = chargeService.Create(charge);

            result.success = true;

            return result;
        }

        public ResultAndMessage ChargeCustomerToAccount(string BuyerCustId,
                                                        string VendorAcctId,
                                                        string StripeKey,
                                                        decimal purchaseAmount,
                                                        decimal smFee)
        {
            ResultAndMessage result = new ResultAndMessage();

            // setting up the card
            var charge = new ChargeCreateOptions
            {
                // always set these properties ( ??? old Stripe comments - do we put amount in two places now??[see just below]
                Amount = purchaseAmount.ToPennies(),
                Currency = "usd",

                Description = "simple-market purchase",

                // set this property if using a customer
                Customer = BuyerCustId,

                // fund receiver
                Destination = new ChargeDestinationCreateOptions() { Account = VendorAcctId, Amount = purchaseAmount.ToPennies() },

                // set this if you have your own application fees 
                //   (you must have your application configured first within Stripe)

                ApplicationFeeAmount = smFee.ToPennies()
            };

            var chargeService = new ChargeService();
            Charge stripeCharge = chargeService.Create(charge);

            result.success = true;

            return result;
        }

        public async Task<ResultAndMessage> CreateContribution(ContributeVm vm)
        {
            ResultAndMessage result = new ResultAndMessage();

            int amt = vm.Amount * 100;
            var customers = new CustomerService();
            var charges = new ChargeService();

            var customer = await customers.CreateAsync(new CustomerCreateOptions
            {
                Email = vm.StripeEmail,
                Source = vm.StripeToken
            });

            if (customer != null)
            {
                var charge = await charges.CreateAsync(new ChargeCreateOptions
                {
                    Amount = amt,
                    Description = "MasterMind Contribution",
                    Currency = "usd",
                    Customer = customer.Id
                });
                result.exData = customer.Id;
                result.success = (charge != null);
            }
            return result;
        }

        public async Task<Customer> CreateMember(string MemberEmail, string stripeToken)
        {
            // This stores customer on sm platform account 

            var service = new CustomerService();
            Customer customer = await service.CreateAsync(new CustomerCreateOptions
            {
                Email = MemberEmail,
                Source = stripeToken,
            });
            return customer;
        }

        public async Task<Member> CreateStripeAccountOnPlatform(Member Member, string BusinessName)
        {
            var accountOptions = new AccountCreateOptions()
            {
                Email = Member.Email,
                Type = AccountType.Custom,
                Country = "US",
                BusinessType = "individual",
                RequestedCapabilities= new List<string>() { "platform_payments"},
                Individual = new PersonCreateOptions
                {
                    FirstName = Member.FirstName,
                    LastName = Member.LastName,
                    IdNumber = Member.SSNum,
                    Email = Member.Email,
                    Address = new AddressOptions()
                    {
                        City = Member.City,
                        Line1 = Member.AddrLine,
                        Line2 = Member.AddrLine2 ?? string.Empty,
                        PostalCode = Member.Zip,
                        State = Member.State,
                        Country = "US"
                    },
                    Dob = new DobOptions
                    {
                        Day = Member.BirthDay,
                        Month = Member.BirthMonth,
                        Year = Member.BirthYear
                    },
                },
                BusinessProfile=new AccountBusinessProfileOptions
                {
                    Name = BusinessName,
                     Url= "https://simple-market.com"

                },
                 TosAcceptance= new AccountTosAcceptanceOptions
                 {
                     Date= Member.StripeTosDate,
                      Ip= Member.StripeTosIp
                 }
            };

            var accountService = new AccountService();
            Account account = await accountService.CreateAsync(accountOptions);

            if (account != null)
            {
                Member.StripeAccountId = account.Id;
                //Member.StripeRequestDate = account.StripeResponse.RequestDate;
                Member.StripeRequestId = account.StripeResponse.RequestId;
                // Stripe has removed this and asks for platform key
                //  instead. 4.9.19
               // Member.StripeAccountPubKey = account.Keys.Publishable;
               // Member.StripeAccountSecKey = account.Keys.Secret;
            }
            return Member;
        }

        public async Task<Account> AddBankToAccount(Member Member, 
                                                    string BusinessName,
                                                    string BankAccount, 
                                                    string RoutingNumber)
        {
            if (Member is null || Member.StripeAccountId is null)
                throw new Exception($"SmStipeService.TestAddBankToAccount : Bad input parameters on member : {Member.Id}");

            // retrieve account
            var service = new AccountService();
            Account account = service.Get(Member.StripeAccountId);
            //  add external_account

            // update account and then Member
            var options = new AccountUpdateOptions
            {
                ExternalAccount = new AccountBankAccountOptions()
                {
                    AccountHolderName = BusinessName,
                    AccountHolderType = "individual",
                    AccountNumber = BankAccount,
                    RoutingNumber = RoutingNumber,
                    Country = "US",
                    Currency = "usd"
                }
            };

            account = await service.UpdateAsync(Member.StripeAccountId, options);

            return account;
        }


        //public async Task<Member> AddBankToAccount(Member Member, Token token)
        //{

        //    //NOTE 2.5  failing with StripeException: Missing required param: external_account.
        //    if (Member is null || Member.StripeAccountId is null || Member.StripeAccountSecKey is null)
        //        throw new Exception($"SmStipeService.AddBankToAccount : Bad input parameters on member : {Member.Id}");

        //    

        //    var options = new ExternalAccountCreateOptions
        //    {
        //        ExternalAccountTokenId = token.Id,
        //        //ExternalAccountBankAccount = new AccountBankAccountOptions()
        //        //{
        //        //    AccountHolderName = Member.BusinessName,
        //        //    AccountHolderType = "individual",
        //        //    AccountNumber = BankAccount,
        //        //    RoutingNumber = RoutingNumber,
        //        //    Country = "US",
        //        //    Currency = "usd"
        //        //},
                 
        //    };

        //    var service = new ExternalAccountService();
        //    var bankAccount = await service.CreateAsync(Member.StripeAccountId, options);

        //    if (bankAccount is null || bankAccount.Id is null)
        //        throw new Exception($"SmStipeService.AddBankToAccount : Add bank account fails for member : {Member.Id}");

        //    Member.StripeBankAccountId = bankAccount.Id;
        //    return Member;
        //}


        // NOTE: switched to CreateAccountAddCard because of following....
        //Error(1.31.19 after upgrading to latest Stripe): 
        // StripeException: This account can only be updated with an account token, because it was
        // originally created with an account token. (Attempted to update param 'account_token' directly.)

        //public async Task<Account> CreateAccount(Member Member,
        //                                               string token)
        //{
        //    

        //    var accountOptions = new AccountCreateOptions()
        //    {
        //        AccountToken=token,

        //        Email = Member.Email,
        //        Type = AccountType.Custom,
        //        Country = "US",
        //        LegalEntity = new AccountLegalEntityOptions
        //        {
        //             Dob=new AccountDobOptions()
        //             {
        //                  Day=Member.BirthDay,
        //                  Month=Member.BirthMonth,
        //                  Year=Member.BirthYear
        //             },
        //              Address=new AddressOptions()
        //              {
        //                  City = Member.Address.City,
        //                  Line1 = Member.Address.AddrLine,
        //                  Line2 = Member.Address.AddrLine2 ?? string.Empty,
        //                  PostalCode = Member.Address.Zip,
        //                  State = Member.Address.State,
        //                  Country= "US"
        //              },

        //            FirstName = Member.FirstName,
        //            LastName = Member.LastName,
        //            BusinessName = Member.BusinessName,
        //            PersonalIdNumber=Member.SSNum,
        //            Type = "individual"  // or 'company' 
        //        },
        //        TosAcceptance=new AccountTosAcceptanceOptions() {  Date= Member.StripeTosDate, Ip= Member.StripeTosIp },
        //        BusinessName = Member.BusinessName,
        //        BusinessUrl = "https://simple-market.com"
        //    };
        //    var accountService = new AccountService();
        //    return await accountService.CreateAsync(accountOptions);
        //}

        public ResultAndMessage CreditCardPurchase(string vendorCustId,
                                                   decimal purchaseAmount,
                                                   decimal smFee,
                                                   string stripeToken)
        {
            var result = new ResultAndMessage();

            int amt = (int)(purchaseAmount * 100);

            var chargeOptions = new ChargeCreateOptions()
            {
                Amount = amt,
                Currency = "usd",
                Description = "Charge for natalie.martin@example.com",
                // NOTE comment out because latest stripe change - don't know what to do ???
                //SourceTokenOrExistingSourceId = stripeToken,
                Destination = new ChargeDestinationCreateOptions() { Account = vendorCustId, Amount = 0 }
            };
            var chargeService = new ChargeService();
            Charge charge = chargeService.Create(chargeOptions);

            return result;
        }

        public async Task<SmAdminVm> GetAdminInfo()
        {
            SmAdminVm vm = new SmAdminVm
            {
                // this call requires an account id input not a key
                //Balance = await GetBalanceFor(SD.StripeTestSecretKey),
                Products = await ListProducts(),
                Plans = await ListPlans(),
                Accounts = await ListAccounts(),
                Customers = await ListCustomers(),
                Subscriptions = await ListSubscriptions(),
                Charges = await ListCharges(),
                BalanceHistory = await GetBalanceHistoryFor(this.options.Value.SecKey)
            };

            // testing api lib
            //vm.Members = await GetSmMembers();

            return vm;
        }

        public void UpdateAdminInfo(StripeVendorSubscription vm)
        {
            throw new NotImplementedException();
        }


    }
}
