using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace i5.Toolkit.SceneDocumentation
{
    [CustomEditor(typeof(DocumentationObject))]
    public class DocumentationObjectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DocumentationObject documentationObject = (DocumentationObject)target;

            if (GUILayout.Button("Open Documentation URL"))
            {
                if (!string.IsNullOrEmpty(documentationObject.url))
                {
                    Application.OpenURL(documentationObject.url);
                }
            }
        }
    }
}