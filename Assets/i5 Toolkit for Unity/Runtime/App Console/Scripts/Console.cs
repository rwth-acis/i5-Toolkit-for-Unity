using i5.Toolkit.Core.ServiceCore;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.SceneConsole
{
    /// <summary>
    /// Handles the console logic
    /// </summary>
    public class Console : MonoBehaviour
    {
        [Tooltip("The console UI for displaying the collected data")]
        [SerializeField] private ConsoleUIBase consoleUi;
        [Tooltip("If set to true, the console will capture logs even if the gameobject is deactivated.")]
        [SerializeField] private bool captureInBackground = true;

        private INotificationService notificationService;
        private LogNotificationAdapter logNotificationAdapter;
        private List<INotificationMessage> notificationMessages = new List<INotificationMessage>();

        private bool isCapturing;

        private void OnEnable()
        {
            if (!ServiceManager.ServiceExists<INotificationService>())
            {
                notificationService = new NotificationService();
                ServiceManager.RegisterService(notificationService);
            }

            if (logNotificationAdapter == null)
            {
                logNotificationAdapter = new LogNotificationAdapter();
            }
            if (!isCapturing)
            {
                notificationService.NotificationPosted += OnNotificationPosted;
                Application.logMessageReceived += Application_logMessageReceived;
                isCapturing = true;
            }
            else
            {
                consoleUi.UpdateUI(notificationMessages);
            }
        }

        private void OnDisable()
        {
            if (!captureInBackground)
            {
                notificationService.NotificationPosted -= OnNotificationPosted;
                Application.logMessageReceived -= Application_logMessageReceived;
                isCapturing = false;
            }
        }

        // called if a notification was posted
        private void OnNotificationPosted(object sender, INotificationMessage message)
        {
            AddMessage(message);
        }

        // called if a log message was received
        private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
        {
            AddMessage(logNotificationAdapter.FromLog(condition, stackTrace, type));
        }

        // adds a message to the console
        private void AddMessage(INotificationMessage message)
        {
            notificationMessages.Add(message);
            consoleUi.UpdateUI(notificationMessages);
        }
    }
}