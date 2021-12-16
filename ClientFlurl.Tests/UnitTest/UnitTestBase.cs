using ClientFlurl.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;

namespace ClientFlurl.Tests
{
    public abstract class UnitTestBase<TIService, TService> : TestBase
    {
        protected AppSettings _appSettings;
        protected TIService _viaCepClient;
        protected ILogger<TService> _logger;

        protected virtual void PrepareService(string baseUrl)
        {
            _appSettings = new AppSettings() { BaseUrl = baseUrl };
            _logger = Substitute.For<ILogger<TService>>();
            _viaCepClient = (TIService)Activator.CreateInstance(typeof(TService), _logger, _appSettings);

            _appSettings.Should().NotBeNull();
            _logger.Should().NotBeNull();
            _appSettings.Should().NotBeNull();
            _viaCepClient.Should().NotBeNull();
        }
    }
}
