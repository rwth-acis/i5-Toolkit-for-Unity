#if UNITY_EDITOR
using i5.Toolkit.Core.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace i5.Toolkit.Core.SceneDocumentation
{
    /// <summary>
    /// Initializes the scene documentation resources
    /// </summary>
    [InitializeOnLoad]
    public class SceneDocumentationInitializer
    {
        /// <summary>
        /// Static constructor which copies the gizmos files from the textures folder to the Assets/Gizmos folder
        /// </summary>
        static SceneDocumentationInitializer()
        {
            if (!Directory.Exists("Assets/Gizmos"))
            {
                Directory.CreateDirectory("Assets/Gizmos");
                Debug.Log("[SceneDocumentationInitializer] Created Gizmos folder");
            }

            string packageLocation = PackagePathUtils.GetPackagePath();

            EnsureFile(packageLocation, "Bug.png");
            EnsureFile(packageLocation, "Todo.png");
            EnsureFile(packageLocation, "Info.png");
        }

        /// <summary>
        /// Ensures that the file with the given name exists in the Gizmos folder
        /// If it does not exist, it is copied from the textures folder
        /// </summary>
        /// <param name="packageLocation">The path where the package is located in the project</param>
        /// <param name="filename">The filename of the gizmos icon</param>
        private static void EnsureFile(string packageLocation, string filename)
        {
            if (!File.Exists("Assets/Gizmos/" + filename))
            {
                File.Copy(packageLocation + "Editor/Scene Documentation/Textures/" + filename, "Assets/Gizmos/" + filename);
                Debug.Log("[SceneDocumentationInitializer] Copied " + filename + " to Gizmos folder");
            }
        }
    }
}
#endif