using ClientFlurl.Domain.Entities;
using ClientFlurl.Domain.Services.Contracts;

namespace ClientFlurl.Domain.Services
{
    public class NotificationContext : INotificationContext
    {
        private Notification _notification;

        public bool HasNotification()
        => _notification is not null;

        public void AddNotification(int key, string message, string realMessage = "")
        => _notification = new Notification(key, message, realMessage);

        public Notification GetNotification()
        => _notification;
    }
}
