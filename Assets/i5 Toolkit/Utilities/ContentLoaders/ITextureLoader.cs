using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface ITextureLoader
{
    Task<WebResponse<Texture2D>> LoadTextureAsync(string uri);
}
