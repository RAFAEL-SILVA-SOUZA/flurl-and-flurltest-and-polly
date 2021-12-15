using ClientFlurl.Domain.Services;
using ClientFlurl.Domain.Services.Contracts;
using ClientFlurl.Entities;
using FluentAssertions;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;

namespace ClientFlurl.Tests.Unit
{
    public abstract class UnitTestBase<TIService, TService> : TestBase
    {
        protected AppSettings _appSettings;
        protected TIService _viaCepClient;
        protected ILogger<TService> _logger;
        protected INotificationContext _notificationContext;

        public UnitTestBase()
        {
            var baseUri = "http://fake.com/";
            var pollyRetryStatusCodes = new int[] { 400, 408, 503, 504 };
            var flurlClientFactory = Substitute.For<IFlurlClientFactory>();
            flurlClientFactory.Get(baseUri).Returns(new FlurlClient(baseUri));

            _logger = Substitute.For<ILogger<TService>>();
            _notificationContext = new NotificationContext();
            _appSettings = new AppSettings
            {
                BaseUrl = baseUri,
                PollyRetryStatusCodes = pollyRetryStatusCodes,
                PollyRetryCount = 1
            };
            _viaCepClient = (TIService)Activator.CreateInstance(typeof(TService), _logger, _appSettings, flurlClientFactory, _notificationContext);

            _appSettings.Should().NotBeNull();
            _logger.Should().NotBeNull();
            _appSettings.Should().NotBeNull();
            _viaCepClient.Should().NotBeNull();
        }
    }
}
