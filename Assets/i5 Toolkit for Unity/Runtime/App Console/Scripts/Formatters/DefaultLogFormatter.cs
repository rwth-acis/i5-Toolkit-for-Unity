namespace i5.Toolkit.Core.AppConsole
{
    /// <summary>
    /// Default console formatter which should be used if no configuration file is provided
    /// </summary>
    public class DefaultConsoleFormatter : ILogFormatter
    {
        /// <summary>
        /// Formats the given log messages to text output
        /// The default formatter returns the log message's content
        /// </summary>
        /// <param name="logMessage">The log message to format</param>
        /// <returns>Returns a formatted string</returns>
        public string Format(ILogMessage logMessage)
        {
            return logMessage.Content;
        }
    }
}
