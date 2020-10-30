namespace i5.Toolkit.Core.AppConsole
{
    public class DefaultConsoleFormatter : ILogFormatter
    {
        public string Format(ILogMessage logMessage)
        {
            return logMessage.Content;
        }
    }
}
