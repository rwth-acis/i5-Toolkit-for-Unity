using System.Threading.Tasks;

namespace i5.Toolkit.Core.Utilities.ContentLoaders
{
    /// <summary>
    /// /// Interface for modules which load content from the Web
    /// </summary>
    /// <typeparam name="T">The result's payload</typeparam>
    public interface IContentLoader<T>
    {
        /// <summary>
        /// Loads a resource from the given URI
        /// Should be used asynchronously
        /// </summary>
        /// <param name="uri">The uri where the string resource is stored</param>
        /// <returns>The fetched resource</returns>
        Task<WebResponse<T>> LoadAsync(string uri);
    }
}