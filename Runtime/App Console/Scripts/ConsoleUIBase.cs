using System.Collections.Generic;
using UnityEngine;

public abstract class ConsoleUIBase : MonoBehaviour
{
    [SerializeField] protected ConsoleFormatterBase consoleFormatter;

    protected virtual void Awake()
    {
        if (consoleFormatter == null)
        {
            consoleFormatter = new DefaultConsoleFormatter();
        }
    }

    public abstract void UpdateUI(List<INotificationMessage> notificationMessages);
}

