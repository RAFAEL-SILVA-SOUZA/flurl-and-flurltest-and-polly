using ClientFlurl.Api;
using ClientFlurl.Entities;
using ClientFlurl.Services;
using FluentAssertions;
using Flurl.Http;
using NSubstitute;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ClientFlurl.Tests.IntegrationTest
{
    public class ViaCepIntegrationTest : IntegrationTestBase<Startup, IViaCepClient>
    {

        [Fact(DisplayName = "Should be return badrequest")]
        public async Task Raise_flurl_exception()
        {
            //Arrange
            _service.When(x => x.GetAddressByZipCode(Arg.Any<string>())).Do(x =>
            {
                var call = new FlurlCall
                {
                    Response = new FlurlResponse(new System.Net.Http.HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                    })
                };
                throw new FlurlHttpException(call,"",new Exception());
            });

            CreateServer();

            // Act
            var response = await _client.GetAsync("/viacep?cep=XPTO");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }


        [Fact(DisplayName = "Should be return empty")]
        public async Task Should_be_return_empty()
        {
            //Arrange
            _service.GetAddressByZipCode(Arg.Any<string>()).Returns(default(Address));
            CreateServer();

            // Act
            var response = await _client.GetAsync("/viacep?cep=XPTO");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }


        [Fact(DisplayName = "Should return http status code of success and a content of type json")]
        public async Task Should_be_get_address_by_cep()
        {
            //Arrange
            CreateHttpTest(Resources.MockJson.Address_correct);

            _service.GetAddressByZipCode(Arg.Any<string>()).Returns(GetInstanceByJson<Address>(Resources.MockJson.Address_correct));
            CreateServer();

            // Act
            var response = await _client.GetAsync("/viacep?cep=24740500");

            // Assert
            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType.ToString().Should().BeEquivalentTo("application/json; charset=utf-8");
        }
    }
}
