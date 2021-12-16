using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http;

namespace ClientFlurl.Tests.IntegrationTest
{
    public abstract class IntegrationTestBase<TStartup, TIService> : TestBase where TStartup : class
                                                                              where TIService : class
    {
        private TestServer _server;
        protected HttpClient _client;
        protected TIService _service;

        public IntegrationTestBase()
        {
            _service = NSubstitute.Substitute.For<TIService>();
        }

        public void CreateServer()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Test")
                .ConfigureTestServices(services =>
                {
                    services.RemoveAll<TIService>();
                    services.TryAddScoped(x => _service);
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
