using ClientFlurl.Domain.Resources;
using ClientFlurl.Entities;
using ClientFlurl.Helpers;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ClientFlurl.Services
{
    public class ViaCepClient : PollyFlurlHelper, IViaCepClient
    {
        private readonly ILogger<ViaCepClient> logger;
        private readonly AppSettings appSettings;

        public ViaCepClient(ILogger<ViaCepClient> logger, IOptions<AppSettings> option) : base(option.Value)
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
                    return default;
                }

                var address = await BuildRetryPolicy
                                        .ExecuteAsync(() => appSettings
                                            .BaseUrl
                                            .AppendPathSegment($"{cep}//json//")
                                            .GetJsonAsync<Address>());

                logger.LogInformation(string.Format(Messages.Success_to_received_response, JsonConvert.SerializeObject(address)));

                return address.IsValid() ? address : default;
            }
            catch (FlurlHttpException ex)
            {
                logger.LogError(string.Format(Messages.Error_to_received_response, ex.StatusCode));
                throw ex;
            }
        }
    }
}
