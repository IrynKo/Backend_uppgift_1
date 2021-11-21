using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class CreateProduct
    {
        [Required]
        public string Title { get; set; }

       
        public string Description { get; set; }

        [Required]
        public string Image { get; set; } = "https://via.placeholder.com/300x300";

        [Required]
        public decimal Price { get; set; }


    }
}
