using System.ComponentModel.DataAnnotations;

namespace SmWeb.ViewModels
{
    public class ContributeVm
    {
        [Required]
        [Range(1, 99999, ErrorMessage = "Amount must be at least 1")]
        public int Amount { get; set; }

        public int ConfirmNum { get; set; }

        public bool SendPdf { get; set; }
        public bool GetsHardcopy { get; set; }
        public string ErrorMsg { get; set; }

        public string StripeEmail { get; set; }
        public string StripeToken { get; set; }
        public string PublishKey { get; set; }

        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }
}
