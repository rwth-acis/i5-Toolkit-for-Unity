using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using i5.Toolkit.Core.AppConsole;

public class Console : MonoBehaviour, IMessageDisplay
{
    private IMessageDisplayUI consoleUi;
    private ILogFormatter consoleFormatter;

    private List<string> messages = new List<string>();

    private void Awake()
    {
        consoleFormatter = GetComponent<ILogFormatter>();
        consoleUi = GetComponent<IMessageDisplayUI>();
    }

    private void Start()
    {
        MessageDisplayLogHandler logHandler = new MessageDisplayLogHandler(this, consoleFormatter);
        Debug.unityLogger.AddLogHandler(logHandler);
    }

    public void AddMessage(string message)
    {
        messages.Add(message);
        consoleUi.UpdateUI(messages);
    }
}
