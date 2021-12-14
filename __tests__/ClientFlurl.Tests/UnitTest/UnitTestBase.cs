using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using ClientFlurl.Entities;
using System;
using Flurl.Http.Configuration;
using ClientFlurl.Domain.Entities;
using Flurl.Http;

namespace ClientFlurl.Tests
{
    public abstract class UnitTestBase<TIService, TService> : TestBase
    {
        protected IOptions<AppSettings> _appSettings;
        protected TIService _viaCepClient;
        protected ILogger<TService> _logger;
        protected INotificationContext _notificationContext;

        protected virtual void PrepareService(string baseUrl)
        {
            var appSettings = new AppSettings() { BaseUrl = baseUrl };

            var flurlClientFactory = Substitute.For<IFlurlClientFactory>();
            flurlClientFactory.Get(baseUrl).Returns(new FlurlClient(baseUrl));

            _logger = Substitute.For<ILogger<TService>>();            
            _notificationContext = new NotificationContext();
            _appSettings = Options.Create(appSettings);
            _viaCepClient = (TIService)Activator.CreateInstance(typeof(TService), _logger, _appSettings, flurlClientFactory, _notificationContext);

            appSettings.Should().NotBeNull();
            _logger.Should().NotBeNull();
            _appSettings.Should().NotBeNull();
            _viaCepClient.Should().NotBeNull();
        }
    }
}
