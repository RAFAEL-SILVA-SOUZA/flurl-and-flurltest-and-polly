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
        protected AsyncRetryPolicy BuildRetryPolicy()
        {
            return Policy
                    .Handle<FlurlHttpException>(IsTransientError)
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        private bool IsTransientError(FlurlHttpException exception)
        {
            int[] httpStatusCodesWorthRetrying =
            {
                StatusCodes.Status408RequestTimeout,
                StatusCodes.Status400BadRequest,
                StatusCodes.Status503ServiceUnavailable,
                StatusCodes.Status504GatewayTimeout
            };

            return exception.StatusCode.HasValue && httpStatusCodesWorthRetrying.Contains(exception.StatusCode.Value);
        }
    }
}
