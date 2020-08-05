using i5.Toolkit.Core.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.ProceduralGeometry
{
    /// <summary>
    /// Construction object to store properties of materials to create them later
    /// </summary>
    public class MaterialConstructor
    {
        /// <summary>
        /// The name of the shader that the material should use
        /// </summary>
        public string ShaderName { get; set; }

        /// <summary>
        /// The name of the material
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The main color of the material
        /// </summary>
        public Color Color { get; set; } = Color.white;

        /// <summary>
        /// Float values to store in the material
        /// Specify the property key and a float value
        /// </summary>
        private Dictionary<string, float> floatValues;

        /// <summary>
        /// Texture constructors in the material
        /// Specify the property key and a texture constructor
        /// </summary>
        private Dictionary<string, ITextureConstructor> textureConstructors;

        /// <summary>
        /// Fetched textures which are used in the material
        /// </summary>
        private Dictionary<string, Texture2D> textures;

        /// <summary>
        /// Initializes the material constructor with default values
        /// </summary>
        public MaterialConstructor() : this("Standard")
        {
        }

        /// <summary>
        /// Initializes the material constructor with a given shader
        /// </summary>
        /// <param name="shaderName">The name of the shader</param>
        public MaterialConstructor(string shaderName)
        {
            ShaderName = shaderName;
            Name = "New Material";
            floatValues = new Dictionary<string, float>();
            textureConstructors = new Dictionary<string, ITextureConstructor>();
        }

        /// <summary>
        /// Sets a float property in the material
        /// </summary>
        /// <param name="name">The name of the float property in the shader</param>
        /// <param name="value">The value of the float property</param>
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

        /// <summary>
        /// Sets a texture request for the material
        /// </summary>
        /// <param name="name">The name of the texture property in the shader</param>
        /// <param name="value">The texture constructor which fetches the texture</param>
        public void SetTexture(string name, ITextureConstructor value)
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

        /// <summary>
        /// Fetches any dependencies of the material, e.g. textures
        /// Call this method before constructing the material
        /// </summary>
        /// <returns>True if all dependencies could be fetched</returns>
        public async Task<bool> FetchDependencies()
        {
            // initialize the textures array if it is not yet created
            if (textures == null)
            {
                textures = new Dictionary<string, Texture2D>();
            }
            bool success = true;
            // go over all texture constructors and fetch their textures
            foreach (KeyValuePair<string, ITextureConstructor> tc in textureConstructors)
            {
                if (!textures.ContainsKey(tc.Key))
                {
                    Texture2D tex = await tc.Value.FetchTextureAsync();
                    if (tex != null)
                    {
                        textures.Add(tc.Key, tex);
                    }
                    else
                    {
                        success = false;
                    }
                }
            }
            return success;
        }

        /// <summary>
        /// Constructs a material from the specified data in this constructor
        /// </summary>
        /// <returns>Returns the constructed material</returns>
        public Material ConstructMaterial()
        {
            // create a material with the correct shader
            Material mat = new Material(Shader.Find(ShaderName));
            mat.name = Name;
            mat.color = Color;
            // set the float properties
            foreach (KeyValuePair<string, float> setCommand in floatValues)
            {
                mat.SetFloat(setCommand.Key, setCommand.Value);
            }
            // set the textures if they have been fetched
            if (textures != null)
            {
                foreach (KeyValuePair<string, Texture2D> textureEntry in textures)
                {
                    mat.SetTexture(textureEntry.Key, textureEntry.Value);
                }
            }
            // if textures have not been fetched but there are texture constructors: log a warning
            if (textures == null && textureConstructors.Count > 0)
            {
                i5Debug.LogWarning("Constructed material which has unfetched textures." + 
                    " Call FetchDependencies() to turn all TextureConstructors into Textures", this);
            }
            return mat;
        }
    }
}