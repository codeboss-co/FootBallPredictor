using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace FootballPredictor.Api
{
    public class Program
    {
        private static readonly string ServiceName = typeof(Program).Namespace;

        public static void Main(string[] args)
        {
            Console.Title = ServiceName;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                // Filter out ASP.NET Core infrastructre logs that are Information and below
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                // .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();

            try
            {
                Log.Information($"{ServiceName} Starting up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                // ASP.NET Core 3.0+:
                // The UseServiceProviderFactory call attaches the
                // Autofac provider to the generic hosting mechanism.
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
