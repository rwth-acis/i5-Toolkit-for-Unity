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
        public DocumentationType type;
        public string url;

        private void OnDrawGizmos()
        {
            switch(type)
            {
                case DocumentationType.INFO:
                    Gizmos.DrawIcon(transform.position, "Info.png", true);
                    break;
                case DocumentationType.TODO:
                    Gizmos.DrawIcon(transform.position, "Todo.png", true);
                    break;
                case DocumentationType.BUG:
                    Gizmos.DrawIcon(transform.position, "Bug.png", true);
                    break;
            }
        }
    }

    public enum DocumentationType
    {
        INFO, TODO, BUG
    }
}