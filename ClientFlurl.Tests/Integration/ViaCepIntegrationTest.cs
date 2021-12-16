using ClientFlurl.Api;
using ClientFlurl.Domain.Services.Contracts;
using ClientFlurl.Entities;
using ClientFlurl.Services;
using ClientFlurl.Tests.Resources;
using ClientFlurl.Tests.Unit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ClientFlurl.Tests.Integration
{
    public class ViaCepIntegrationTest : IntegrationTestBase<Startup, IViaCepClient, ViaCepClient>
    {

        [Fact(DisplayName = "Should return internal server error when passed an invalid zip code")]
        public async Task Raise_exception_on_get_address_by_cep()
        {
            //Arrange
            _service.When(x => x.GetAddressByZipCode(Arg.Any<string>())).Do(x =>
            {
                _logger.LogError(string.Format(Messages.Error_to_received_response, (int)HttpStatusCode.BadRequest));
                _notificationContext.AddNotification((int)HttpStatusCode.BadRequest, string.Format(Messages.Error_to_received_response, (int)HttpStatusCode.BadRequest));
            });
            CreateServer();

            // Act
            var response = await _client.GetAsync("/viacep?zipcode=XPTO");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            _logger.Received(1).Log(LogLevel.Error, string.Format(Messages.Error_to_received_response, (int)HttpStatusCode.BadRequest));
        }

        [Fact(DisplayName = "Should return http status code of success and empty content")]
        public async Task Should_be_get_address_by_cep_no_content()
        {
            //Arrange
            CreateServer();

            // Act
            var response = await _client.GetAsync("/viacep?zipcode=24455512");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact(DisplayName = "Should return http status code of success and empty content")]
        public async Task Should_be_get_address_by_cep_success_and_empty_content()
        {
            //Arrange
            _service.GetAddressByZipCode(Arg.Any<string>()).Returns(default(Address));
            CreateServer();

            // Act
            var response = await _client.GetAsync("/viacep?zipcode=24455512");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact(DisplayName = "Should return http status code of success and a content of type json")]
        public async Task Should_be_get_address_by_cep()
        {
            //Arrange
            _service.GetAddressByZipCode(Arg.Any<string>()).Returns(Mocks.AddressCorrect());
            CreateServer();

            // Act
            var response = await _client.GetAsync("/viacep?zipcode=24740500");

            // Assert
            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType.ToString().Should().BeEquivalentTo("application/json; charset=utf-8");
        }
    }
}
