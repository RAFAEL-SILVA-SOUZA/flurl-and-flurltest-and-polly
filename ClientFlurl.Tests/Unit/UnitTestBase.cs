using ClientFlurl.Domain.Services;
using ClientFlurl.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Net.Http;

namespace ClientFlurl.Tests.Unit
{
    public abstract class UnitTestBase<TIService, TService> : TestBase
    {
        protected AppSettings _appSettings;
        protected TIService _viaCepClient;
        protected ILogger<TService> _logger;
        protected NotificationContext _notificationContext;

        public UnitTestBase()
        {
            var baseUri = "http://fake.com/";
            var pollyRetryStatusCodes = new int[] { 400, 408, 503, 504 };

            var client = Substitute.For<HttpClient>();
            client.BaseAddress = new Uri(baseUri);

            _logger = Substitute.For<ILogger<TService>>();
            _notificationContext = new NotificationContext();
            _appSettings = new AppSettings
            {
                BaseUrl = baseUri,
                PollyRetryStatusCodes = pollyRetryStatusCodes,
                PollyRetryCount = 1
            };
            _viaCepClient = (TIService)Activator.CreateInstance(typeof(TService), _logger, _appSettings, client, _notificationContext);

            _appSettings.Should().NotBeNull();
            _logger.Should().NotBeNull();
            _appSettings.Should().NotBeNull();
            _viaCepClient.Should().NotBeNull();
        }
    }
}
