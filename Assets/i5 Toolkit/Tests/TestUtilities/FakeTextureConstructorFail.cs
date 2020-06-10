using i5.Toolkit.ProceduralGeometry;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.TestUtilities
{
    /// <summary>
    /// Simulates a texture constructor where texture fetching fails
    /// </summary>
    public class FakeTextureConstructorFail : ITextureConstructor
    {
        /// <summary>
        /// Simulates fetching the texture
        /// </summary>
        /// <returns>Returns null because the texture could not be fetched</returns>
        public async Task<Texture2D> FetchTextureAsync()
        {
            await Task.Delay(1);
            return null;
        }
    }
}