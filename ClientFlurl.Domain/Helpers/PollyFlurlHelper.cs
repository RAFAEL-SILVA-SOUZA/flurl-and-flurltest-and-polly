using ClientFlurl.Entities;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Polly;
using Polly.Retry;
using System;
using System.Linq;

namespace ClientFlurl.Helpers
{
    public abstract class PollyFlurlHelper
    {
        private readonly AppSettings appSettings;
        private readonly int[] httpStatusCodesWorthRetrying =
                               {
                                   StatusCodes.Status408RequestTimeout,
                                   StatusCodes.Status400BadRequest,
                                   StatusCodes.Status503ServiceUnavailable,
                                   StatusCodes.Status504GatewayTimeout
                               };

        protected PollyFlurlHelper(AppSettings appSettings)
           => this.appSettings = appSettings;

        protected AsyncRetryPolicy BuildRetryPolicy
           => Policy.Handle<FlurlHttpException>(IsTransientError)
                    .WaitAndRetryAsync(appSettings.PollyRetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        private bool IsTransientError(FlurlHttpException exception)
           => exception.StatusCode.HasValue && httpStatusCodesWorthRetrying.Contains(exception.StatusCode.Value);
    }
}
