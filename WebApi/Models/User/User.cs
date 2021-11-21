using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 2, ErrorMessage = "FirstName must be between {1} and {2} character in length.")]
        [Column(TypeName = "nvarchar(100)")]
       
        public string FirstName { get; set; }

      
        [StringLength(100, MinimumLength = 2, ErrorMessage = "LastName must be between {1} and {2} character in length.")]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }

       
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
         ErrorMessage = "Invalid Email")]
        [Column(TypeName = "nvarchar(200)")]
        public string Email { get; set; }

       
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$",
         ErrorMessage = "Password must be at least eight characters long, at least one letter and one number")]
        [Column(TypeName = "nvarchar(200)")]
        public string Password { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

    }
}
