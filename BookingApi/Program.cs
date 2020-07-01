using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using BookingApi.Data;
using Microsoft.Extensions.DependencyInjection;

namespace BookingApi
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var host = CreateHostBuilder(args).Build();

            // Initialize Db
            InitializeDb(host);

            host.Run();
        }


        public static void InitializeDb(IHost host) 
        {
            using (var scope = host.Services.CreateScope()) 
            {
                var services = scope.ServiceProvider;

                try 
                {
                    var context = services.GetRequiredService<BookingContext>();
                    DbInitializer.Initialize(context);
                } 
                catch (Exception ex) 
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
