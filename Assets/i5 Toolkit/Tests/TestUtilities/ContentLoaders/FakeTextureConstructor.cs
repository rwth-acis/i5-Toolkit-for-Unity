using i5.Toolkit.Core.ProceduralGeometry;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.TestUtilities
{
    /// <summary>
    /// Fake texture constructor which always returns a 2x2 texture
    /// </summary>
    public class FakeTextureConstructor : ITextureConstructor
    {
        /// <summary>
        /// Simulates a call to the texture constructor
        /// </summary>
        /// <returns>Returns a 2x2 texture</returns>
        public async Task<Texture2D> FetchTextureAsync()
        {
            await Task.Delay(1);
            return new Texture2D(2, 2);
        }
    }
}