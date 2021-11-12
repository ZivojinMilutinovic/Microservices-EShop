using InventoryMicroservice.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace InventoryMicroservice.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }

        public DbSet<Inventory> Inventory { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<Supplier> Supplier { get; set; }
    }

}
