using FluentAssertions;
using Flurl.Http.Testing;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ClientFlurl.Tests
{
    public abstract class TestBase
    {
        protected HttpTest _httpTest;

        protected void CreateHttpTest(string response = "", bool realHttp = false, int statusCode = StatusCodes.Status200OK)
        {
            _httpTest = new HttpTest();
            _httpTest.RespondWith(response, statusCode);
            if (realHttp) _httpTest.AllowRealHttp();

            _httpTest.Should().NotBeNull();
        }

        protected T GetInstanceByJson<T>(string json)
           => JsonConvert.DeserializeObject<T>(json);
    }
}
