using i5.Toolkit.Core.ServiceCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : MonoBehaviour, INotificationMessageReceiver
{
    [SerializeField] private ConsoleUIBase consoleUi;

    private INotificationService notificationService;
    private LogNotificationAdapter logNotificationAdapter;
    private List<INotificationMessage> notificationMessages = new List<INotificationMessage>();

    private void OnEnable()
    {
        if (!ServiceManager.ServiceExists<INotificationService>())
        {
            notificationService = new NotificationService();
            ServiceManager.RegisterService(notificationService);
        }

        notificationService.NotificationPosted += OnNotificationPosted;
        if (logNotificationAdapter == null)
        {
            logNotificationAdapter = new LogNotificationAdapter(this);
        }
        logNotificationAdapter.IsSubscribed = true;
    }

    private void OnDisable()
    {
        notificationService.NotificationPosted -= OnNotificationPosted;
        logNotificationAdapter.IsSubscribed = false;
    }

    private void OnNotificationPosted(object sender, INotificationMessage message)
    {
        AddMessage(message);
    }

    public void ReceiveNotification(INotificationMessage message)
    {
        AddMessage(message);
    }

    private void AddMessage(INotificationMessage message)
    {
        notificationMessages.Add(message);
        consoleUi.UpdateUI(notificationMessages);
    }
}
