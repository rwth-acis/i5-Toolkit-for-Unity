using i5.Toolkit.Core.Experimental.UnityAdapters;
using System;
using TMPro;

namespace i5.Toolkit.Core.AppConsole
{
    /// <summary>
    /// Console UI for text displays
    /// </summary>
    public class TextConsoleUI : ConsoleUI
    {
        private ITextDisplay consoleTextDisplay;

        /// <summary>
        /// Creates the console UI logic instance
        /// </summary>
        /// <param name="consoleTextDisplay">The text display on which messages should be shown</param>
        /// <param name="logFormatterConfiguration">The formatter configuration that defines how messages are formatted</param>
        public TextConsoleUI(ITextDisplay consoleTextDisplay, LogFormatterConfiguration logFormatterConfiguration = null) : base(logFormatterConfiguration)
        {
            this.consoleTextDisplay = consoleTextDisplay;
        }

        // Updates the UI by writing the formatted messages to the text display
        protected override void UpdateUI()
        {
            string text = "";
            for (int i = 0; i < Console.Messages.Count; i++)
            {
                ILogMessage logMessage = Console.Messages[i];
                if (logMessage != null)
                {
                    text += logFormatter.Format(logMessage);
                }
                else
                {
                    text += Console.Messages[i].Content;
                }

                if (i < Console.Messages.Count - 1)
                {
                    text += Environment.NewLine;
                }
            }

            consoleTextDisplay.Text = text;
        }
    }
}
