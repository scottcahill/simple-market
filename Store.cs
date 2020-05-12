using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmWeb.Models
{
    public class Store
    { 

        public string Id { get; set; }

        [Required, StringLength(80)]
        [Display(Name = "Business Name")]
        public string BusinessName { get; set; }

        public string ImgPath { get; set; }

        [Display(Name = "Vendor Id")]
        public string VendorId { get; set; }

        [Required]
        public string RoutingNumber { get; set; }
        [Required]
        public string BankAccount { get; set; }

        public DateTime DateCreated { get; set; }

        public IEnumerable<string> ProductIdList { get; set; }

    }
}
