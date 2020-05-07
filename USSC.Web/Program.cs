using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using USSC.Services;
using USSC.Services.SeedDataService;

namespace USSC.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();
                try
                {
                    logger.LogInformation("Running application");
                    var seed = services.GetRequiredService<ISeedData>();
                    seed.Initialise();
                }
                catch (Exception ex)
                {
                    logger.LogCritical("Error creating/seeding database - " + ex.Message, ex);
                }
            }    
                
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .ConfigureLogging(builder => builder.AddConsole().AddFile());
                });
    }
}
