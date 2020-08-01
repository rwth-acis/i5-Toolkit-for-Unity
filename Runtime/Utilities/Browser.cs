using UnityEngine;

namespace i5.Toolkit.Core.Utilities
{
    public class Browser : IBrowser
    {
        public void OpenURL(string url)
        {
            Application.OpenURL(url);
        }
    }
}
