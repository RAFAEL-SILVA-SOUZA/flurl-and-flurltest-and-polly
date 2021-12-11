using FluentAssertions;
using Flurl.Http.Testing;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ClientFlurl.Tests
{
    public abstract class BaseTest
    {
        protected HttpTest _httpTest;

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
