using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{

    public abstract class VisibilityManager : MonoBehaviour, IVisibilityManager
    {
        public abstract bool IsVisible { get; set; }
    }
}