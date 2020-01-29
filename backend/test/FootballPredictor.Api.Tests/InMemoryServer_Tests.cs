using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FootballPredictor.Api.Tests.WebServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace FootballPredictor.Api.Tests
{
    public class InMemoryServer_Tests : IClassFixture<FootballPredictorApiWebApplicationFactory<Startup>>
    {
        private readonly FootballPredictorApiWebApplicationFactory<Startup> _factory;
        private HttpClient _client;
        private const string AUTH_SCHEME_NAME = "Test";

        public InMemoryServer_Tests(FootballPredictorApiWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            
            ConfigureHttpClient();
        }

        [Fact]
        public async Task Given_test_web_host_will_response_to_http_get_request()
        {
            var response = await _client.GetAsync("/team");

            // Checks
            response.ShouldNotBeNull();
            response.IsSuccessStatusCode.ShouldBeTrue();
        }

        #region Private Methods
        private void ConfigureHttpClient()
        {
            _client = _factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        // Mock an AuthenticationHandler to test aspects of authentication and authorization
                        services.AddAuthentication(AUTH_SCHEME_NAME)
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(AUTH_SCHEME_NAME, options => { });
                    });
                })
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false,
                });
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AUTH_SCHEME_NAME);
        } 
        #endregion
    }
}
