using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using AmazonS3Microservice.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserMicroservice.Data;

namespace AmazonS3Microservice
{
    public class Startup
    {

        public IConfiguration Configuration { get; }

      

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
        }

       
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AmazonS3Microservice", Version = "v1" });
            });
            services.AddDbContextPool<AppDbContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("S3Conn")));
            services.AddTransient<IAmazonS3Repo, AmazonS3Repo>();

            services.AddDefaultAWSOptions(new AWSOptions
            {
                Credentials = new BasicAWSCredentials("AKIAVPP2S62KJMSUVAOO", "oEdQfwiyPeD2OJ6FtvBEPD0jGu99eCD84l184ow/"),
                Region = Amazon.RegionEndpoint.EUCentral1
            });

            services.AddAWSService<IAmazonS3>();
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AmazonS3Microservice v1"));
            }
            app.UseCors(options =>
           options.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            PrepDb.PrepPopulation(app, env.IsProduction());
        }
    }
}
