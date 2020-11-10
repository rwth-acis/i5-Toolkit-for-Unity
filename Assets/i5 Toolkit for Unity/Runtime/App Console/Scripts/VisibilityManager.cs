using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    /// <summary>
    /// MonoBehaviour for managing an object's visiblity
    /// </summary>
    public abstract class VisibilityManager : MonoBehaviour, IVisibilityManager
    {
        public abstract bool IsVisible { get; set; }
    }
}