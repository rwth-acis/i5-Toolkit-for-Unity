using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    public class TmpConsoleUI : ConsoleUIBase
    {
        [SerializeField] private TextMeshProUGUI consoleTextDisplay;

        protected override void UpdateUI()
        {
            base.UpdateUI();

            string text = "";
            for (int i = 0; i < console.Messages.Count; i++)
            {
                ILogMessage logMessage = console.Messages[i];
                if (logMessage != null)
                {
                    text += logFormatter.Format(logMessage);
                }
                else
                {
                    text += console.Messages[i].Content;
                }

                if (i < console.Messages.Count - 1)
                {
                    text += Environment.NewLine;
                }
            }

            consoleTextDisplay.text = text;
        }
    }
}