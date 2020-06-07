using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Utilities
{
    public interface ITextureLoader
    {
        Task<WebResponse<Texture2D>> LoadTextureAsync(string uri);
    }
}