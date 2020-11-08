namespace i5.Toolkit.Core.AppConsole
{
    public abstract class ConsoleUI
    {
        public bool CaptureInBackground { get; set; }

        protected ILogFormatter logFormatter;

        public IConsole Console { get; set; }

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

        public void OnEnable()
        {
            Console.IsCapturing = true;
            Console.OnMessageAdded += Console_OnMessageAdded;
            UpdateUI();
        }

        public void OnDisable()
        {
            if (!CaptureInBackground)
            {
                Console.IsCapturing = false;
            }
            Console.OnMessageAdded -= Console_OnMessageAdded;
        }

        private void Console_OnMessageAdded()
        {
            UpdateUI();
        }

        protected abstract void UpdateUI();
    }
}
