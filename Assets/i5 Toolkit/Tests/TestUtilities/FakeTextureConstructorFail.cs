using i5.Toolkit.ProceduralGeometry;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.TestUtilities
{
    public class FakeTextureConstructorFail : ITextureConstructor
    {
        public async Task<Texture2D> FetchTextureAsync()
        {
            await Task.Delay(1);
            return null;
        }
    }
}