using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductMicroservice.Data;
using ProductMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMicroservice.Data
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
                if (!context.Products.Any())
                {
                    Console.WriteLine("Seeding database...");
                    context.Products.Add(new Product() {ProductNumber="1", Price=10.0,Description="Warm sweater",ClothingType="Sweater",Collection="Men"});
                    context.Products.Add(new Product() { ProductNumber = "2", Price = 15.0, Description = "Cosy dress for a women", ClothingType = "Dress", Collection = "Women" });
                    context.Products.Add(new Product() { ProductNumber = "3", Price = 20.0, Description = "Kids T-Shirt", ClothingType = "T-Shirt", Collection = "Kids" });
                    context.Products.Add(new Product() { ProductNumber = "4", Price = 30.0, Description = "Practical sports shorts", ClothingType = "Shorts", Collection = "Men" });
                    context.Products.Add(new Product() { ProductNumber = "5", Price = 40.0, Description = "Elegant skirt for every ocassion", ClothingType = "Skirt", Collection = "Women" });
                    context.Products.Add(new Product() { ProductNumber = "6", Price = 18.0, Description = "Tight Jeans", ClothingType = "Jeans", Collection = "Women" });
                    context.Products.Add(new Product() { ProductNumber = "7", Price = 22.0, Description = "Fancy shoes", ClothingType = "Shoes", Collection = "Kids" });
                    context.Products.Add(new Product() { ProductNumber = "8", Price = 35.0, Description = "Coat for rain", ClothingType = "Coat", Collection = "Men" });
                    context.Products.Add(new Product() { ProductNumber = "9", Price = 58.0, Description = "Elegant suit", ClothingType = "Suit", Collection = "Men" });
                    context.Products.Add(new Product() { ProductNumber = "10", Price = 99.0, Description = "Cool cap", ClothingType = "Cap", Collection = "Kids" });
                    context.Products.Add(new Product() { ProductNumber = "11", Price = 82.0, Description = "Motorcycle jacket", ClothingType = "Jacket", Collection = "Men" });
                    context.Products.Add(new Product() { ProductNumber = "12", Price = 74.0, Description = "Tie for a die", ClothingType = "Tie", Collection = "Men" });
                    context.Products.Add(new Product() { ProductNumber = "13", Price = 32.0, Description = "Warm socks", ClothingType = "Socks", Collection = "Kids" });
                    context.Products.Add(new Product() { ProductNumber = "14", Price = 12.0, Description = "Elegant jeans", ClothingType = "Jeans", Collection = "Women" });
                    context.Products.Add(new Product() { ProductNumber = "15", Price = 100.0, Description = "T-shirt for her", ClothingType = "T-shirt", Collection = "Women" });
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
