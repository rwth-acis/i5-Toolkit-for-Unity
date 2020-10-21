using UnityEngine;

namespace i5.Toolkit.Core.SceneConsole
{

    public abstract class VisibilityManager : MonoBehaviour, IVisibilityManager
    {
        public abstract bool IsVisible { get; set; }
    }
}