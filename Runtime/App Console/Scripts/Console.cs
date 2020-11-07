using i5.Toolkit.Core.ServiceCore;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    /// <summary>
    /// Handles the console logic
    /// </summary>
    public class Console : IConsole
    {
        //private INotificationService notificationService;
        public List<ILogMessage> Messages { get; protected set; }

        private bool isCapturing;

        public event Action OnMessageAdded;

        public Console()
        {
            Messages = new List<ILogMessage>();
        }

        public bool IsCapturing
        {
            get => isCapturing;
            set
            {
                bool changeSubscription = false;
                if (isCapturing != value)
                {
                    changeSubscription = true;
                }
                isCapturing = value;
                if (changeSubscription)
                {
                    if (value)
                    {
                        Subscribe();
                    }
                    else
                    {
                        Unsubscribe();
                    }
                }
            }
        }

        protected virtual void Subscribe()
        {
            Application.logMessageReceived += Application_logMessageReceived;
        }

        protected virtual void Unsubscribe()
        {
            Application.logMessageReceived -= Application_logMessageReceived;
        }

        // called if a log message was received
        private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
        {
            AddMessage(new LogMessage(condition, stackTrace, type));
        }

        // adds a message to the console
        protected void AddMessage(ILogMessage message)
        {
            Messages.Add(message);
            OnMessageAdded?.Invoke();
        }
    }
}