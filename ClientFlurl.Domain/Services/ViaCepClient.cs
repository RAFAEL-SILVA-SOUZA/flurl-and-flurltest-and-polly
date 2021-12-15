using ClientFlurl.Domain.Resources;
using ClientFlurl.Domain.Services.Contracts;
using ClientFlurl.Entities;
using ClientFlurl.Helpers;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClientFlurl.Services
{
    public class ViaCepClient : PollyFlurlHelper, IViaCepClient
    {
        private readonly ILogger<ViaCepClient> logger;
        private readonly INotificationContext notificationContext;
        private readonly IFlurlClient _flurlClient;

        public ViaCepClient(ILogger<ViaCepClient> logger,
                            AppSettings appSettings,
                            HttpClient httpClient,
                            INotificationContext notificationContext) : base(appSettings)
        {
            this.logger = logger;
            this.notificationContext = notificationContext;
            _flurlClient = new FlurlClient(httpClient);
        }

        public async Task<Address> GetAddressByZipCode(string zipCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(zipCode))
                {
                    logger.LogError(Messages.Message_null_zip_code);
                    return default;
                }

                var response = await GetResponseFromServer(zipCode);

                if (VerifyIfIsSuccessStatusCode(response) || VerifyIfIsExactlySuccessStatusCode(response))
                {
                    return default;
                }

                return await TryValidateAddress(response);
            }
            catch (FlurlHttpException ex)
            {
                HandlerFlurlHttpException(ex);
                return default;
            }
        }

        private async Task<IFlurlResponse> GetResponseFromServer(string zipCode)
        {
            return await BuildRetryPolicy
                               .ExecuteAsync(() => _flurlClient.Request($"{zipCode}/json/")
                                                               .GetAsync());
        }

        private bool VerifyIfIsSuccessStatusCode(IFlurlResponse flurlResponse)
        {
            if (!flurlResponse.ResponseMessage.IsSuccessStatusCode)
            {
                notificationContext.AddNotification(flurlResponse.StatusCode, string.Format(Messages.Error_to_received_response, flurlResponse.StatusCode));
                logger.LogError(string.Format(Messages.Error_to_received_response, flurlResponse.StatusCode));
                return true;
            }
            return false;
        }

        private bool VerifyIfIsExactlySuccessStatusCode(IFlurlResponse flurlResponse)
        {
            if (!flurlResponse.ResponseMessage.IsSuccessStatusCode)
            {
                logger.LogInformation(string.Format(Messages.Success_to_received_response, JsonConvert.SerializeObject(default)));
                return true;
            }
            return false;
        }

        private async Task<Address> TryValidateAddress(IFlurlResponse flurlResponse)
        {
            var address = await flurlResponse.GetJsonAsync<Address>();

            if (address is null || address.IsNotValid())
            {
                logger.LogInformation(string.Format(Messages.Success_to_received_response, JsonConvert.SerializeObject(address)));
                return default;
            }

            logger.LogInformation(string.Format(Messages.Success_to_received_response, JsonConvert.SerializeObject(address)));
            return address;
        }

        private void HandlerFlurlHttpException(FlurlHttpException ex)
        {
            notificationContext.AddNotification(ex.StatusCode.Value, string.Format(Messages.Error_to_received_response, ex.StatusCode), ex.Message);
            logger.LogError(string.Format(Messages.Error_to_received_response, ex.StatusCode));
        }
    }
}
