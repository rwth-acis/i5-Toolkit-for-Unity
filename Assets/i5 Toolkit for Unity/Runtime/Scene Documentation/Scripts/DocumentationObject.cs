using UnityEngine;

namespace i5.Toolkit.Core.SceneDocumentation
{
    /// <summary>
    /// A special component which provides documentation information in the editor
    /// </summary>
    public class DocumentationObject : MonoBehaviour
    {
        /// <summary>
        /// The title of the documentation
        /// </summary>
        public string title;
        /// <summary>
        /// A description of the documented highlight
        /// </summary>
        [TextArea(3, 15)]
        public string description;
        /// <summary>
        /// The type of documentation (e.g. information, todo mark or bug mark)
        /// Based on this, the icon is changed
        /// </summary>
        public DocumentationType type;
        /// <summary>
        /// A url to further documentation on the matter
        /// </summary>
        public string url;

#if UNITY_EDITOR

        /// <summary>
        /// Called by the Unity Editor to draw gizmos
        /// Shows the corresponding icon at the position of the Transform
        /// </summary>
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
#endif
    }

    /// <summary>
    /// The different types of Documentation which are available
    /// </summary>
    public enum DocumentationType
    {
        INFO, TODO, BUG, NO_ICON
    }
}