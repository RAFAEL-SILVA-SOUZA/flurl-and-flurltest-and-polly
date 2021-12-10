using Flurl.Http;
using Polly;
using Polly.Retry;
using System;
using System.Linq;
using System.Net;

namespace POC_Flurl.Helpers
{
    public abstract class PollyFlurlHelper
    {
        protected AsyncRetryPolicy BuildRetryPolicy()
        {
            var retryPolicy = Policy
               .Handle<FlurlHttpException>(IsTransientError)
               .WaitAndRetryAsync(3, retryAttempt =>
               {
                   var nextAttemptIn = TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
                   return nextAttemptIn;
               });

            return retryPolicy;
        }

        private bool IsTransientError(FlurlHttpException exception)
        {
            int[] httpStatusCodesWorthRetrying =
            {
                (int)HttpStatusCode.RequestTimeout,
                (int)HttpStatusCode.BadGateway,
                (int)HttpStatusCode.ServiceUnavailable,
                (int)HttpStatusCode.GatewayTimeout
            };

            return exception.StatusCode.HasValue && httpStatusCodesWorthRetrying.Contains(exception.StatusCode.Value);
        }
    }
}
