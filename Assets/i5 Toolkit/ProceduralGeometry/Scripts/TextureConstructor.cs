using i5.Toolkit.Utilities;
using i5.Toolkit.Utilities.ContentLoaders;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace i5.Toolkit.ProceduralGeometry
{
    /// <summary>
    /// Constructs a texture by fetching it from the Web
    /// </summary>
    public class TextureConstructor : ITextureConstructor
    {
        /// <summary>
        /// The path where the texture is stored
        /// </summary>
        public string LoadPath { get; set; }

        /// <summary>
        /// Module which loads the object
        /// </summary>
        public IContentLoader<Texture2D> TextureLoader { get; set; }

        /// <summary>
        /// Creates a texture constructor with the given load path
        /// </summary>
        /// <param name="loadPath">The path where the texture is stored</param>
        public TextureConstructor(string loadPath)
        {
            LoadPath = loadPath;
            TextureLoader = new UnityTextureLoader();
        }

        /// <summary>
        /// Fetches the texture from the Web based on the LoadPath
        /// </summary>
        /// <returns>Returns the fetched texture or null if something went wrong</returns>
        public async Task<Texture2D> FetchTextureAsync()
        {
            WebResponse<Texture2D> resp = await TextureLoader.LoadAsync(LoadPath);
            if (resp.Successful)
            {
                return resp.Content;
            }
            else
            {
                i5Debug.LogError(resp.ErrorMessage, this);
                return null;
            }
        }
    }
}