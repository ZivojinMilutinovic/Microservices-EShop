using Microsoft.EntityFrameworkCore;
using ShippingMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingMicroservice.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }

        public DbSet<CreditCard> CreditCard { get; set; }

        public DbSet<ShippingDetails> ShippingDetails { get; set; }
    }
}
