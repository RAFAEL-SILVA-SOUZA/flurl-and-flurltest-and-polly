using Newtonsoft.Json;

namespace ClientFlurl.Domain.Entities
{
    public class Notification
    {
        [JsonIgnore]
        public int StatusCode { get; }
        public string Message { get; }
        public string RealMessage { get; }

        public Notification(int statusCode, string message, string realMessage = "")
        {
            StatusCode = statusCode;
            Message = message;
            RealMessage = realMessage;
        }
    }
}
