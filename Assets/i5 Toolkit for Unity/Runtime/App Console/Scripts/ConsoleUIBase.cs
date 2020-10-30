using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    public abstract class ConsoleUIBase : MonoBehaviour
    {
        [SerializeField] protected LogFormatterConfiguration logFormatterConfiguration;
        [SerializeField] protected AutoScroll autoScroll;

        protected ILogFormatter logFormatter;

        protected virtual void Awake()
        {
            if (logFormatterConfiguration == null)
            {
                logFormatter = new DefaultConsoleFormatter();
            }
        }

        public virtual void UpdateUI(List<INotificationMessage> notificationMessages)
        {
            autoScroll.ExpectContentChange = true;
        }
    }

}