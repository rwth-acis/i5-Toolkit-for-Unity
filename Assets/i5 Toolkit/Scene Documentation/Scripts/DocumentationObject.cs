using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.SceneDocumentation
{
    public class DocumentationObject : MonoBehaviour
    {
        public string title;
        [TextArea]
        public string description;
        public string url;
    }
}