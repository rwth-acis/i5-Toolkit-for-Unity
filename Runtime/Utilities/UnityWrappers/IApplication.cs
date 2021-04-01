using System;

namespace i5.Toolkit.Core.Utilities.UnityAdapters
{
    public interface IApplication
    {
        event Action<string> DeepLinkActivated;
        string AbsoluteURL { get; }
    }
}
