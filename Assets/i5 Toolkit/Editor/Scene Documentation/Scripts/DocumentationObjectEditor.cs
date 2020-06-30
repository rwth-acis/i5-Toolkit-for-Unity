#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace i5.Toolkit.Core.SceneDocumentation
{
    /// <summary>
    /// Editor script which defines the inspector view for the DocumentationObject component
    /// </summary>
    [CustomEditor(typeof(DocumentationObject))]
    public class DocumentationObjectEditor : Editor
    {
        /// <summary>
        /// Called by the Unity Editor to create the inspector GUI
        /// </summary>
        public override void OnInspectorGUI()
        {
            // create the default GUI first, so that we do not need to worry about creating text fields for properties
            base.OnInspectorGUI();

            // Editor has a target of type object - we know that it must be a DocumentationObject, so we can cast it
            DocumentationObject documentationObject = (DocumentationObject)target;

            // append a button to the inspector view which can open the URL specified in the url field
            // if the button is pressed, the if statement becomes true
            if (GUILayout.Button("Open Documentation URL"))
            {
                // open the URL if it exists
                if (!string.IsNullOrEmpty(documentationObject.url))
                {
                    Application.OpenURL(documentationObject.url);
                }
            }
        }
    }
}
#endif