namespace i5.Toolkit.Core.AppConsole
{
    public interface ILogFormatter
    {
        string Format(ILogMessage logMessage);
    }
}
