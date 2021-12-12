using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;

namespace ClientFlurl.Tests.IntegrationTest
{
    public abstract class IntegrationTestBase<TStartup> : TestBase where TStartup : class
    {
        private readonly TestServer _server;
        protected readonly HttpClient _client;

        public IntegrationTestBase()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Test")
                .UseStartup<TStartup>());

            //Act
            _client = _server.CreateClient();

            //Assert
            _server.Should().NotBeNull();
            _client.Should().NotBeNull();
        }
    }
}
