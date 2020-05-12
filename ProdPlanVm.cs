using System.ComponentModel.DataAnnotations;

namespace SmWeb.ViewModels
{
    public class ProdPlanVm
    {
        public string ProductName { get; set; }
        public string ProductId { get; set; }

        public string Nickname { get; set; }
        [Required]
        public string Interval { get; set; }
        [Required]
        [Range(100, int.MaxValue, ErrorMessage = "Please enter value for amount")]
        public int Amount { get; set; }  //pennies
    }
}
