using FluentAssertions;
using Flurl.Http.Testing;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ClientFlurl.Tests
{
    public abstract class TestBase
    {
        protected HttpTest _httpTest;

        protected void CreateHttpTest(string response = "", int statusCode = StatusCodes.Status200OK)
        {
            _httpTest = new HttpTest();
            _httpTest.RespondWith(response, statusCode);
            _httpTest.Should().NotBeNull();
        }

        protected static T GetInstanceByJson<T>(string json)
           => JsonConvert.DeserializeObject<T>(json);
    }
}
