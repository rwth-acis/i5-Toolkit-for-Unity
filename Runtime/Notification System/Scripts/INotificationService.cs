using i5.Toolkit.Core.ServiceCore;
using System;

public interface INotificationService : IService
{
    void PostNotification(string message);

    void PostNotification(INotificationMessage message);

    event EventHandler<INotificationMessage> NotificationPosted;
}

