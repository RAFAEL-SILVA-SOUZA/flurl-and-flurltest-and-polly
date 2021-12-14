namespace ClientFlurl.Domain.Entities
{
    public interface INotificationContext
    {
        public void AddNotification(int key, string message, string realMessage = "");
        public Notification GetNotification();
        public bool HasNotification();
    }
}
