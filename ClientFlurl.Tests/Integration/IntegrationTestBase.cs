using ClientFlurl.Domain.Services;
using ClientFlurl.Domain.Services.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Net.Http;

namespace ClientFlurl.Tests.Integration
{
    public abstract class IntegrationTestBase<TStartup, TIService, TService>: TestBase where TStartup : class
                                                                                       where TIService : class
                                                                                       where TService : class
    {
        private TestServer _server;
        protected HttpClient _client;
        protected TIService _service;

        protected ILogger<TService> _logger;
        protected NotificationContext _notificationContext;

        public IntegrationTestBase()
        {
            _service = Substitute.For<TIService>();
            _logger = Substitute.For<ILogger<TService>>();
            _notificationContext = Substitute.For<NotificationContext>();
        }

        protected void CreateServer()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Test")
                .ConfigureTestServices(services =>
                {
                    services.RemoveAll<ILogger<TService>>();
                    services.TryAddScoped(x => _logger);

                    services.RemoveAll<NotificationContext>();
                    services.TryAddScoped(x => _notificationContext);

                    services.RemoveAll<TIService>();
                    services.RemoveAll<TService>();
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
