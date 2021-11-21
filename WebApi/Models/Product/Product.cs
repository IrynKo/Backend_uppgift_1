using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        
        [Column(TypeName = "nvarchar(100)")]
        public string Title { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(1000)")]
        public string Image { get; set; }


        [Column(TypeName = "money")]
        public decimal Price { get; set;}

        public DateTime? DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateUpdated { get; set; }
        
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }



    }
}
