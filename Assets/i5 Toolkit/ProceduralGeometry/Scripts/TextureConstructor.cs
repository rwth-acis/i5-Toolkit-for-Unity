using i5.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace i5.Toolkit.ProceduralGeometry
{
    public class TextureConstructor : ITextureConstructor
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
            WebResponse<Texture2D> resp = await TextureLoader.LoadTextureAsync(LoadPath);
            if (resp.Successful)
            {
                return resp.Content;
            }
            else
            {
                i5Debug.LogError(resp.ErrorMessage, this);
                return null;
            }
        }
    }
}