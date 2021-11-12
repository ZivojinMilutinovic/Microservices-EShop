using AmazonS3Microservice.Data;
using AmazonS3Microservice.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserMicroservice.Data
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
                if (!context.ProductImages.Any())
                {
                    Console.WriteLine("Seeding database...");
                    context.ProductImages.Add(new ProductImage() { PictureUrl = "sweater1.jpg",ProductId=1 });
                    context.ProductImages.Add(new ProductImage() { PictureUrl = "dress1.jpg", ProductId = 2 });
                    context.ProductImages.Add(new ProductImage() { PictureUrl = "tshirt1.jpg", ProductId = 3 });
                    context.ProductImages.Add(new ProductImage() { PictureUrl = "shorts1.jpg", ProductId = 4 });
                    context.ProductImages.Add(new ProductImage() { PictureUrl = "skirt1.jpg", ProductId = 5 });
                    context.ProductImages.Add(new ProductImage() { PictureUrl = "jeans1.jpg", ProductId = 6 });
                    context.ProductImages.Add(new ProductImage() { PictureUrl = "shoes1.jpg", ProductId = 7 });
                    context.ProductImages.Add(new ProductImage() { PictureUrl = "coat1.jpg", ProductId = 8 });
                    context.ProductImages.Add(new ProductImage() { PictureUrl = "suit1.jpg", ProductId = 9 });
                    context.ProductImages.Add(new ProductImage() { PictureUrl = "cap1.jpg", ProductId = 10 });
                    context.ProductImages.Add(new ProductImage() { PictureUrl = "jacket1.jpg", ProductId = 11 });
                    context.ProductImages.Add(new ProductImage() { PictureUrl = "tie1.jpg", ProductId = 12 });
                    context.ProductImages.Add(new ProductImage() { PictureUrl = "socks1.jpg", ProductId = 13 });
                    context.ProductImages.Add(new ProductImage() { PictureUrl = "jeans2.jpg", ProductId = 14 });
                    context.ProductImages.Add(new ProductImage() { PictureUrl = "tshirt2.jpg", ProductId = 15 });
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
