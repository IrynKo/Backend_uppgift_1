using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class OrderDetailsUpdate
    {
   
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

   
        public int Quantity { get; set; }

        public decimal Price { get; set; }

       
       
    }
}
