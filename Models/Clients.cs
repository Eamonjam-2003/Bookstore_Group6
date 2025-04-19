using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bookstore_Group6.Models
{
    [Table("Client")]
    public class Clients
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }


        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        public int? BookId { get; set; }

        [ForeignKey("BookId")]
        public virtual Books Book { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(255)]
        public string Password { get; set; }

    }
}