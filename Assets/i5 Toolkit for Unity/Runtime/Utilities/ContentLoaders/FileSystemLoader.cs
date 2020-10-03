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
    public class FileSystemLoader : IContentLoader<string>
    {
        /// <summary>
        /// Loads content from the local disk, at which the uri points to. 
        /// </summary>
        /// <param name="path">The file path of the object file.</param>
        /// <returns>Returns the read string content</returns>
        public async Task<WebResponse<string>> LoadAsync(string path)
        {
            if (!File.Exists(path))
            {
                i5Debug.LogError("The File: " + path + " does not exist.", this);
                return new WebResponse<string>("File does not exist.", 404);
            }
            else
            {
                string readText;
                byte[] readByte;

                using (StreamReader reader = File.OpenText(path))
                {
                    readText = await reader.ReadToEndAsync();
                }

                using (FileStream SourceStream = File.Open(path, FileMode.Open))
                {
                    readByte = new byte[SourceStream.Length];
                    await SourceStream.ReadAsync(readByte, 0, (int)SourceStream.Length);
                }

                return new WebResponse<string>(readText, readByte, 200);
            }

        }
    }
}
