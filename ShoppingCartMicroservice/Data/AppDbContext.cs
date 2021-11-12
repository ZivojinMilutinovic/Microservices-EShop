using Microsoft.EntityFrameworkCore;
using ShoppingCartMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ShoppingCartMicroservice.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }

        public DbSet<Product> Product { get; set; }

        public DbSet<ShoppingCart> ShoppingCart { get; set; }
    }

}
