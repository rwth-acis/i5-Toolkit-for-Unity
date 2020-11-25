namespace i5.Toolkit.Core.Experimental.NotificationSystem
{
    public class NotificationMessage : INotificationMessage
    {
        public string Content { get; }

        public NotificationMessage(string content)
        {
            Content = content;
        }
    }
}