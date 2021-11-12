using InventoryMicroservice.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMicroservice.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }
        private static void SeedData(AppDbContext context, bool isProd)
        {
            try
            {
                context.Database.Migrate();

                if (!context.Inventory.Any())
                {
                    context.Inventory.Add(new Inventory
                    {
                        Quantity=0
                    });
                    context.SaveChanges();

                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"-->Could not run migrations: {ex.Message}");
            }
        }
    }
}
