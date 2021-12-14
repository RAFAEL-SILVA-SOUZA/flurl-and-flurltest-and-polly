using FluentAssertions;
using Flurl.Http.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Net.Http;

namespace ClientFlurl.Tests.IntegrationTest
{
    public abstract class IntegrationTestBase<TStartup, TIService> where TStartup : class
                                                                   where TIService : class
    {
        private TestServer _server;
        protected HttpClient _client;
        protected IFlurlClientFactory _flurlClientFactory;

        protected ILogger<TIService> _logger;

        public IntegrationTestBase()
        {
            _flurlClientFactory = Substitute.For<IFlurlClientFactory>();
            _logger = Substitute.For<ILogger<TIService>>();
        }

        protected void CreateServer()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Test")
                .ConfigureTestServices(services =>
                {
                    services.RemoveAll<IFlurlClientFactory>();
                    services.RemoveAll<ILogger<TIService>>();
                    services.TryAddScoped(x => _flurlClientFactory);
                    services.TryAddScoped(x => _logger);
                })
                .UseStartup<TStartup>());


            //Act
            _client = _server.CreateClient();

            //Assert
            _server.Should().NotBeNull();
            _client.Should().NotBeNull();
        }
    }
}
