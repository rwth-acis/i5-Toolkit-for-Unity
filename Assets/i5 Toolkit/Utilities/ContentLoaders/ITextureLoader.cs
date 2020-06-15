using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Utilities
{
    /// <summary>
    /// Interface for texture loaders which fetch textures from the Web
    /// </summary>
    public interface ITextureLoader
    {
        /// <summary>
        /// Loads a texture from the given uri
        /// Should be used asynchronously
        /// </summary>
        /// <param name="uri">The Uri where the texture is stored</param>
        /// <returns>Returns the texture</returns>
        Task<WebResponse<Texture2D>> LoadTextureAsync(string uri);
    }
}