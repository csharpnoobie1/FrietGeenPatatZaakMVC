using Microsoft.EntityFrameworkCore;
using FrietGeenPatatZaakMVC.Models;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ClassLibrary1.Models;
using FrietGeenPatatZaakMVC.Controllers.API;
using System.Text.Json.Serialization;

namespace FrietGeenPatatZaakMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddScoped<ProductsAPIController>();


            builder.Services.AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });


            // Add services to the container
            builder.Services.AddControllersWithViews();
            // Add database context
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<FrietGeenPatatZaakContext>(options =>
                options.UseSqlServer(connectionString));

            // Add Swagger services
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "FrietGeenPatatZaakMVC API",
                    Version = "v1",
                    Description = "API voor het beheren van producten en bestellingen bij FrietGeenPatatZaakMVC",
                    Contact = new OpenApiContact
                    {
                        Name = "Support Team",
                        Email = "support@frietgeenpatatzaak.com"
                    }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();  // Enable detailed error page during development
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FrietGeenPatatZaakMVC API v1");
                    c.RoutePrefix = "swagger";
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            // API routes
            app.MapControllers();

            // MVC routes
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

    }
}
