namespace i5.Toolkit.Core.AppConsole
{
    public abstract class ConsoleUI
    {
        public bool CaptureInBackground { get; set; }

        protected ILogFormatter logFormatter;

        protected IConsole console;

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
            console = new Console();
        }

        public void OnEnable()
        {
            console.IsCapturing = true;
            console.OnMessageAdded += Console_OnMessageAdded;
            UpdateUI();
        }

        public void OnDisable()
        {
            if (!CaptureInBackground)
            {
                console.IsCapturing = false;
            }
            console.OnMessageAdded -= Console_OnMessageAdded;
        }

        private void Console_OnMessageAdded()
        {
            UpdateUI();
        }

        protected abstract void UpdateUI();
    }
}
