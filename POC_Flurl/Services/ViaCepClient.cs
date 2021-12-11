using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using POC_Flurl.Entities;
using POC_Flurl.Helpers;
using POC_Flurl.Resources;
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
                if (string.IsNullOrWhiteSpace(cep))
                {
                    logger.LogError(Messages.Message_null_zip_code);
                    return null;
                }

                var address = await BuildRetryPolicy()
                                    .ExecuteAsync(() => appSettings
                                                        .BaseUrl
                                                        .AppendPathSegment($"{cep}//json//")
                                                        .GetJsonAsync<Address>());

                logger.LogInformation(string.Format(Messages.Success_to_received_response, JsonConvert.SerializeObject(address)));

                return address;
            }
            catch (FlurlHttpException ex)
            {
                logger.LogError(string.Format(Messages.Error_to_received_response, ex.StatusCode));
                throw ex;
            }
        }
    }
}
