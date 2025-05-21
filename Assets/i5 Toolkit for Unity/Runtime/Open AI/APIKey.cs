using UnityEngine;

namespace i5.Toolkit.Core.OpenAI
{
    /// <summary>
    /// Retrives the API key from the Resources folder. You need to create this folder in Assets and name it EXEACTLY Resources.
    /// Then create a file called openAI.txt somewhere and import it into Resources. Finally, enter your API key into openAI.txt.
    /// Never commit this file to source control!
    /// </summary>
    public static class APIKey
    {
        private static string _key = "";

        public static string key
        {
            get
            {
                if (_key == "")
                {
                    TextAsset keyAsset = Resources.Load("openAI") as TextAsset;
                    if (keyAsset != null)
                    {
                        _key = keyAsset.text;
                        _key = _key.Replace("\n", "");
                    }
                    else
                    {
                        Debug.LogError("Create a Resources folder and import a file called openAI.txt containig your API key!");
                    }
                    
                }
                return _key;
            }
        }
    }
}