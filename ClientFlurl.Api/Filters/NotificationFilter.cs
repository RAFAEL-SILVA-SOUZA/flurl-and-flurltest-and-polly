using ClientFlurl.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ClientFlurl.Api.Filters
{
    public class NotificationFilter : IAsyncResultFilter
    {
        private readonly NotificationContext _notificationContext;

        public NotificationFilter(NotificationContext notificationContext)
        {
            _notificationContext = notificationContext;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (_notificationContext.HasNotification())
            {
                var notification = _notificationContext.GetNotification();

                context.HttpContext.Response.StatusCode = notification.StatusCode;
                context.HttpContext.Response.ContentType = "application/json";

                await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(notification));

                return;
            }
            await next();
        }
    }
}
