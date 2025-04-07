using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bookstore_Group6.Models
{
    public class BuyerBorrower
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string ClientType { get; set; } // "Buyer" or "Borrower"

        [Required, DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; }

        public string Password { get; set; }

    }
}
