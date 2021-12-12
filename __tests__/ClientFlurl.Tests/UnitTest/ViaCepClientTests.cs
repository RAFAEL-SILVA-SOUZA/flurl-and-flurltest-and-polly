using FluentAssertions;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using ClientFlurl.Entities;
using ClientFlurl.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ClientFlurl.Tests
{
    public class ViaCepClientTests : UnitTestBase<IViaCepClient, ViaCepClient>
    {
        public ViaCepClientTests() => PrepareService("https://viacep.com.br/ws/");
        private const string zip_code = "24740500";


        [Fact(DisplayName = "Raise exception if the method not got the object")]
        public async Task Raise_exception_on_getaddressbyzipcode()
        {
            //Arrange
            CreateHttpTest(realHttp:true);

            //Act
            Func<Task<Address>> act = async () => await _viaCepClient.GetAddressByZipCode("XPTO");

            //Assert 
            await act.Should().ThrowAsync<FlurlHttpException>();
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
        public async Task Raise_exception_on_getaddressbyzipcode_status_codes(int statusCode)
        {
            //Arrange
            CreateHttpTest(statusCode: statusCode);
            
            //Act
            Func<Task<Address>> act = async () => await _viaCepClient.GetAddressByZipCode(zip_code);

            //Assert 
            await act.Should().ThrowAsync<FlurlHttpException>().Where(x => x.StatusCode == statusCode);
            _logger.Received(1).Log(LogLevel.Error, string.Format(Resources.Messages.Error_to_received_response, statusCode));
        }


        [Fact(DisplayName = "Test if the method got the object exactly as it came from the server")]
        public async Task Should_be_getaddressbyzipcode_realhttp_success()
        {
            //Arrange
            var addressMock = GetInstanceByJson<Address>(Resources.MockJson.Address_correct);
            CreateHttpTest(Resources.MockJson.Address_correct, true);

            //Act
            var address = await _viaCepClient.GetAddressByZipCode(zip_code);

            //Assert 
            address.Should().NotBeNull();
            address.Should().BeOfType<Address>();
            address.Should().BeEquivalentTo(addressMock);
            _logger.Received(1).Log(LogLevel.Information, string.Format(Resources.Messages.Success_to_received_response, JsonConvert.SerializeObject(address)));
        }

        [Fact(DisplayName = "Test if the method got the object other than how it came from the server")]
        public async Task Should_be_getaddressbyzipcode_realhttp_compare_error()
        {
            //Arrange
            var addressMock = GetInstanceByJson<Address>(Resources.MockJson.Address_incorrect);
            CreateHttpTest(Resources.MockJson.Address_incorrect, true);

            //Act
            var address = await _viaCepClient.GetAddressByZipCode(zip_code);

            //Assert 
            address.Should().NotBeNull();
            address.Should().BeOfType<Address>();
            address.Should().NotBeEquivalentTo(addressMock);
        }
    }
}
