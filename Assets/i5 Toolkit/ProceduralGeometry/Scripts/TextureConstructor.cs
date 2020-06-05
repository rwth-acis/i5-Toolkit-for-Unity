using i5.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class TextureConstructor
{
    public string LoadPath { get; set; }

    public TextureConstructor(string loadPath)
    {
        LoadPath = loadPath;
    }

    public async Task<Texture2D> FetchTextureAsync()
    {
        using (UnityWebRequest req = UnityWebRequestTexture.GetTexture(LoadPath))
        {
            await req.SendWebRequest();

            if (req.isNetworkError || req.isHttpError)
            {
                i5Debug.LogError("Error fetching texture: " + req.error, this);
                return null;
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(req);
                return texture;
            }
        }
    }
}
