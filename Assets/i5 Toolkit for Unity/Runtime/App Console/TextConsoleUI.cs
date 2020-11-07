using i5.Toolkit.Core.Utilities.UnityAdapters;
using System;
using TMPro;

namespace i5.Toolkit.Core.AppConsole
{
    public class TextConsoleUI : ConsoleUI
    {
        private ITextAdapter consoleTextDisplay;

        public TextConsoleUI(ITextAdapter consoleTextDisplay, LogFormatterConfiguration logFormatterConfiguration = null) : base(logFormatterConfiguration)
        {
            this.consoleTextDisplay = consoleTextDisplay;
        }

        protected override void UpdateUI()
        {
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

            consoleTextDisplay.Text = text;
        }
    }
}
