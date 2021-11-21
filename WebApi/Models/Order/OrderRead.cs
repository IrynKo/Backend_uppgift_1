using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class OrderRead
    {
        
        public int Id { get; set; }

       
        public int UserId { get; set; }

       
        public decimal Amount { get; set; }

        public DateTime? DateCreated { get; set; } = DateTime.Now;

        public int  OrderDetailId { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }


    }
}
