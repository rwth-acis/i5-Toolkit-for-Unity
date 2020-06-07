using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.ProceduralGeometry
{
    public interface ITextureConstructor
    {
        Task<Texture2D> FetchTextureAsync();
    }
}