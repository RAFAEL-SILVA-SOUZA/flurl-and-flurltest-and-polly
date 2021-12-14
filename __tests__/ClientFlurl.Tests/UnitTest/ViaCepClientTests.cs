using ClientFlurl.Entities;
using ClientFlurl.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace ClientFlurl.Tests
{
    public class ViaCepClientTests : UnitTestBase<IViaCepClient, ViaCepClient>
    {
        public ViaCepClientTests() => PrepareService("https://viacep.com.br/ws/");
        private const string zip_code = "24740500";


        [Fact(DisplayName = "Badrequest if the method not got the object")]
        public async Task Should_be_badrequest_on_getaddressbyzipcode()
        {
            //Arrange
            CreateHttpTest(statusCode: StatusCodes.Status400BadRequest);

            //Act
            var address = await _viaCepClient.GetAddressByZipCode("XPTO");

            //Assert 
            address.Should().BeNull();

            _notificationContext.HasNotification().Should().BeTrue();
            _notificationContext.GetNotification().StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            _notificationContext.GetNotification().Message.Should().Be(string.Format(Resources.Messages.Error_to_received_response, StatusCodes.Status400BadRequest));
            _logger.Received(1).Log(LogLevel.Error, string.Format(Resources.Messages.Error_to_received_response, StatusCodes.Status400BadRequest));
        }

        [Fact(DisplayName = "Test if the method got the object just like in the mock")]
        public async Task Should_be_getaddressbyzipcode_success()
        {
            //Arrange
            var enderecoMock = GetInstanceByJson<Address>(Resources.MockJson.Address_correct);
            CreateHttpTest(Resources.MockJson.Address_correct);

            //Act
            var address = await _viaCepClient.GetAddressByZipCode(zip_code);

            //Assert 
            address.Should().NotBeNull();
            address.Should().BeOfType<Address>();
            address.Should().BeEquivalentTo(enderecoMock);
            _logger.Received(1).Log(LogLevel.Information, string.Format(Resources.Messages.Success_to_received_response, JsonConvert.SerializeObject(address)));
        }

        [Fact(DisplayName = "Test if the method not got the object")]
        public async Task Should_be_getaddressbyzipcode_cep_null()
        {
            //Arrange
            CreateHttpTest();

            //Act
            var address = await _viaCepClient.GetAddressByZipCode(null);

            //Assert 
            address.Should().BeNull();
            _logger.Received(1).Log(LogLevel.Error, Resources.Messages.Message_null_zip_code);
        }

        [Theory(DisplayName = "Raise exception in the cases where the server's return is not in the status code 200 line")]
        [InlineData(StatusCodes.Status400BadRequest)]
        [InlineData(StatusCodes.Status401Unauthorized)]
        [InlineData(StatusCodes.Status500InternalServerError)]
        [InlineData(StatusCodes.Status503ServiceUnavailable)]
        public async Task Should_be_notify_error_on_getaddressbyzipcode_status_codes(int statusCode)
        {
            //Arrange
            CreateHttpTest(statusCode: statusCode);

            //Act
            var address = await _viaCepClient.GetAddressByZipCode("XPTO");

            //Assert 
            address.Should().BeNull();
            _notificationContext.HasNotification().Should().BeTrue();
            _notificationContext.GetNotification().StatusCode.Should().Be(statusCode);
            _notificationContext.GetNotification().Message.Should().Be(string.Format(Resources.Messages.Error_to_received_response, statusCode));
            _logger.Received(1).Log(LogLevel.Error, string.Format(Resources.Messages.Error_to_received_response, statusCode));
        }
    }
}
