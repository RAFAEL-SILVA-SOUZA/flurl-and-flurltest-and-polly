using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;

namespace ClientFlurl.Tests.IntegrationTest
{
    public abstract class IntegrationTestBase<TStartup> : BaseTest where TStartup : class
    {
        private readonly TestServer _server;
        protected readonly HttpClient _client;

        public IntegrationTestBase()
        {
            // Arrange
            var statupDirectory = AssemblyDirectory;
            _server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseContentRoot(statupDirectory)
                .UseConfiguration(new ConfigurationBuilder()
                    .SetBasePath(statupDirectory)
                    .AddJsonFile("appsettings.Test.json")
                    .Build()
                )
                .UseStartup<TStartup>());

            _client = _server.CreateClient();

            _server.Should().NotBeNull();
            _client.Should().NotBeNull();
        }

        private static string AssemblyDirectory
        {
            get
            {
                string codeBase = typeof(TStartup).Assembly.Location;
                var uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

    }
}
