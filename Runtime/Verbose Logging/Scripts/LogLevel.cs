/// <summary>
/// Defines the importance of a log statement
/// </summary>
public enum LogLevel
{
    /// <summary>
    /// Category for log statements about critical errors that endanger the further execution of the program
    /// </summary>
    CRITICAL = 0,
    /// <summary>
    /// Category for log statements about errors from which the application can recover
    /// </summary>
    ERROR = 1,
    /// <summary>
    /// Category for log statements about warnings
    /// e.g., where a state was detected that could lead to an error
    /// </summary>
    WARNING = 2,
    /// <summary>
    /// Category for log statements about general information about application's state
    /// </summary>
    INFO = 3,
    /// <summary>
    /// Category for log statements that developers and maintainers can use to debug functionality
    /// e.g., by outputting values and more detail
    /// </summary>
    DEBUG = 4,
    /// <summary>
    /// Category for log statements that help trace the execution paths of the application, e.g, which methods were entered and which if case was selected
    /// The most detailed category of log statements
    /// </summary>
    TRACE = 5
}