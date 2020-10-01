using i5.Toolkit.Core.ServiceCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class NotificationService : INotificationService
{
    public event EventHandler<INotificationMessage> NotificationPosted;

    public NotificationService()
    {
    }

    public void Initialize(IServiceManager owner)
    {
    }

    public void Cleanup()
    {
    }

    public void PostNotification(string message)
    {
        INotificationMessage notification = new NotificationMessage(message);
        InvokeNotificationEvent(notification);
    }

    public void PostNotification(INotificationMessage message)
    {
        InvokeNotificationEvent(message);
    }

    private void InvokeNotificationEvent(INotificationMessage notification)
    {
        NotificationPosted?.Invoke(this, notification);
    }
}