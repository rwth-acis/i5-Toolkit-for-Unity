using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

