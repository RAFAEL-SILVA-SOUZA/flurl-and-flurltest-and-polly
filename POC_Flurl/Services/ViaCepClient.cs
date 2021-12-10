using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using POC_Flurl.Entities;
using POC_Flurl.Helpers;
using System.Threading.Tasks;

namespace POC_Flurl.Services
{
    public class ViaCepClient : PollyFlurlHelper, IViaCepClient
    {
        private readonly ILogger<ViaCepClient> logger;
        private readonly AppSettings appSettings;

        public ViaCepClient(ILogger<ViaCepClient> logger, IOptions<AppSettings> option)
        {
            this.logger = logger;
            appSettings = option.Value;
        }

        public async Task<Address> GetAddressByZipCode(string cep)
        {
            try
            {
                var address = await BuildRetryPolicy()
                                         .ExecuteAsync(() => appSettings.BaseUrl
                                                                  .AppendPathSegment($"{cep}//json//")
                                                                  .GetJsonAsync<Address>());

                logger.LogInformation($"Successfully received: {JsonConvert.SerializeObject(address)}");
                return address;
            }
            catch (FlurlHttpException ex)
            {
                logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
