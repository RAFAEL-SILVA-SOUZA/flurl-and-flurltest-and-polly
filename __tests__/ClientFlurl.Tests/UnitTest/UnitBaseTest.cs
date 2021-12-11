using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using ClientFlurl.Entities;
using System;

namespace ClientFlurl.Tests
{
    public abstract class UnitBaseTest<TIService, TService> : BaseTest
    {
        protected IOptions<AppSettings> _appSettings;
        protected TIService _viaCepClient;
        protected ILogger<TService> _logger;

        protected virtual void PrepareService(string baseUrl)
        {
            var appSettings = new AppSettings() { BaseUrl = baseUrl };
            _logger = Substitute.For<ILogger<TService>>();
            _appSettings = Options.Create(appSettings);
            _viaCepClient = (TIService)Activator.CreateInstance(typeof(TService), _logger, _appSettings);

            appSettings.Should().NotBeNull();
            _logger.Should().NotBeNull();
            _appSettings.Should().NotBeNull();
            _viaCepClient.Should().NotBeNull();
        }
    }
}
