using ClientFlurl.Api;
using FluentAssertions;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ClientFlurl.Tests.IntegrationTest
{
    public class ViaCepIntegrationTest : IntegrationTestBase<Startup>
    {

        [Fact(DisplayName = "Should return internal server error when passed an invalid zip code")]
        public async Task Raise_exception_on_get_address_by_cep()
        {
            //Arrange
            CreateHttpTest(Resources.MockJson.Address_correct);

            // Act
            var response = await _client.GetAsync("/viacep?cep=XPTO");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }


        [Fact(DisplayName = "Should return http status code of success and a content of type json")]
        public async Task Should_be_get_address_by_cep()
        {
            //Arrange
            CreateHttpTest(Resources.MockJson.Address_correct);

            // Act
            var response = await _client.GetAsync("/viacep?cep=24740500");

            // Assert
            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType.ToString().Should().BeEquivalentTo("application/json; charset=utf-8");
        }
    }
}
