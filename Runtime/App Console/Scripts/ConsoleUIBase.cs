using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.SceneConsole
{
    public abstract class ConsoleUIBase : MonoBehaviour
    {
        [SerializeField] protected ConsoleFormatterBase consoleFormatter;
        [SerializeField] protected AutoScroll autoScroll;

        protected virtual void Awake()
        {
            if (consoleFormatter == null)
            {
                consoleFormatter = new DefaultConsoleFormatter();
            }
        }

        public virtual void UpdateUI(List<INotificationMessage> notificationMessages)
        {
            autoScroll.ExpectContentChange = true;
        }
    }

}