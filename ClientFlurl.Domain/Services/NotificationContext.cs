using ClientFlurl.Domain.Entities;

namespace ClientFlurl.Domain.Services
{
    public class NotificationContext
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
