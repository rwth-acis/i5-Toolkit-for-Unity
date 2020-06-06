using i5.Toolkit.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.ProceduralGeometry
{
    public class MaterialConstructor
    {
        public string ShaderName { get; set; }

        public string Name { get; set; }

        public Color Color { get; set; } = Color.white;

        private Dictionary<string, float> floatValues;

        private Dictionary<string, TextureConstructor> textureConstructors;
        private Dictionary<string, Texture2D> textures;



        public MaterialConstructor() : this("Standard")
        {
        }

        public MaterialConstructor(string shaderName)
        {
            ShaderName = shaderName;
            Name = "New Material";
            floatValues = new Dictionary<string, float>();
            textureConstructors = new Dictionary<string, TextureConstructor>();
        }

        public void SetFloat(string name, float value)
        {
            if (floatValues.ContainsKey(name))
            {
                floatValues[name] = value;
            }
            else
            {
                floatValues.Add(name, value);
            }
        }

        public void SetTexture(string name, TextureConstructor value)
        {
            if (textureConstructors.ContainsKey(name))
            {
                textureConstructors[name] = value;
            }
            else
            {
                textureConstructors.Add(name, value);
            }
        }

        public async Task FetchDependencies()
        {
            textures = new Dictionary<string, Texture2D>();
            foreach (KeyValuePair<string, TextureConstructor> tc in textureConstructors)
            {
                if (!textures.ContainsKey(tc.Key))
                {
                    Texture2D tex = await tc.Value.FetchTextureAsync();
                    if (tex != null)
                    {
                        textures.Add(tc.Key, tex);
                    }
                }
            }
        }

        public Material ConstructMaterial()
        {
            Material mat = new Material(Shader.Find(ShaderName));
            mat.name = Name;
            mat.color = Color;
            foreach (KeyValuePair<string, float> setCommand in floatValues)
            {
                mat.SetFloat(setCommand.Key, setCommand.Value);
            }
            if (textures != null)
            {
                foreach (KeyValuePair<string, Texture2D> textureEntry in textures)
                {
                    mat.SetTexture(textureEntry.Key, textureEntry.Value);
                }
            }
            if (textures == null && textureConstructors.Count > 0)
            {
                i5Debug.LogWarning("Constructed material which has unfetched textures." + 
                    " Call FetchDependencies() to turn all TextureConstructors into Textures", this);
            }
            return mat;
        }
    }
}