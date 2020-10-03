using System.Threading.Tasks;
using UnityEngine.Networking;
using i5.Toolkit.Core.Utilities.Async;
using System.IO;
using System.Text;
using UnityEngine;

namespace i5.Toolkit.Core.Utilities.ContentLoaders
{
    /// <summary>
    /// Content loader that uses System.IO to read data from a file
    /// </summary>
    public class CompositeLoader : IContentLoader<string>
    {
        /// <summary>
        /// Loads content from the web or a local file 
        /// </summary>
        /// <param name="uri">The uri of the object.</param>
        /// <returns>Returns the read string content</returns>
        public async Task<WebResponse<string>> LoadAsync(string uri)
        {
            if (File.Exists(uri))
            {
                FileSystemLoader fsLoader = new FileSystemLoader();
                return await fsLoader.LoadAsync(uri);
            }
            else
            {
                UnityWebRequestLoader uwrLoader = new UnityWebRequestLoader();
                return await uwrLoader.LoadAsync(uri);
            }

        }
    }
}
