using i5.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace i5.Toolkit.ProceduralGeometry
{
    public class TextureConstructor
    {
        public string LoadPath { get; set; }

        public ITextureLoader TextureLoader { get; set; }

        public TextureConstructor(string loadPath)
        {
            LoadPath = loadPath;
            TextureLoader = new UnityTextureLoader();
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
}