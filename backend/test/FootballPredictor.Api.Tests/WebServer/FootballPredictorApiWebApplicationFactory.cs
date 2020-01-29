using FootballPredictor.Data.InMemory;
using FootballPredictor.Domain.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

// https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-3.1#customize-webapplicationfactory
namespace FootballPredictor.Api.Tests.WebServer
{
    public class FootballPredictorApiWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

            builder.ConfigureServices(services =>
            {
                // Remove the app's ITeamsDbRepository registration.
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ITeamsDbRepository));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add ITeamsDbRepository using an in-memory database for testing.
                services.AddSingleton<ITeamsDbRepository, TeamsInMemoryDbRepository>();
            });
        }
    }
}
