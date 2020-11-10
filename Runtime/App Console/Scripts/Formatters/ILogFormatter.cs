namespace i5.Toolkit.Core.AppConsole
{
    /// <summary>
    /// Formatter that can format log messages to text output
    /// </summary>
    public interface ILogFormatter
    {
        /// <summary>
        /// Formats the given log message to text output
        /// </summary>
        /// <param name="logMessage">The log message to format</param>
        /// <returns>Returns formatted text output based on the log message</returns>
        string Format(ILogMessage logMessage);
    }
}
