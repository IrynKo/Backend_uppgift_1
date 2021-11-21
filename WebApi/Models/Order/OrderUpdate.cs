using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class OrderUpdate
    {
      
        public int Id { get; set; }
       
        public decimal Amount { get; set; }

        public DateTime? DateCreated { get; set; } = DateTime.Now;

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        
    }
}
