using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore_Group6.Models
{
    [Table("Book")]
    public class Books
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Author { get; set; }

        [StringLength(255)]
        public string Publisher { get; set; }

        [StringLength(20)]
        public string ISBN { get; set; }

        public decimal? Price { get; set; }

        public int? Pages { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [DefaultValue(false)]
        public bool IsBorrowed { get; set; } // To check if the book is currently borrowed

        [DefaultValue(false)]
        public bool IsBought { get; set; }   // To check if the book is already bought

        [StringLength(255)]
        public string ImagePath { get; set; }

    }
}
