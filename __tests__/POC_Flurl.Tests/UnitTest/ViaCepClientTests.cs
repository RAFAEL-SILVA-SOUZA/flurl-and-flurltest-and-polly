using FluentAssertions;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using POC_Flurl.Entities;
using POC_Flurl.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace POC_Flurl.Tests
{
    public class ViaCepClientTests : TestBase<IViaCepClient, ViaCepClient>
    {
        public ViaCepClientTests() => PrepareService("https://viacep.com.br/ws/");
        private const string zip_code = "24740500";

        [Fact(DisplayName = "Test if the method got the object just like in the mock")]
        public async Task Should_be_getaddressbyzipcode_success()
        {
            var enderecoMock = GetInstanceByJson<Address>(Resources.MockJson.Address_correct);
            CreateHttpTest(enderecoMock);

            var address = await _viaCepClient.GetAddressByZipCode(zip_code);

            address.Should().NotBeNull();
            address.Should().BeOfType<Address>();
            address.Should().BeEquivalentTo(enderecoMock);
            _logger.Received(1).Log(LogLevel.Information, string.Format(Resources.Messages.Success_to_received_response, JsonConvert.SerializeObject(address)));
        }

        [Fact(DisplayName = "Test if the method not got the object")]
        public async Task Should_be_getaddressbyzipcode_cep_null()
        {            
            CreateHttpTest(new {});
            var address = await _viaCepClient.GetAddressByZipCode(null);

            address.Should().BeNull();
            _logger.Received(1).Log(LogLevel.Error, Resources.Messages.Message_null_zip_code);
        }

        [Theory(DisplayName = "Tests the cases where the server's return is not in the status code 200 line")]
        [InlineData(StatusCodes.Status400BadRequest)]
        [InlineData(StatusCodes.Status401Unauthorized)]
        [InlineData(StatusCodes.Status500InternalServerError)]
        [InlineData(StatusCodes.Status503ServiceUnavailable)]
        public async Task Should_be_getaddressbyzipcode_error(int statusCode)
        {
            CreateHttpTest<object>(new { }, statusCode: statusCode);
            Func<Task<Address>> act = async () => await _viaCepClient.GetAddressByZipCode(zip_code);
            await act.Should().ThrowAsync<FlurlHttpException>().Where(x => x.StatusCode == statusCode);
            _logger.Received(1).Log(LogLevel.Error, string.Format(Resources.Messages.Error_to_received_response, statusCode));
        }


        [Fact(DisplayName = "Test if the method got the object exactly as it came from the server")]
        public async Task Should_be_getaddressbyzipcode_realhttp_success()
        {
            var addressMock = GetInstanceByJson<Address>(Resources.MockJson.Address_correct);

            CreateHttpTest(addressMock, true);

            var address = await _viaCepClient.GetAddressByZipCode(zip_code);

            address.Should().NotBeNull();
            address.Should().BeOfType<Address>();
            address.Should().BeEquivalentTo(addressMock);

            _logger.Received(1).Log(LogLevel.Information, string.Format(Resources.Messages.Success_to_received_response, JsonConvert.SerializeObject(address)));
        }

        [Fact(DisplayName = "Test if the method got the object other than how it came from the server")]
        public async Task Should_be_getaddressbyzipcode_realhttp_compare_error()
        {
            var addressMock = GetInstanceByJson<Address>(Resources.MockJson.Address_incorrect);

            CreateHttpTest(addressMock, true);

            var address = await _viaCepClient.GetAddressByZipCode(zip_code);

            address.Should().NotBeNull();
            address.Should().BeOfType<Address>();
            address.Should().NotBeEquivalentTo(addressMock);
        }
    }
}
