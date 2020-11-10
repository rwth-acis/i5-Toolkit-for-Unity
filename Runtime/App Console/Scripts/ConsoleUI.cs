namespace i5.Toolkit.Core.AppConsole
{
    /// <summary>
    /// Logic for the console's UI connection
    /// </summary>
    public abstract class ConsoleUI
    {
        /// <summary>
        /// If true, the console captures messages, even if the object is deactivated
        /// </summary>
        public bool CaptureInBackground { get; set; }

        protected ILogFormatter logFormatter;

        /// <summary>
        /// Console which handles the message capturing
        /// </summary>
        public IConsole Console { get; set; }

        /// <summary>
        /// Creates a new console UI logic
        /// </summary>
        /// <param name="logFormatterConfiguration">Determines how messages should be formatted</param>
        public ConsoleUI(LogFormatterConfiguration logFormatterConfiguration = null)
        {
            if (logFormatterConfiguration == null)
            {
                logFormatter = new DefaultConsoleFormatter();
            }
            else
            {
                logFormatter = logFormatterConfiguration.GenerateFormatter();
            }
            Console = new Console();
        }

        /// <summary>
        /// Called if the object is enabled
        /// Starts capturing messages
        /// </summary>
        public void OnEnable()
        {
            Console.IsCapturing = true;
            Console.OnMessageAdded += Console_OnMessageAdded;
            UpdateUI();
        }

        /// <summary>
        /// Called if hte object is disabled
        /// Stops capturing if it should not capture in background
        /// </summary>
        public void OnDisable()
        {
            if (!CaptureInBackground)
            {
                Console.IsCapturing = false;
            }
            Console.OnMessageAdded -= Console_OnMessageAdded;
        }

        // called if the console message 
        private void Console_OnMessageAdded()
        {
            UpdateUI();
        }

        // updates the UI display
        protected abstract void UpdateUI();
    }
}
