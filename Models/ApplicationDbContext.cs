using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Bookstore_Group6.Models;
using System.Reflection.Emit;

namespace Bookstore_Group6.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() :
        base("Bookstore")
        { }

        public DbSet<Bookstore_Group6.Models.Books> Books { get; set; }
        public DbSet<Bookstore_Group6.Models.Clients> Clients { get; set; }
        public DbSet<Bookstore_Group6.Models.Transactions> Transactions { get; set; }
        public DbSet<BuyerBorrower> BuyerBorrowers { get; set; }
        public DbSet<SearchHistory> SearchHistories { get; set; }


    }
}
