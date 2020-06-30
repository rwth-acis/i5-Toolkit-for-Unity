using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.ProceduralGeometry
{
    /// <summary>
    /// Interface for texture constructors
    /// </summary>
    public interface ITextureConstructor
    {
        /// <summary>
        /// Asynchronously fetches a texture
        /// </summary>
        /// <returns></returns>
        Task<Texture2D> FetchTextureAsync();
    }
}