using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmWeb.Models
{
    public class Product
    {
        public string Id { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; }

        [Required, StringLength(240)]
        public string Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Price should be greater than $1")]
        public double Price { get; set; }

        public string ImgPath { get; set; }
        [Display(Name = "Category Type")]
        public string CategoryId { get; set; }
        [NotMapped]
        public string CategoryName { get; set; }

        public string CosmosEntityName { get; set; }
    }
}
