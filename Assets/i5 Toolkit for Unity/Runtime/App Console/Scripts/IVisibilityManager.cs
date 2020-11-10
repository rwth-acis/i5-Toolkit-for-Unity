namespace i5.Toolkit.Core.AppConsole
{
    /// <summary>
    /// Visibility manager that handles showing and hiding Unity objects
    /// </summary>
    public interface IVisibilityManager
    {
        /// <summary>
        /// If true, an object is visible
        /// </summary>
        bool IsVisible { get; set; }
    }
}