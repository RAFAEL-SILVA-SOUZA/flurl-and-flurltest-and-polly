using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using NSubstitute;
using System.Net;
using System.Net.Http;

namespace ClientFlurl.Tests.Extensions
{
    public static class FlurlExtensions
    {
        public static void BuildFlurlClientFactory<T>(this IFlurlClientFactory flurlClientFactory, HttpStatusCode httpStatusCode, string httpContent = "")
        {
            var client = Substitute.For<IFlurlClient>();
            var flurlRequest = Substitute.For<IFlurlRequest>();

            var httpRes = new HttpResponseMessage
            {
                StatusCode = httpStatusCode
            };

            var flurlresponse = Substitute.For<IFlurlResponse>();
            flurlresponse.StatusCode.Returns((int)httpStatusCode);
            flurlresponse.ResponseMessage.Returns(httpRes);
            flurlresponse.GetJsonAsync<T>().Returns(JsonConvert.DeserializeObject<T>(httpContent));

            flurlRequest.GetAsync().Returns(flurlresponse);
            client.Request(Arg.Any<string>()).Returns(flurlRequest);
            flurlClientFactory.Get(Arg.Any<Flurl.Url>()).Returns(client);
        }
    }
}
