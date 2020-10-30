using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    public class TextConsoleUI : ConsoleUIBase
    {
        [SerializeField] private TextMeshProUGUI consoleTextDisplay;

        public override void UpdateUI(List<INotificationMessage> notificationMessages)
        {
            base.UpdateUI(notificationMessages);

            string text = "";
            for (int i = 0; i < notificationMessages.Count; i++)
            {
                ILogMessage logMessage = notificationMessages[i] as ILogMessage;
                if (logMessage != null)
                {
                    text += consoleFormatter.Format(logMessage);
                }
                else
                {
                    text += notificationMessages[i].Content;
                }

                if (i < notificationMessages.Count - 1)
                {
                    text += Environment.NewLine;
                }
            }

            consoleTextDisplay.text = text;
        }
    }
}