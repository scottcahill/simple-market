
using System;
using System.ComponentModel.DataAnnotations;

namespace SmWeb.Models
{
  
    public class Member
    { 
       
        public string Id { get; set; }

        [Required]
        public string Email { get; set; }

        //[Required]
        public string Password { get; set; }

        public string Role { get; set; }

        [Required, StringLength(30)]
        public string FirstName { get; set; }

        [Required, StringLength(30)]
        public string LastName { get; set; }

        [Required, StringLength(60)]
      
        public string AddrLine { get; set; }

     
        public string AddrLine2 { get; set; }

        
        public string SSNum { get; set; }

        public int BirthDay { get; set; }

        public int BirthMonth { get; set; }

        public int BirthYear { get; set; }

        //[Required, StringLength(20)]
        public string City { get; set; }
        [Required, StringLength(10)]
        public string Zip { get; set; }
        public string State { get; set; }
        [Required, StringLength(12)]
        public string CellNumber { get; set; }

        public DateTime DateCreated { get; set; }

        public MemberType MemberType { get; set; }

        // Stripe ------------
        public string StripeCustomerId { get; set; }

        public string StripeAccountId { get; set; }

        public string StripeBankAccountId { get; set; }

        public string StripeSubscriptionId { get; set; }

        public long SubscriptionAmountInCents { get; set; }

        public DateTime? SubscriptionPurchaseDate { get; set; }
        public DateTime? SubscriptionExpireDate { get; set; }

        public string StripeRequestId { get; set; }

        public string StripeTosIp { get; set; }

        public DateTime? StripeTosDate { get; set; }  // seconds

        public string CosmosEntityName { get; set; }
    }
}
