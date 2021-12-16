using ClientFlurl.Entities;
using Flurl.Http;
using Polly;
using Polly.Retry;
using System;
using System.Linq;

namespace ClientFlurl.Helpers
{
    public abstract class PollyFlurlHelper
    {
        private readonly AppSettings appSettings;
        protected PollyFlurlHelper(AppSettings appSettings)
           => this.appSettings = appSettings;

        protected AsyncRetryPolicy BuildRetryPolicy
           => Policy.Handle<FlurlHttpException>(IsTransientError)
                    .WaitAndRetryAsync(appSettings.PollyRetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        private bool IsTransientError(FlurlHttpException exception)
           => exception.StatusCode.HasValue && appSettings.PollyRetryStatusCodes.Contains(exception.StatusCode.Value);
    }
}
