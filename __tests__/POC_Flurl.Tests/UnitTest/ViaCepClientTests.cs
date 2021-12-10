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

        [Fact(DisplayName = "Test if the method got the object just like in the mock")]
        public async Task Should_be_getaddressbyzipcode_success()
        {
            var enderecoMock = new Address("24740-500", "Rua Libanio Ratazi",
                                            "", "Coelho",
                                            "São Gonçalo", "RJ",
                                            "3304904", "",
                                            "21", "5897");
            CreateHttpTest(enderecoMock);
            
            var address = await _viaCepClient.GetAddressByZipCode("24740500");

            address.Should().NotBeNull();
            address.Should().BeOfType<Address>();
            address.Should().BeEquivalentTo(enderecoMock);
            _logger.Received(1).Log(LogLevel.Information, $"Successfully received: {JsonConvert.SerializeObject(address)}");
        }

        [Theory(DisplayName = "Tests the cases where the server's return is not in the status code 200 line")]
        [InlineData(StatusCodes.Status400BadRequest, "Call failed with status code 400 (Bad Request): GET https://viacep.com.br/ws/24740500//json//")]
        [InlineData(StatusCodes.Status401Unauthorized, "Call failed with status code 401 (Unauthorized): GET https://viacep.com.br/ws/24740500//json//")]
        [InlineData(StatusCodes.Status500InternalServerError, "Call failed with status code 500 (Internal Server Error): GET https://viacep.com.br/ws/24740500//json//")]
        [InlineData(StatusCodes.Status503ServiceUnavailable, "Call failed with status code 503 (Service Unavailable): GET https://viacep.com.br/ws/24740500//json//")]
        public async Task Should_be_getaddressbyzipcode_error(int statusCode, string message)
        {
            CreateHttpTest<object>(new { }, statusCode: statusCode);
            Func<Task<Address>> act = async () => await _viaCepClient.GetAddressByZipCode("24740500");
            await act.Should().ThrowAsync<FlurlHttpException>();
            _logger.Received(1).Log(LogLevel.Error, message);
        }


        [Fact(DisplayName = "Test if the method got the object exactly as it came from the server")]
        public async Task Should_be_getaddressbyzipcode_realhttp_success()
        {
            var addressMock = new Address("24740-500", "Rua Libanio Ratazi",
                                          "", "Coelho",
                                          "São Gonçalo", "RJ",
                                          "3304904", "",
                                          "21", "5897");

            CreateHttpTest(addressMock, true);

            var address = await _viaCepClient.GetAddressByZipCode("24740500");

            address.Should().NotBeNull();
            address.Should().BeOfType<Address>();
            address.Should().BeEquivalentTo(addressMock);
            _logger.Received(1).Log(LogLevel.Information, $"Successfully received: {JsonConvert.SerializeObject(address)}");
        }

        [Fact(DisplayName = "Test if the method got the object other than how it came from the server")]
        public async Task Should_be_getaddressbyzipcode_realhttp_compare_error()
        {
            var addressMock = new Address("24740-500", "Rua Libanio",
                                          "", "Coelho",
                                          "São Gonçalo", "RJ",
                                          "3304904", "",
                                          "21", "5897");

            CreateHttpTest(addressMock, true);

            var address = await _viaCepClient.GetAddressByZipCode("24740500");

            address.Should().NotBeNull();
            address.Should().BeOfType<Address>();
            address.Should().NotBeEquivalentTo(addressMock);
        }
    }
}
