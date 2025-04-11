// Models/Login.cs
using System.ComponentModel.DataAnnotations;

namespace Bookstore_Group6.Models
{
    public class Login
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
