using FluentAssertions;
using Flurl.Http.Testing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NSubstitute;
using POC_Flurl.Entities;
using System;

namespace POC_Flurl.Tests
{
    public abstract class TestBase<TIService, TService>
    {
        protected IOptions<AppSettings> _appSettings;
        protected TIService _viaCepClient;
        protected HttpTest _httpTest;
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

        protected void CreateHttpTest<T>(T response, bool realHttp = false, int statusCode = StatusCodes.Status200OK) where T : class
        {
            _httpTest = new HttpTest();
            _httpTest.RespondWith(JsonConvert.SerializeObject(response), statusCode);
            if (realHttp) _httpTest.AllowRealHttp();

            _httpTest.Should().NotBeNull();
        }

        protected T GetInstanceByJson<T>(string json)
        => JsonConvert.DeserializeObject<T>(json);
    }
}
